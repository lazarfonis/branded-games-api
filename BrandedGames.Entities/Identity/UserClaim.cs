using Microsoft.AspNetCore.Identity;
using System;

namespace BrandedGames.Entities.Identity;

/// <summary>Identity claim associated with a <see cref="User"/>.</summary>
public class UserClaim : IdentityUserClaim<Guid>
{
    /// <summary>The user this claim belongs to.</summary>
    public virtual User User { get; set; }
}
