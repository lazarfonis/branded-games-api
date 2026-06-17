using BrandedGames.Entities;
using BrandedGames.Entities.Identity;
using BrandedGames.Entities.Interfaces;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class UserTests
{
    [Fact]
    public void Collections_AreInitialized_AndEmpty_ByDefault()
    {
        var user = new User();

        Assert.NotNull(user.Claims);
        Assert.Empty(user.Claims);
        Assert.NotNull(user.Logins);
        Assert.Empty(user.Logins);
        Assert.NotNull(user.Tokens);
        Assert.Empty(user.Tokens);
        Assert.NotNull(user.UserRoles);
        Assert.Empty(user.UserRoles);
    }

    [Fact]
    public void ImplementsIEntity()
    {
        Assert.IsAssignableFrom<IEntity>(new User());
    }

    [Fact]
    public void Properties_RoundTrip()
    {
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var sentAt = createdAt.AddMinutes(-1);

        var user = new User
        {
            Id = id,
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com",
            FileName = "avatar.png",
            FileOriginalName = "original-avatar.png",
            FileUrl = "https://cdn.example.com/avatar.png",
            ThumbUrl = "https://cdn.example.com/avatar-thumb.png",
            EmailVerificationCode = "123456",
            DateVerificationCodeSent = sentAt,
            Active = true,
            CreatedAt = createdAt
        };

        Assert.Equal(id, user.Id);
        Assert.Equal("Ada", user.FirstName);
        Assert.Equal("Lovelace", user.LastName);
        Assert.Equal("ada@example.com", user.Email);
        Assert.Equal("avatar.png", user.FileName);
        Assert.Equal("original-avatar.png", user.FileOriginalName);
        Assert.Equal("https://cdn.example.com/avatar.png", user.FileUrl);
        Assert.Equal("https://cdn.example.com/avatar-thumb.png", user.ThumbUrl);
        Assert.Equal("123456", user.EmailVerificationCode);
        Assert.Equal(sentAt, user.DateVerificationCodeSent);
        Assert.True(user.Active);
        Assert.Equal(createdAt, user.CreatedAt);
    }

    [Fact]
    public void UserRoles_Collection_AcceptsItems()
    {
        var user = new User();

        user.UserRoles.Add(new UserRole());

        Assert.Single(user.UserRoles);
    }
}
