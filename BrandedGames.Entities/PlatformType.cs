using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

/// <summary>
/// A platform a branded game can target (for example web, mobile or kiosk).
/// </summary>
public class PlatformType
{
    /// <summary>Unique identifier of the platform type.</summary>
    public Guid Id { get; set; }

    /// <summary>Display name of the platform.</summary>
    public string Name { get; set; }

    /// <summary>Name of the icon used to represent the platform.</summary>
    public string IconName { get; set; }

    /// <summary>Description of the platform.</summary>
    public string Description { get; set; }

    /// <summary>The game forms targeting this platform (join to <see cref="GameForm"/>).</summary>
    [InverseProperty(nameof(GameFormPlatformType.PlatformType))]
    public virtual ICollection<GameFormPlatformType> GameForms { get; set; } = new HashSet<GameFormPlatformType>();
}
