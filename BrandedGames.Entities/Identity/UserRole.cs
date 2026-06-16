using Microsoft.AspNetCore.Identity;
using System;

namespace BrandedGames.Entities.Identity;

/// <summary>Join entity assigning a <see cref="Role"/> to a <see cref="User"/>.</summary>
public class UserRole : IdentityUserRole<Guid>
{
    /// <summary>The assigned user.</summary>
    public virtual User User { get; set; }

    /// <summary>The assigned role.</summary>
    public virtual Role Role { get; set; }
}
