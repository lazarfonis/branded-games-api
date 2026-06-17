using AutoMapper;

namespace BrandedGames.Tests.Infrastructure;

/// <summary>
/// Builds an <see cref="IMapper"/> from the production <see cref="MapperConfig"/> profile so
/// manager tests exercise the same entity/view-model mappings the API uses.
/// </summary>
public static class TestMapper
{
    /// <summary>Creates an <see cref="IMapper"/> configured with <see cref="MapperConfig"/>.</summary>
    public static IMapper Create()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MapperConfig>());
        return configuration.CreateMapper();
    }
}
