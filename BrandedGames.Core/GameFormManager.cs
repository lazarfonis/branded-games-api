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

public class GameFormManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;
    private readonly CloudinaryFileManager fileManager;

    public GameFormManager(BrandedGamesDbContext db, IMapper mapper, CloudinaryFileManager fileManager)
    {
        this.db = db;
        this.mapper = mapper;
        this.fileManager = fileManager;
    }

    public async Task<List<GameFormModel>> GetGames()
    {
        return await mapper.ProjectTo<GameFormModel>(db.GameForms).ToListAsync();
    }

    public async Task<GameFormModel> GetGame(Guid id)
    {
        var game = await mapper.ProjectTo<GameFormModel>(db.GameForms.Where(g => g.Id == id)).FirstOrDefaultAsync();
        ValidationHelper.MustExist<GameForm>(game != null);
        return game;
    }

    public async Task DeleteGame(Guid id)
    {
        var game = await db.GameForms.FirstOrDefaultAsync(g => g.Id == id);
        ValidationHelper.MustExist(game);
        db.GameForms.Remove(game);
        await db.SaveChangesAsync();
    }

    public async Task Create(GameFormCreateModel model)
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
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new ValidationException(ErrorCode.InternalServerError);
        }
    }
}
