using BrandedGames.Entities;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class PlatformTypeTests
{
    [Fact]
    public void GameFormPlatformTypes_Collection_IsInitialized_AndEmpty_ByDefault()
    {
        var platform = new PlatformType();

        Assert.NotNull(platform.GameFormPlatformTypes);
        Assert.Empty(platform.GameFormPlatformTypes);
    }

    [Fact]
    public void Properties_RoundTrip()
    {
        var id = Guid.NewGuid();

        var platform = new PlatformType
        {
            Id = id,
            Name = "Web",
            IconName = "globe",
            Description = "Runs in the browser"
        };

        Assert.Equal(id, platform.Id);
        Assert.Equal("Web", platform.Name);
        Assert.Equal("globe", platform.IconName);
        Assert.Equal("Runs in the browser", platform.Description);
    }

    [Fact]
    public void GameFormPlatformTypes_Collection_AcceptsItems()
    {
        var platform = new PlatformType();

        platform.GameFormPlatformTypes.Add(new GameFormPlatformType());

        Assert.Single(platform.GameFormPlatformTypes);
    }
}
