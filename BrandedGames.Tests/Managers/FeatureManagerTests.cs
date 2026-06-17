using BrandedGames.Common.Exceptions;
using BrandedGames.Common.Models;
using BrandedGames.Core;
using BrandedGames.Entities;
using BrandedGames.Tests.Infrastructure;
using Xunit;

namespace BrandedGames.Tests.Managers;

public class FeatureManagerTests
{
    private static FeatureManager CreateManager(BrandedGames.Data.BrandedGamesDbContext db)
        => new(db, TestMapper.Create());

    [Fact]
    public async Task GetFeatures_ReturnsAllFeatures()
    {
        using var db = TestDbContextFactory.Create();
        db.GameFeatures.Add(new GameFeature { Id = Guid.NewGuid(), Name = "Leaderboard" });
        db.GameFeatures.Add(new GameFeature { Id = Guid.NewGuid(), Name = "Timer" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetFeatures();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Name == "Leaderboard");
        Assert.Contains(result, f => f.Name == "Timer");
    }

    [Fact]
    public async Task GetFeature_WhenExists_ReturnsFeature()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.GameFeatures.Add(new GameFeature { Id = id, Name = "Leaderboard", Price = 500 });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetFeature(id);

        Assert.Equal(id, result.Id);
        Assert.Equal("Leaderboard", result.Name);
        Assert.Equal(500, result.Price);
    }

    [Fact]
    public async Task GetFeature_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.GetFeature(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateFeature_PersistsAndReturnsFeature()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);
        var model = new FeatureCreateModel { Name = "Confetti", IconName = "party", Price = 200, Description = "Celebrate" };

        var result = await manager.CreateFeature(model);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Confetti", result.Name);
        var persisted = Assert.Single(db.GameFeatures);
        Assert.Equal("Confetti", persisted.Name);
        Assert.Equal(200, persisted.Price);
    }

    [Fact]
    public async Task UpdateFeature_WhenExists_UpdatesValues()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.GameFeatures.Add(new GameFeature { Id = id, Name = "Old", Price = 100 });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);
        var model = new FeatureUpdateModel { Name = "New", IconName = "star", Price = 999, Description = "Updated" };

        var result = await manager.UpdateFeature(id, model);

        Assert.Equal("New", result.Name);
        Assert.Equal(999, result.Price);
        var persisted = await db.GameFeatures.FindAsync(id);
        Assert.Equal("New", persisted.Name);
        Assert.Equal(999, persisted.Price);
    }

    [Fact]
    public async Task UpdateFeature_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);
        var model = new FeatureUpdateModel { Name = "New" };

        await Assert.ThrowsAsync<NotFoundException>(() => manager.UpdateFeature(Guid.NewGuid(), model));
    }

    [Fact]
    public async Task DeleteFeature_WhenExists_RemovesFeature()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.GameFeatures.Add(new GameFeature { Id = id, Name = "Leaderboard" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        await manager.DeleteFeature(id);

        Assert.Empty(db.GameFeatures);
    }

    [Fact]
    public async Task DeleteFeature_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.DeleteFeature(Guid.NewGuid()));
    }
}
