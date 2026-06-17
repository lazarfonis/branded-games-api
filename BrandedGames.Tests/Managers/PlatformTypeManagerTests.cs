using BrandedGames.Common.Exceptions;
using BrandedGames.Common.Models;
using BrandedGames.Core;
using BrandedGames.Entities;
using BrandedGames.Tests.Infrastructure;
using Xunit;

namespace BrandedGames.Tests.Managers;

public class PlatformTypeManagerTests
{
    private static PlatformTypeManager CreateManager(BrandedGames.Data.BrandedGamesDbContext db)
        => new(db, TestMapper.Create());

    [Fact]
    public async Task GetPlatforms_ReturnsAllPlatforms()
    {
        using var db = TestDbContextFactory.Create();
        db.PlatformTypes.Add(new PlatformType { Id = Guid.NewGuid(), Name = "Web" });
        db.PlatformTypes.Add(new PlatformType { Id = Guid.NewGuid(), Name = "Mobile" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetPlatforms();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Web");
        Assert.Contains(result, p => p.Name == "Mobile");
    }

    [Fact]
    public async Task GetPlatform_WhenExists_ReturnsPlatform()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.PlatformTypes.Add(new PlatformType { Id = id, Name = "Web", Description = "Browser" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetPlatform(id);

        Assert.Equal(id, result.Id);
        Assert.Equal("Web", result.Name);
        Assert.Equal("Browser", result.Description);
    }

    [Fact]
    public async Task GetPlatform_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.GetPlatform(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreatePlatform_PersistsAndReturnsPlatform()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);
        var model = new PlatformTypeCreateModel { Name = "Kiosk", IconName = "screen", Description = "On-site" };

        var result = await manager.CreatePlatform(model);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Kiosk", result.Name);
        var persisted = Assert.Single(db.PlatformTypes);
        Assert.Equal("Kiosk", persisted.Name);
        Assert.Equal("On-site", persisted.Description);
    }

    [Fact]
    public async Task UpdatePlatform_WhenExists_UpdatesValues()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.PlatformTypes.Add(new PlatformType { Id = id, Name = "Old", Description = "old" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);
        var model = new PlatformTypeUpdateModel { Name = "New", IconName = "new", Description = "new desc" };

        var result = await manager.UpdatePlatform(id, model);

        Assert.Equal("New", result.Name);
        var persisted = await db.PlatformTypes.FindAsync(id);
        Assert.Equal("New", persisted.Name);
        Assert.Equal("new desc", persisted.Description);
    }

    [Fact]
    public async Task UpdatePlatform_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.UpdatePlatform(Guid.NewGuid(), new PlatformTypeUpdateModel { Name = "x" }));
    }

    [Fact]
    public async Task DeletePlatform_WhenExists_RemovesPlatform()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.PlatformTypes.Add(new PlatformType { Id = id, Name = "Web" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        await manager.DeletePlatform(id);

        Assert.Empty(db.PlatformTypes);
    }

    [Fact]
    public async Task DeletePlatform_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.DeletePlatform(Guid.NewGuid()));
    }
}
