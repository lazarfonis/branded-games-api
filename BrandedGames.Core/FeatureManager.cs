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

public class FeatureManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    public FeatureManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<FeatureModel>> GetFeatures()
    {
        return await mapper.ProjectTo<FeatureModel>(db.GameFeatures).ToListAsync();
    }

    public async Task<FeatureModel> GetFeature(Guid id)
    {
        var feature = await db.GameFeatures.FirstOrDefaultAsync(f => f.Id == id);
        ValidationHelper.MustExist(feature);
        return mapper.Map<FeatureModel>(feature);
    }

    public async Task<FeatureModel> CreateFeature(FeatureCreateModel model)
    {
        var feature = mapper.Map<GameFeature>(model);
        await db.GameFeatures.AddAsync(feature);
        await db.SaveChangesAsync();
        return mapper.Map<FeatureModel>(feature);
    }

    public async Task<FeatureModel> UpdateFeature(Guid id, FeatureUpdateModel model)
    {
        var feature = await db.GameFeatures.FirstOrDefaultAsync(f => f.Id == id);
        ValidationHelper.MustExist(feature);
        mapper.Map(model, feature);
        await db.SaveChangesAsync();
        return mapper.Map<FeatureModel>(feature);
    }

    public async Task DeleteFeature(Guid id)
    {
        var feature = await db.GameFeatures.FirstOrDefaultAsync(f => f.Id == id);
        ValidationHelper.MustExist(feature);
        db.GameFeatures.Remove(feature);
        await db.SaveChangesAsync();
    }
}
