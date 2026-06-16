using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using BrandedGames.Common.Enums;

namespace BrandedGames.Entities;

/// <summary>
/// A file (typically an image) uploaded for a <see cref="GameForm"/> and stored in the
/// external file storage provider.
/// </summary>
public class GameFormFile
{
    /// <summary>Unique identifier of the file.</summary>
    public Guid Id { get; set; }

    /// <summary>Identifier of the owning <see cref="GameForm"/>, if any.</summary>
    public Guid? GameFormId { get; set; }

    /// <summary>Stored (sanitized) file name.</summary>
    [MaxLength(50)]
    public string FileName { get; set; }

    /// <summary>Original file name as provided on upload.</summary>
    [MaxLength(1000)]
    public string FileOriginalName { get; set; }

    /// <summary>Public URL of the stored file.</summary>
    [MaxLength(1000)]
    public string FileUrl { get; set; }

    /// <summary>Public URL of the generated thumbnail.</summary>
    [MaxLength(1000)]
    public string ThumbUrl { get; set; }

    /// <summary>The kind of file (for example image).</summary>
    public FileType FileType { get; set; }

    /// <summary>Timestamp when the file record was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Timestamp when the file record was last modified.</summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>The owning game form.</summary>
    [ForeignKey(nameof(GameFormId))]
    [InverseProperty(nameof(Entities.GameForm.Files))]
    public virtual GameForm GameForm { get; set; }
}
