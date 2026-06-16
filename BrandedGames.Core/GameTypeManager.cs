using AutoMapper;
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
/// Business logic and data access for <see cref="GameType"/> entities.
/// </summary>
public class GameTypeManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    /// <summary>Creates a new <see cref="GameTypeManager"/>.</summary>
    /// <param name="db">The database context.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public GameTypeManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    /// <summary>Gets all game types.</summary>
    /// <returns>The list of all game types.</returns>
    public async Task<List<GameTypeModel>> GetTypes()
    {
        return await mapper.ProjectTo<GameTypeModel>(db.GameTypes).ToListAsync();
    }

    /// <summary>Gets a single game type by its identifier.</summary>
    /// <param name="id">The game type identifier.</param>
    /// <returns>The requested game type.</returns>
    public async Task<GameTypeModel> GetType(Guid id)
    {
        var gameType = await db.GameTypes.FirstOrDefaultAsync(t => t.Id == id);
        ValidationHelper.MustExist(gameType);
        return mapper.Map<GameTypeModel>(gameType);
    }

    /// <summary>Creates a new game type.</summary>
    /// <param name="model">The game type to create.</param>
    /// <returns>The created game type.</returns>
    public async Task<GameTypeModel> CreateType(GameTypeCreateModel model)
    {
        var gameType = mapper.Map<GameType>(model);
        await db.GameTypes.AddAsync(gameType);
        await db.SaveChangesAsync();
        return mapper.Map<GameTypeModel>(gameType);
    }

    /// <summary>Updates an existing game type.</summary>
    /// <param name="id">The identifier of the game type to update.</param>
    /// <param name="model">The new game type values.</param>
    /// <returns>The updated game type.</returns>
    public async Task<GameTypeModel> UpdateType(Guid id, GameTypeUpdateModel model)
    {
        var gameType = await db.GameTypes.FirstOrDefaultAsync(t => t.Id == id);
        ValidationHelper.MustExist(gameType);
        mapper.Map(model, gameType);
        await db.SaveChangesAsync();
        return mapper.Map<GameTypeModel>(gameType);
    }

    /// <summary>Deletes a game type.</summary>
    /// <param name="id">The identifier of the game type to delete.</param>
    public async Task DeleteType(Guid id)
    {
        var gameType = await db.GameTypes.FirstOrDefaultAsync(t => t.Id == id);
        ValidationHelper.MustExist(gameType);
        db.GameTypes.Remove(gameType);
        await db.SaveChangesAsync();
    }
}
