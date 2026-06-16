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

public class GameTypeManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    public GameTypeManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<GameTypeModel>> GetTypes()
    {
        return await mapper.ProjectTo<GameTypeModel>(db.GameTypes).ToListAsync();
    }

    public async Task<GameTypeModel> GetType(Guid id)
    {
        var gameType = await db.GameTypes.FirstOrDefaultAsync(t => t.Id == id);
        ValidationHelper.MustExist(gameType);
        return mapper.Map<GameTypeModel>(gameType);
    }

    public async Task<GameTypeModel> CreateType(GameTypeCreateModel model)
    {
        var gameType = mapper.Map<GameType>(model);
        await db.GameTypes.AddAsync(gameType);
        await db.SaveChangesAsync();
        return mapper.Map<GameTypeModel>(gameType);
    }

    public async Task<GameTypeModel> UpdateType(Guid id, GameTypeUpdateModel model)
    {
        var gameType = await db.GameTypes.FirstOrDefaultAsync(t => t.Id == id);
        ValidationHelper.MustExist(gameType);
        mapper.Map(model, gameType);
        await db.SaveChangesAsync();
        return mapper.Map<GameTypeModel>(gameType);
    }

    public async Task DeleteType(Guid id)
    {
        var gameType = await db.GameTypes.FirstOrDefaultAsync(t => t.Id == id);
        ValidationHelper.MustExist(gameType);
        db.GameTypes.Remove(gameType);
        await db.SaveChangesAsync();
    }
}
