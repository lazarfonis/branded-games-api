using BrandedGames.Entities;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class PlatformTypeTests
{
    [Fact]
    public void GameForms_Collection_IsInitialized_AndEmpty_ByDefault()
    {
        var platform = new PlatformType();

        Assert.NotNull(platform.GameForms);
        Assert.Empty(platform.GameForms);
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
    public void GameForms_Collection_AcceptsItems()
    {
        var platform = new PlatformType();

        platform.GameForms.Add(new GameFormPlatformType());

        Assert.Single(platform.GameForms);
    }
}
