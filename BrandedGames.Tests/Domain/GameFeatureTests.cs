using BrandedGames.Entities;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class GameFeatureTests
{
    [Fact]
    public void Features_Collection_IsInitialized_AndEmpty_ByDefault()
    {
        var feature = new GameFeature();

        Assert.NotNull(feature.Features);
        Assert.Empty(feature.Features);
    }

    [Fact]
    public void Properties_RoundTrip()
    {
        var id = Guid.NewGuid();

        var feature = new GameFeature
        {
            Id = id,
            Name = "Leaderboard",
            IconName = "trophy",
            Price = 500,
            Description = "Ranks players by score"
        };

        Assert.Equal(id, feature.Id);
        Assert.Equal("Leaderboard", feature.Name);
        Assert.Equal("trophy", feature.IconName);
        Assert.Equal(500, feature.Price);
        Assert.Equal("Ranks players by score", feature.Description);
    }

    [Fact]
    public void Features_Collection_AcceptsItems()
    {
        var feature = new GameFeature();

        feature.Features.Add(new GameFormFeature());

        Assert.Single(feature.Features);
    }
}
