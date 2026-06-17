using BrandedGames.Entities;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class GameTypeTests
{
    [Fact]
    public void GameForms_Collection_IsInitialized_AndEmpty_ByDefault()
    {
        var gameType = new GameType();

        Assert.NotNull(gameType.GameForms);
        Assert.Empty(gameType.GameForms);
    }

    [Fact]
    public void Properties_RoundTrip()
    {
        var id = Guid.NewGuid();

        var gameType = new GameType
        {
            Id = id,
            Name = "Memory",
            IconName = "cards"
        };

        Assert.Equal(id, gameType.Id);
        Assert.Equal("Memory", gameType.Name);
        Assert.Equal("cards", gameType.IconName);
    }

    [Fact]
    public void GameForms_Collection_AcceptsItems()
    {
        var gameType = new GameType();

        gameType.GameForms.Add(new GameForm());

        Assert.Single(gameType.GameForms);
    }
}
