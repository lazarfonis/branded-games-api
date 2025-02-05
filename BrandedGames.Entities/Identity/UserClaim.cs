using Microsoft.AspNetCore.Identity;
using System;

namespace BrandedGames.Entities.Identity;

public class UserClaim : IdentityUserClaim<Guid>
{
    public virtual User User { get; set; }
}
