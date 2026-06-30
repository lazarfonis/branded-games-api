using AutoMapper;
using AutoMapper.Features;
using BrandedGames.Common.Enums;
using BrandedGames.Common.Exceptions;
using BrandedGames.Common.Helpers;
using BrandedGames.Common.Models;
using BrandedGames.Data;
using BrandedGames.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Core;

/// <summary>
/// Business logic and data access for <see cref="GameForm"/> entities, including
/// related features, platforms and uploaded files.
/// </summary>
public class GameFormManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;
    private readonly ICloudinaryFileManager fileManager;

    /// <summary>Creates a new <see cref="GameFormManager"/>.</summary>
    /// <param name="db">The database context.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="fileManager">The file storage manager used to upload game files.</param>
    public GameFormManager(BrandedGamesDbContext db, IMapper mapper, ICloudinaryFileManager fileManager)
    {
        this.db = db;
        this.mapper = mapper;
        this.fileManager = fileManager;
    }

    /// <summary>Gets all game forms with their features, platforms and files.</summary>
    /// <returns>The list of all game forms.</returns>
    public async Task<List<GameFormModel>> GetGames()
    {
        return await mapper.ProjectTo<GameFormModel>(db.GameForms).ToListAsync();
    }

    /// <summary>Gets the game forms submitted by a specific user.</summary>
    /// <param name="userId">The identifier of the submitting user.</param>
    /// <returns>The user's game forms.</returns>
    public async Task<List<GameFormModel>> GetMyGames(Guid userId)
    {
        return await mapper.ProjectTo<GameFormModel>(db.GameForms.Where(g => g.UserId == userId)).ToListAsync();
    }

    /// <summary>Gets a single game form by its identifier.</summary>
    /// <param name="id">The game form identifier.</param>
    /// <returns>The requested game form.</returns>
    public async Task<GameFormModel> GetGame(Guid id)
    {
        var game = await mapper.ProjectTo<GameFormModel>(db.GameForms.Where(g => g.Id == id)).FirstOrDefaultAsync();
        ValidationHelper.MustExist<GameForm>(game != null);
        return game;
    }

    /// <summary>Deletes a game form. Related features and platform links are removed by cascade.</summary>
    /// <param name="id">The identifier of the game form to delete.</param>
    public async Task DeleteGame(Guid id)
    {
        var game = await db.GameForms.FirstOrDefaultAsync(g => g.Id == id);
        ValidationHelper.MustExist(game);
        db.GameForms.Remove(game);
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Creates a new game form together with its selected features, target platforms and
    /// uploaded files. The whole operation runs in a single database transaction.
    /// </summary>
    /// <param name="model">The game configuration to create.</param>
    /// <param name="userId">Identifier of the user submitting the form, or null if anonymous.</param>
    public async Task Create(GameFormCreateModel model, Guid? userId = null)
    {
        var gameTypeExists = await db.GameTypes.AnyAsync(gt => gt.Id == model.GameTypeId);
        ValidationHelper.MustExist<GameType>(gameTypeExists);

        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            if (!model.FeatureIds.Any())
            {
                //throw ValidationException();
            }

            if (!model.PlatformTypeIds.Any())
            {
                //throw ValidationException();
            }

            var gameForm = new GameForm
            {
                GameTypeId = model.GameTypeId,
                UserId = userId,
                CustomerType = model.CustomerType,
                Price = model.Price,
                Items = model.Items,
                RewardTopPlayers = model.RewardTopPlayers,
                RewardPlacements = model.RewardPlacements,
                Rewards = model.Rewards
            };

            await db.GameForms.AddAsync(gameForm);
            await db.SaveChangesAsync();

            foreach (var featureId in model.FeatureIds)
            {
                var featureExists = await db.GameFeatures.AnyAsync(gf => gf.Id == featureId);
                ValidationHelper.MustExist<GameFeature>(featureExists);

                var gameFormFeature = new GameFormFeature
                {
                    GameFeatureId = featureId,
                    GameFormId = gameForm.Id
                };

                await db.GameFormFeatures.AddAsync(gameFormFeature);
            }

            foreach (var platformTypeId in model.PlatformTypeIds)
            {
                var platformTypeExists = await db.PlatformTypes.AnyAsync(pt => pt.Id == platformTypeId);
                ValidationHelper.MustExist<PlatformType>(platformTypeExists);

                var gameFormPlatformType = new GameFormPlatformType
                {
                    GameFormId = gameForm.Id,
                    PlatformTypeId = platformTypeId
                };

                await db.GameFormPlatformTypes.AddAsync(gameFormPlatformType);
            }

            foreach (var file in model.Files)
            {
                var gameFile = new GameFormFile
                {
                    GameFormId = gameForm.Id,
                    FileOriginalName = file.Name,
                    FileType = FileType.Image
                };

                await db.GameFormFiles.AddAsync(gameFile);

                var uploadResult = await fileManager.ProcessFileStorageUpload(file);
                gameFile.FileUrl = uploadResult.Url.ToString();
                gameFile.FileOriginalName = uploadResult.OriginalFilename;
            }

            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new ValidationException(ErrorCode.InternalServerError);
        }
    }
}
