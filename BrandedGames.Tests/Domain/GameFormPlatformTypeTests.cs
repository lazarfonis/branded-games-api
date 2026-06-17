using BrandedGames.Entities;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class GameFormPlatformTypeTests
{
    [Fact]
    public void Properties_RoundTrip()
    {
        var gameFormId = Guid.NewGuid();
        var platformTypeId = Guid.NewGuid();
        var gameForm = new GameForm { Id = gameFormId };
        var platformType = new PlatformType { Id = platformTypeId };

        var link = new GameFormPlatformType
        {
            GameFormId = gameFormId,
            PlatformTypeId = platformTypeId,
            GameForm = gameForm,
            PlatformType = platformType
        };

        Assert.Equal(gameFormId, link.GameFormId);
        Assert.Equal(platformTypeId, link.PlatformTypeId);
        Assert.Same(gameForm, link.GameForm);
        Assert.Same(platformType, link.PlatformType);
    }
}
