using BrandedGames.Common.Enums;
using BrandedGames.Entities;
using Xunit;

namespace BrandedGames.Tests.Domain;

public class GameFormFileTests
{
    [Fact]
    public void Properties_RoundTrip()
    {
        var id = Guid.NewGuid();
        var gameFormId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var modifiedAt = createdAt.AddMinutes(5);
        var gameForm = new GameForm { Id = gameFormId };

        var file = new GameFormFile
        {
            Id = id,
            GameFormId = gameFormId,
            FileName = "logo.png",
            FileOriginalName = "original-logo.png",
            FileUrl = "https://cdn.example.com/logo.png",
            ThumbUrl = "https://cdn.example.com/logo-thumb.png",
            FileType = FileType.Image,
            CreatedAt = createdAt,
            ModifiedAt = modifiedAt,
            GameForm = gameForm
        };

        Assert.Equal(id, file.Id);
        Assert.Equal(gameFormId, file.GameFormId);
        Assert.Equal("logo.png", file.FileName);
        Assert.Equal("original-logo.png", file.FileOriginalName);
        Assert.Equal("https://cdn.example.com/logo.png", file.FileUrl);
        Assert.Equal("https://cdn.example.com/logo-thumb.png", file.ThumbUrl);
        Assert.Equal(FileType.Image, file.FileType);
        Assert.Equal(createdAt, file.CreatedAt);
        Assert.Equal(modifiedAt, file.ModifiedAt);
        Assert.Same(gameForm, file.GameForm);
    }

    [Fact]
    public void GameFormId_AndModifiedAt_AreNullable()
    {
        var file = new GameFormFile { GameFormId = null, ModifiedAt = null };

        Assert.Null(file.GameFormId);
        Assert.Null(file.ModifiedAt);
    }
}
