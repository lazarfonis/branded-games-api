using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities.Interfaces;

/// <summary>
/// Marks an entity that carries audit timestamps. The timestamps are populated
/// automatically by the data context when changes are saved.
/// </summary>
public interface IEntity
{
    /// <summary>Timestamp when the entity was created.</summary>
    DateTime CreatedAt { get; set; }

    /// <summary>Timestamp when the entity was last modified.</summary>
    DateTime? ModifiedAt { get; set; }
}
