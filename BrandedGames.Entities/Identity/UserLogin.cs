using Microsoft.AspNetCore.Identity;
using System;

namespace BrandedGames.Entities.Identity;

/// <summary>External login (for example a social provider) associated with a <see cref="User"/>.</summary>
public class UserLogin : IdentityUserLogin<Guid>
{
    /// <summary>The user this login belongs to.</summary>
    public virtual User User { get; set; }
}
