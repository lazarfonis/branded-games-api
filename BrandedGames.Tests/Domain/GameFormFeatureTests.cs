using BrandedGames.Entities;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class GameFormFeatureTests
{
    [Fact]
    public void Properties_RoundTrip()
    {
        var gameFeatureId = Guid.NewGuid();
        var gameFormId = Guid.NewGuid();
        var gameFeature = new GameFeature { Id = gameFeatureId };
        var gameForm = new GameForm { Id = gameFormId };

        var link = new GameFormFeature
        {
            GameFeatureId = gameFeatureId,
            GameFormId = gameFormId,
            GameFeature = gameFeature,
            GameForm = gameForm
        };

        Assert.Equal(gameFeatureId, link.GameFeatureId);
        Assert.Equal(gameFormId, link.GameFormId);
        Assert.Same(gameFeature, link.GameFeature);
        Assert.Same(gameForm, link.GameForm);
    }
}
