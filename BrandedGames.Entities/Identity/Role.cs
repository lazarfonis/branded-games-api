using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BrandedGames.Entities.Identity;

/// <summary>
/// Application role. Extends ASP.NET Core Identity's role with navigation properties.
/// </summary>
public class Role : IdentityRole<Guid>
{
    /// <summary>The user-role assignments for this role.</summary>
    public virtual ICollection<UserRole> UserRoles { get; set; }

    /// <summary>The claims granted to this role.</summary>
    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
}
