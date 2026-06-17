using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BrandedGames.Core;

/// <summary>
/// Abstraction over the file storage provider used to upload game files. Extracted so that
/// consumers (for example <see cref="GameFormManager"/>) can be unit tested without performing
/// real network uploads.
/// </summary>
public interface ICloudinaryFileManager
{
    /// <summary>Uploads a file to the storage provider.</summary>
    /// <param name="file">The file to upload.</param>
    /// <returns>The upload result, including the stored file URL.</returns>
    Task<ImageUploadResult> ProcessFileStorageUpload(IFormFile file);
}
