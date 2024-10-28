using System;
using System.ComponentModel.DataAnnotations;

using static playFusionX.Common.EntityValidationConstants;

namespace playFusionX.Data.Models;

public class UnifiedPlaylist
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(PlaylistNameMaxValue)]
    public string Name { get; set; } = null!;

    [Required]
    public string OwnerId { get; set; } = null!;

    public virtual ICollection<UnifiedTrack> Tracks { get; set; } = new HashSet<UnifiedTrack>();
}
