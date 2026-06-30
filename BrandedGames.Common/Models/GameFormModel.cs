using BrandedGames.Common.Enums;

namespace BrandedGames.Common.Models;

public class GameFormModel
{
    public Guid Id { get; set; }
    public Guid GameTypeId { get; set; }
    public string GameTypeName { get; set; }
    public Guid? UserId { get; set; }
    public CustomerType CustomerType { get; set; }
    public int? Price { get; set; }
    public string Items { get; set; }
    public bool RewardTopPlayers { get; set; }
    public string Rewards { get; set; }
    public string RewardPlacements { get; set; }

    public List<FeatureModel> Features { get; set; } = new();
    public List<PlatformTypeModel> PlatformTypes { get; set; } = new();
    public List<GameFormFileModel> Files { get; set; } = new();
}
