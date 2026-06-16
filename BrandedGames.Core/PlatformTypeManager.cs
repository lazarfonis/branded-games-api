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

public class PlatformTypeManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    public PlatformTypeManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<PlatformTypeModel>> GetPlatforms()
    {
        return await mapper.ProjectTo<PlatformTypeModel>(db.PlatformTypes).ToListAsync();
    }

    public async Task<PlatformTypeModel> GetPlatform(Guid id)
    {
        var platform = await db.PlatformTypes.FirstOrDefaultAsync(p => p.Id == id);
        ValidationHelper.MustExist(platform);
        return mapper.Map<PlatformTypeModel>(platform);
    }

    public async Task<PlatformTypeModel> CreatePlatform(PlatformTypeCreateModel model)
    {
        var platform = mapper.Map<PlatformType>(model);
        await db.PlatformTypes.AddAsync(platform);
        await db.SaveChangesAsync();
        return mapper.Map<PlatformTypeModel>(platform);
    }

    public async Task<PlatformTypeModel> UpdatePlatform(Guid id, PlatformTypeUpdateModel model)
    {
        var platform = await db.PlatformTypes.FirstOrDefaultAsync(p => p.Id == id);
        ValidationHelper.MustExist(platform);
        mapper.Map(model, platform);
        await db.SaveChangesAsync();
        return mapper.Map<PlatformTypeModel>(platform);
    }

    public async Task DeletePlatform(Guid id)
    {
        var platform = await db.PlatformTypes.FirstOrDefaultAsync(p => p.Id == id);
        ValidationHelper.MustExist(platform);
        db.PlatformTypes.Remove(platform);
        await db.SaveChangesAsync();
    }
}
