using Microsoft.AspNetCore.Identity;
using System;

namespace BrandedGames.Entities.Identity;

/// <summary>Identity claim associated with a <see cref="Role"/>.</summary>
public class RoleClaim : IdentityRoleClaim<Guid>
{
    /// <summary>The role this claim belongs to.</summary>
    public virtual Role Role { get; set; }
}
