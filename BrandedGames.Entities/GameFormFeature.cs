using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

/// <summary>
/// Join entity representing the many-to-many relationship between a
/// <see cref="GameForm"/> and a <see cref="GameFeature"/>.
/// </summary>
public class GameFormFeature
{
    /// <summary>Identifier of the related <see cref="GameFeature"/>.</summary>
    public Guid GameFeatureId { get; set; }

    /// <summary>Identifier of the related <see cref="GameForm"/>.</summary>
    public Guid GameFormId { get; set; }

    /// <summary>The related feature.</summary>
    [ForeignKey(nameof(GameFeatureId))]
    [InverseProperty(nameof(Entities.GameFeature.Features))]
    public virtual GameFeature GameFeature { get; set; }

    /// <summary>The related game form.</summary>
    [ForeignKey(nameof(GameFormId))]
    [InverseProperty(nameof(Entities.GameForm.Features))]
    public virtual GameForm GameForm { get; set; }
}
