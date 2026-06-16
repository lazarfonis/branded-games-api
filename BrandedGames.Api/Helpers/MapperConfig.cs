using AutoMapper;
using BrandedGames.Common.Models;
using BrandedGames.Entities;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        Guid? userId = null;

        // Read mappings (entity -> view model)
        CreateMap<PlatformType, PlatformTypeModel>();
        CreateMap<GameType, GameTypeModel>();
        CreateMap<GameFeature, FeatureModel>();
        CreateMap<GameFormFile, GameFormFileModel>();
        CreateMap<GameForm, GameFormModel>()
            .ForMember(d => d.GameTypeName, o => o.MapFrom(s => s.GameType.Name))
            .ForMember(d => d.Features, o => o.MapFrom(s => s.Features.Select(f => f.GameFeature)))
            .ForMember(d => d.PlatformTypes, o => o.MapFrom(s => s.GameFormPlatformTypes.Select(p => p.PlatformType)))
            .ForMember(d => d.Files, o => o.MapFrom(s => s.Files));

        // Write mappings (create/update model -> entity)
        CreateMap<FeatureCreateModel, GameFeature>();
        CreateMap<FeatureUpdateModel, GameFeature>();
        CreateMap<GameTypeCreateModel, GameType>();
        CreateMap<GameTypeUpdateModel, GameType>();
        CreateMap<PlatformTypeCreateModel, PlatformType>();
        CreateMap<PlatformTypeUpdateModel, PlatformType>();
    }
}
