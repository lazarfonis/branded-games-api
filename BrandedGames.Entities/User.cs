using BrandedGames.Entities.Identity;
using BrandedGames.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedGames.Entities;

/// <summary>
/// Application user. Extends ASP.NET Core Identity with profile, avatar and
/// email-verification data, and implements <see cref="IEntity"/> for audit timestamps.
/// </summary>
public class User : IdentityUser<Guid>, IEntity
{
    /// <summary>User's first name.</summary>
    [MaxLength(100)]
    public string FirstName { get; set; }

    /// <summary>User's last name.</summary>
    [MaxLength(100)]
    public string LastName { get; set; }

    /// <summary>Stored (sanitized) avatar file name.</summary>
    [MaxLength(50)]
    public string FileName { get; set; }

    /// <summary>Original avatar file name as provided on upload.</summary>
    [MaxLength(1000)]
    public string FileOriginalName { get; set; }

    /// <summary>Public URL of the avatar.</summary>
    [MaxLength(1000)]
    public string FileUrl { get; set; }

    /// <summary>Public URL of the avatar thumbnail.</summary>
    [MaxLength(1000)]
    public string ThumbUrl { get; set; }

    /// <summary>Code sent to the user to verify their email address.</summary>
    [MaxLength(6)]
    public string EmailVerificationCode { get; set; }

    /// <summary>Timestamp when the verification code was last sent.</summary>
    public DateTime? DateVerificationCodeSent { get; set; }

    /// <summary>Whether the user account is active.</summary>
    public bool Active { get; set; }

    /// <summary>Timestamp when the user was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Timestamp when the user was last modified.</summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>The user's identity claims.</summary>
    public virtual ICollection<UserClaim> Claims { get; set; } = new HashSet<UserClaim>();

    /// <summary>The user's external logins.</summary>
    public virtual ICollection<UserLogin> Logins { get; set; } = new HashSet<UserLogin>();

    /// <summary>The user's authentication tokens.</summary>
    public virtual ICollection<UserToken> Tokens { get; set; } = new HashSet<UserToken>();

    /// <summary>The roles assigned to the user.</summary>
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}
