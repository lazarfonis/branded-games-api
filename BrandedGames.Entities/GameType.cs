using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

/// <summary>
/// A category of game (for example quiz or memory) that a branded game can be based on.
/// </summary>
public class GameType
{
    /// <summary>Unique identifier of the game type.</summary>
    public Guid Id { get; set; }

    /// <summary>Display name of the game type.</summary>
    public string Name { get; set; }

    /// <summary>Name of the icon used to represent the game type.</summary>
    public string IconName { get; set; }

    /// <summary>The game forms created from this game type.</summary>
    [InverseProperty(nameof(GameForm.GameType))]
    public virtual ICollection<GameForm> GameForms { get; set; } = new HashSet<GameForm>();
}
