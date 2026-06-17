using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BrandedGames.Common.Enums;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace BrandedGames.Core;

/// <summary>
/// Handles uploading files to the Cloudinary file storage provider.
/// </summary>
public class CloudinaryFileManager : ICloudinaryFileManager
{
    private readonly Cloudinary cloudinary;

    /// <summary>Creates a new <see cref="CloudinaryFileManager"/>.</summary>
    /// <param name="configuration">Application configuration providing the Cloudinary API key.</param>
    public CloudinaryFileManager(IConfiguration configuration)
    {
        cloudinary = new Cloudinary(configuration["cloudinaryApiKey"]);
        cloudinary.Api.Secure = true;
    }

    /// <summary>Uploads a file to Cloudinary.</summary>
    /// <param name="file">The file to upload.</param>
    /// <returns>The Cloudinary upload result, including the stored file URL.</returns>
    public async Task<ImageUploadResult> ProcessFileStorageUpload(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true
        };

        return await cloudinary.UploadAsync(uploadParams);
    }
}
