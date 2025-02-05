using Microsoft.AspNetCore.Identity;
using System;

namespace BrandedGames.Entities.Identity;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public virtual Role Role { get; set; }
}
