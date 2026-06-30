using BrandedGames.Common.Enums;
using BrandedGames.Entities;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class GameFormTests
{
    [Fact]
    public void Collections_AreInitialized_AndEmpty_ByDefault()
    {
        var gameForm = new GameForm();

        Assert.NotNull(gameForm.Features);
        Assert.Empty(gameForm.Features);
        Assert.NotNull(gameForm.Files);
        Assert.Empty(gameForm.Files);
        Assert.NotNull(gameForm.Platforms);
        Assert.Empty(gameForm.Platforms);
    }

    [Fact]
    public void Properties_RoundTrip()
    {
        var id = Guid.NewGuid();
        var gameTypeId = Guid.NewGuid();
        var gameType = new GameType { Name = "Quiz" };

        var gameForm = new GameForm
        {
            Id = id,
            GameTypeId = gameTypeId,
            CustomerType = CustomerType.Brand,
            Price = 1000,
            Items = "prizes",
            RewardTopPlayers = true,
            Rewards = "gift cards",
            RewardPlacements = "top 3",
            GameType = gameType
        };

        Assert.Equal(id, gameForm.Id);
        Assert.Equal(gameTypeId, gameForm.GameTypeId);
        Assert.Equal(CustomerType.Brand, gameForm.CustomerType);
        Assert.Equal(1000, gameForm.Price);
        Assert.Equal("prizes", gameForm.Items);
        Assert.True(gameForm.RewardTopPlayers);
        Assert.Equal("gift cards", gameForm.Rewards);
        Assert.Equal("top 3", gameForm.RewardPlacements);
        Assert.Same(gameType, gameForm.GameType);
    }

    [Fact]
    public void Price_IsNullable()
    {
        var gameForm = new GameForm { Price = null };

        Assert.Null(gameForm.Price);
    }

    [Fact]
    public void Collections_AcceptItems()
    {
        var gameForm = new GameForm();

        gameForm.Features.Add(new GameFormFeature());
        gameForm.Files.Add(new GameFormFile());
        gameForm.Platforms.Add(new GameFormPlatformType());

        Assert.Single(gameForm.Features);
        Assert.Single(gameForm.Files);
        Assert.Single(gameForm.Platforms);
    }
}
