using Microsoft.AspNetCore.Identity;
using System;

namespace BrandedGames.Entities.Identity;

/// <summary>Authentication token associated with a <see cref="User"/>.</summary>
public class UserToken : IdentityUserToken<Guid>
{
    /// <summary>The user this token belongs to.</summary>
    public virtual User User { get; set; }
}
