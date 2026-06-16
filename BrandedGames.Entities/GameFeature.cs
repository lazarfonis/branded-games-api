using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

/// <summary>
/// A feature that can be added to a branded game (for example a leaderboard or a timer),
/// with its own price and presentation metadata.
/// </summary>
public class GameFeature
{
    /// <summary>Unique identifier of the feature.</summary>
    public Guid Id { get; set; }

    /// <summary>Display name of the feature.</summary>
    public string Name { get; set; }

    /// <summary>Name of the icon used to represent the feature.</summary>
    public string IconName { get; set; }

    /// <summary>Price of the feature, in the smallest currency unit.</summary>
    public int Price { get; set; }

    /// <summary>Description of what the feature does.</summary>
    public string Description { get; set; }

    /// <summary>The game forms that use this feature (join to <see cref="GameForm"/>).</summary>
    [InverseProperty(nameof(GameFormFeature.GameFeature))]
    public virtual ICollection<GameFormFeature> Features { get; set; } = new HashSet<GameFormFeature>();
}
