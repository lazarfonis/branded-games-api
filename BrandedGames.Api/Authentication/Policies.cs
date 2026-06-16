namespace BrandedGames.Api.Authentication;

/// <summary>
/// Names of the authorization policies used across the API.
/// </summary>
public static class Policies
{
    /// <summary>Policy requiring the user's email to be not yet confirmed.</summary>
    public const string NotConfirmedEmail = "NotConfirmedEmail";

    /// <summary>Policy requiring the user's email to be confirmed.</summary>
    public const string EmailConfirmed = "EmailConfirmed";

    /// <summary>Policy requiring an authenticated, registered user.</summary>
    public const string RegisteredUser = "RegisteredUser";

    /// <summary>Policy requiring an existing user.</summary>
    public const string ExistingUser = "ExistingUser";

    /// <summary>Policy requiring the user to have the administrator role.</summary>
    public const string AdministratorUser = "AdministratorUser";
}
