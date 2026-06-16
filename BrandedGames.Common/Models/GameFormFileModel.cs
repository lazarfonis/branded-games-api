using BrandedGames.Common.Enums;

namespace BrandedGames.Common.Models;

public class GameFormFileModel
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileOriginalName { get; set; }
    public string FileUrl { get; set; }
    public string ThumbUrl { get; set; }
    public FileType FileType { get; set; }
}
