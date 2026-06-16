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
/// Business logic and data access for <see cref="GameFeature"/> entities.
/// </summary>
public class FeatureManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    /// <summary>Creates a new <see cref="FeatureManager"/>.</summary>
    /// <param name="db">The database context.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public FeatureManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    /// <summary>Gets all features.</summary>
    /// <returns>The list of all features.</returns>
    public async Task<List<FeatureModel>> GetFeatures()
    {
        return await mapper.ProjectTo<FeatureModel>(db.GameFeatures).ToListAsync();
    }

    /// <summary>Gets a single feature by its identifier.</summary>
    /// <param name="id">The feature identifier.</param>
    /// <returns>The requested feature.</returns>
    public async Task<FeatureModel> GetFeature(Guid id)
    {
        var feature = await db.GameFeatures.FirstOrDefaultAsync(f => f.Id == id);
        ValidationHelper.MustExist(feature);
        return mapper.Map<FeatureModel>(feature);
    }

    /// <summary>Creates a new feature.</summary>
    /// <param name="model">The feature to create.</param>
    /// <returns>The created feature.</returns>
    public async Task<FeatureModel> CreateFeature(FeatureCreateModel model)
    {
        var feature = mapper.Map<GameFeature>(model);
        await db.GameFeatures.AddAsync(feature);
        await db.SaveChangesAsync();
        return mapper.Map<FeatureModel>(feature);
    }

    /// <summary>Updates an existing feature.</summary>
    /// <param name="id">The identifier of the feature to update.</param>
    /// <param name="model">The new feature values.</param>
    /// <returns>The updated feature.</returns>
    public async Task<FeatureModel> UpdateFeature(Guid id, FeatureUpdateModel model)
    {
        var feature = await db.GameFeatures.FirstOrDefaultAsync(f => f.Id == id);
        ValidationHelper.MustExist(feature);
        mapper.Map(model, feature);
        await db.SaveChangesAsync();
        return mapper.Map<FeatureModel>(feature);
    }

    /// <summary>Deletes a feature.</summary>
    /// <param name="id">The identifier of the feature to delete.</param>
    public async Task DeleteFeature(Guid id)
    {
        var feature = await db.GameFeatures.FirstOrDefaultAsync(f => f.Id == id);
        ValidationHelper.MustExist(feature);
        db.GameFeatures.Remove(feature);
        await db.SaveChangesAsync();
    }
}
