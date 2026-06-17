using BrandedGames.Common.Exceptions;
using BrandedGames.Common.Models;
using BrandedGames.Core;
using BrandedGames.Entities;
using BrandedGames.Tests.Infrastructure;
using Xunit;

namespace BrandedGames.Tests.Managers;

public class GameTypeManagerTests
{
    private static GameTypeManager CreateManager(BrandedGames.Data.BrandedGamesDbContext db)
        => new(db, TestMapper.Create());

    [Fact]
    public async Task GetTypes_ReturnsAllTypes()
    {
        using var db = TestDbContextFactory.Create();
        db.GameTypes.Add(new GameType { Id = Guid.NewGuid(), Name = "Quiz" });
        db.GameTypes.Add(new GameType { Id = Guid.NewGuid(), Name = "Memory" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetTypes();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Name == "Quiz");
        Assert.Contains(result, t => t.Name == "Memory");
    }

    [Fact]
    public async Task GetType_WhenExists_ReturnsType()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.GameTypes.Add(new GameType { Id = id, Name = "Quiz", IconName = "question" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetType(id);

        Assert.Equal(id, result.Id);
        Assert.Equal("Quiz", result.Name);
        Assert.Equal("question", result.IconName);
    }

    [Fact]
    public async Task GetType_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.GetType(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateType_PersistsAndReturnsType()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);
        var model = new GameTypeCreateModel { Name = "Puzzle", IconName = "grid" };

        var result = await manager.CreateType(model);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Puzzle", result.Name);
        var persisted = Assert.Single(db.GameTypes);
        Assert.Equal("Puzzle", persisted.Name);
        Assert.Equal("grid", persisted.IconName);
    }

    [Fact]
    public async Task UpdateType_WhenExists_UpdatesValues()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.GameTypes.Add(new GameType { Id = id, Name = "Old", IconName = "old" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);
        var model = new GameTypeUpdateModel { Name = "New", IconName = "new" };

        var result = await manager.UpdateType(id, model);

        Assert.Equal("New", result.Name);
        var persisted = await db.GameTypes.FindAsync(id);
        Assert.Equal("New", persisted.Name);
        Assert.Equal("new", persisted.IconName);
    }

    [Fact]
    public async Task UpdateType_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.UpdateType(Guid.NewGuid(), new GameTypeUpdateModel { Name = "x" }));
    }

    [Fact]
    public async Task DeleteType_WhenExists_RemovesType()
    {
        using var db = TestDbContextFactory.Create();
        var id = Guid.NewGuid();
        db.GameTypes.Add(new GameType { Id = id, Name = "Quiz" });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        await manager.DeleteType(id);

        Assert.Empty(db.GameTypes);
    }

    [Fact]
    public async Task DeleteType_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.DeleteType(Guid.NewGuid()));
    }
}
