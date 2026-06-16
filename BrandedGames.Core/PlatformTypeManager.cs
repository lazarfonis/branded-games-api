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
/// Business logic and data access for <see cref="PlatformType"/> entities.
/// </summary>
public class PlatformTypeManager
{
    private readonly BrandedGamesDbContext db;
    private readonly IMapper mapper;

    /// <summary>Creates a new <see cref="PlatformTypeManager"/>.</summary>
    /// <param name="db">The database context.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public PlatformTypeManager(BrandedGamesDbContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    /// <summary>Gets all platform types.</summary>
    /// <returns>The list of all platform types.</returns>
    public async Task<List<PlatformTypeModel>> GetPlatforms()
    {
        return await mapper.ProjectTo<PlatformTypeModel>(db.PlatformTypes).ToListAsync();
    }

    /// <summary>Gets a single platform type by its identifier.</summary>
    /// <param name="id">The platform type identifier.</param>
    /// <returns>The requested platform type.</returns>
    public async Task<PlatformTypeModel> GetPlatform(Guid id)
    {
        var platform = await db.PlatformTypes.FirstOrDefaultAsync(p => p.Id == id);
        ValidationHelper.MustExist(platform);
        return mapper.Map<PlatformTypeModel>(platform);
    }

    /// <summary>Creates a new platform type.</summary>
    /// <param name="model">The platform type to create.</param>
    /// <returns>The created platform type.</returns>
    public async Task<PlatformTypeModel> CreatePlatform(PlatformTypeCreateModel model)
    {
        var platform = mapper.Map<PlatformType>(model);
        await db.PlatformTypes.AddAsync(platform);
        await db.SaveChangesAsync();
        return mapper.Map<PlatformTypeModel>(platform);
    }

    /// <summary>Updates an existing platform type.</summary>
    /// <param name="id">The identifier of the platform type to update.</param>
    /// <param name="model">The new platform type values.</param>
    /// <returns>The updated platform type.</returns>
    public async Task<PlatformTypeModel> UpdatePlatform(Guid id, PlatformTypeUpdateModel model)
    {
        var platform = await db.PlatformTypes.FirstOrDefaultAsync(p => p.Id == id);
        ValidationHelper.MustExist(platform);
        mapper.Map(model, platform);
        await db.SaveChangesAsync();
        return mapper.Map<PlatformTypeModel>(platform);
    }

    /// <summary>Deletes a platform type.</summary>
    /// <param name="id">The identifier of the platform type to delete.</param>
    public async Task DeletePlatform(Guid id)
    {
        var platform = await db.PlatformTypes.FirstOrDefaultAsync(p => p.Id == id);
        ValidationHelper.MustExist(platform);
        db.PlatformTypes.Remove(platform);
        await db.SaveChangesAsync();
    }
}
