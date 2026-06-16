using BrandedGames.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

/// <summary>
/// A branded game configuration submitted by a customer. Ties together the chosen
/// game type, selected features, target platforms, uploaded files and reward settings.
/// </summary>
public class GameForm
{
    /// <summary>Unique identifier of the game form.</summary>
    public Guid Id { get; set; }

    /// <summary>Identifier of the <see cref="GameType"/> this game is based on.</summary>
    public Guid GameTypeId { get; set; }

    /// <summary>The type of customer the game is created for.</summary>
    public CustomerType CustomerType { get; set; }

    /// <summary>Optional price of the game, in the smallest currency unit.</summary>
    public int? Price { get; set; }

    /// <summary>Free-form description of the items used within the game.</summary>
    public string Items { get; set; }

    /// <summary>Whether the top-ranked players are rewarded.</summary>
    public bool RewardTopPlayers { get; set; }

    /// <summary>Description of the rewards offered.</summary>
    public string Rewards { get; set; }

    /// <summary>Description of how rewards are distributed across placements.</summary>
    public string RewardPlacements { get; set; }

    /// <summary>The game type this game form is based on.</summary>
    [ForeignKey(nameof(GameTypeId))]
    [InverseProperty(nameof(Entities.GameType.GameForms))]
    public virtual GameType GameType { get; set; }

    /// <summary>The features selected for this game (join to <see cref="GameFeature"/>).</summary>
    [InverseProperty(nameof(GameFormFeature.GameForm))]
    public virtual ICollection<GameFormFeature> Features { get; set; } = new HashSet<GameFormFeature>();

    /// <summary>The files (images) uploaded for this game.</summary>
    [InverseProperty(nameof(GameFormFile.GameForm))]
    public virtual ICollection<GameFormFile> Files { get; set; } = new HashSet<GameFormFile>();

    /// <summary>The platforms this game targets (join to <see cref="PlatformType"/>).</summary>
    [InverseProperty(nameof(GameFormPlatformType.GameForm))]
    public virtual ICollection<GameFormPlatformType> GameFormPlatformTypes { get; set; } = new HashSet<GameFormPlatformType>();
}
