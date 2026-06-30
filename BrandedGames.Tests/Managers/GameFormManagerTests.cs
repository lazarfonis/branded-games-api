using BrandedGames.Common.Enums;
using BrandedGames.Common.Exceptions;
using BrandedGames.Common.Models;
using BrandedGames.Core;
using BrandedGames.Data;
using BrandedGames.Entities;
using BrandedGames.Tests.Infrastructure;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace BrandedGames.Tests.Managers;

public class GameFormManagerTests
{
    private static GameFormManager CreateManager(BrandedGamesDbContext db, ICloudinaryFileManager fileManager = null)
        => new(db, TestMapper.Create(), fileManager ?? Mock.Of<ICloudinaryFileManager>());

    private static GameType SeedGameType(BrandedGamesDbContext db, string name = "Quiz")
    {
        var gameType = new GameType { Id = Guid.NewGuid(), Name = name };
        db.GameTypes.Add(gameType);
        return gameType;
    }

    [Fact]
    public async Task GetGames_ReturnsAllGames_WithProjectedTypeName()
    {
        using var db = TestDbContextFactory.Create();
        var gameType = SeedGameType(db, "Quiz");
        db.GameForms.Add(new GameForm { Id = Guid.NewGuid(), GameTypeId = gameType.Id, GameType = gameType, CustomerType = CustomerType.Brand });
        db.GameForms.Add(new GameForm { Id = Guid.NewGuid(), GameTypeId = gameType.Id, GameType = gameType, CustomerType = CustomerType.IndividualUser });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetGames();

        Assert.Equal(2, result.Count);
        Assert.All(result, g => Assert.Equal("Quiz", g.GameTypeName));
    }

    [Fact]
    public async Task GetMyGames_ReturnsOnlyGamesOwnedByUser()
    {
        using var db = TestDbContextFactory.Create();
        var gameType = SeedGameType(db);
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        db.GameForms.Add(new GameForm { Id = Guid.NewGuid(), GameTypeId = gameType.Id, GameType = gameType, UserId = userId });
        db.GameForms.Add(new GameForm { Id = Guid.NewGuid(), GameTypeId = gameType.Id, GameType = gameType, UserId = otherUserId });
        db.GameForms.Add(new GameForm { Id = Guid.NewGuid(), GameTypeId = gameType.Id, GameType = gameType, UserId = null });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetMyGames(userId);

        var game = Assert.Single(result);
        Assert.Equal(userId, game.UserId);
    }

    [Fact]
    public async Task GetGame_WhenExists_ReturnsProjectedGame()
    {
        using var db = TestDbContextFactory.Create();
        var gameType = SeedGameType(db, "Memory");
        var id = Guid.NewGuid();
        db.GameForms.Add(new GameForm { Id = id, GameTypeId = gameType.Id, GameType = gameType, CustomerType = CustomerType.Brand, Price = 1000 });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        var result = await manager.GetGame(id);

        Assert.Equal(id, result.Id);
        Assert.Equal("Memory", result.GameTypeName);
        Assert.Equal(1000, result.Price);
    }

    [Fact]
    public async Task GetGame_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.GetGame(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteGame_WhenExists_RemovesGame()
    {
        using var db = TestDbContextFactory.Create();
        var gameType = SeedGameType(db);
        var id = Guid.NewGuid();
        db.GameForms.Add(new GameForm { Id = id, GameTypeId = gameType.Id });
        await db.SaveChangesAsync();
        var manager = CreateManager(db);

        await manager.DeleteGame(id);

        Assert.Empty(db.GameForms);
    }

    [Fact]
    public async Task DeleteGame_WhenMissing_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);

        await Assert.ThrowsAsync<NotFoundException>(() => manager.DeleteGame(Guid.NewGuid()));
    }

    [Fact]
    public async Task Create_WithValidData_PersistsGameWithLinksAndUploadedFiles()
    {
        using var db = TestDbContextFactory.Create();
        var gameType = SeedGameType(db);
        var feature = new GameFeature { Id = Guid.NewGuid(), Name = "Leaderboard" };
        var platform = new PlatformType { Id = Guid.NewGuid(), Name = "Web" };
        db.GameFeatures.Add(feature);
        db.PlatformTypes.Add(platform);
        await db.SaveChangesAsync();

        var fileMock = new Mock<IFormFile>();
        fileMock.SetupGet(f => f.Name).Returns("logo.png");

        var fileManagerMock = new Mock<ICloudinaryFileManager>();
        fileManagerMock
            .Setup(m => m.ProcessFileStorageUpload(It.IsAny<IFormFile>()))
            .ReturnsAsync(new ImageUploadResult
            {
                Url = new Uri("https://cdn.example.com/logo.png"),
                OriginalFilename = "logo.png"
            });

        var manager = CreateManager(db, fileManagerMock.Object);
        var model = new GameFormCreateModel
        {
            GameTypeId = gameType.Id,
            CustomerType = CustomerType.Brand,
            Items = "prizes",
            FeatureIds = { feature.Id },
            PlatformTypeIds = { platform.Id },
            Files = { fileMock.Object }
        };

        await manager.Create(model);

        var persistedGame = Assert.Single(db.GameForms);
        Assert.Equal(gameType.Id, persistedGame.GameTypeId);
        Assert.Equal(CustomerType.Brand, persistedGame.CustomerType);
        Assert.Single(db.GameFormFeatures);
        Assert.Single(db.GameFormPlatformTypes);
        var persistedFile = Assert.Single(db.GameFormFiles);
        Assert.Equal("https://cdn.example.com/logo.png", persistedFile.FileUrl);
        Assert.Equal("logo.png", persistedFile.FileOriginalName);
        Assert.Equal(FileType.Image, persistedFile.FileType);
        fileManagerMock.Verify(m => m.ProcessFileStorageUpload(It.IsAny<IFormFile>()), Times.Once);
    }

    [Fact]
    public async Task Create_WithUser_PersistsOwner()
    {
        using var db = TestDbContextFactory.Create();
        var gameType = SeedGameType(db);
        await db.SaveChangesAsync();
        var userId = Guid.NewGuid();
        var manager = CreateManager(db);
        var model = new GameFormCreateModel
        {
            GameTypeId = gameType.Id,
            CustomerType = CustomerType.Brand
        };

        await manager.Create(model, userId);

        var persistedGame = Assert.Single(db.GameForms);
        Assert.Equal(userId, persistedGame.UserId);
    }

    [Fact]
    public async Task Create_WithoutUser_LeavesOwnerNull()
    {
        using var db = TestDbContextFactory.Create();
        var gameType = SeedGameType(db);
        await db.SaveChangesAsync();
        var manager = CreateManager(db);
        var model = new GameFormCreateModel
        {
            GameTypeId = gameType.Id,
            CustomerType = CustomerType.Brand
        };

        await manager.Create(model);

        var persistedGame = Assert.Single(db.GameForms);
        Assert.Null(persistedGame.UserId);
    }

    [Fact]
    public async Task Create_WithMissingGameType_ThrowsNotFound()
    {
        using var db = TestDbContextFactory.Create();
        var manager = CreateManager(db);
        var model = new GameFormCreateModel { GameTypeId = Guid.NewGuid(), CustomerType = CustomerType.Brand };

        await Assert.ThrowsAsync<NotFoundException>(() => manager.Create(model));
    }
}
