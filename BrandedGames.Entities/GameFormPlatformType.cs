using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

/// <summary>
/// Join entity representing the many-to-many relationship between a
/// <see cref="GameForm"/> and a <see cref="PlatformType"/>.
/// </summary>
public class GameFormPlatformType
{
    /// <summary>Identifier of the related <see cref="GameForm"/>.</summary>
    public Guid GameFormId { get; set; }

    /// <summary>Identifier of the related <see cref="PlatformType"/>.</summary>
    public Guid PlatformTypeId { get; set; }

    /// <summary>The related game form.</summary>
    [ForeignKey(nameof(GameFormId))]
    [InverseProperty(nameof(Entities.GameForm.Platforms))]
    public virtual GameForm GameForm { get; set; }

    /// <summary>The related platform type.</summary>
    [ForeignKey(nameof(PlatformTypeId))]
    [InverseProperty(nameof(Entities.PlatformType.GameForms))]
    public virtual PlatformType PlatformType { get; set; }
}
