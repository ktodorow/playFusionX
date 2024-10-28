using System;
using System.ComponentModel.DataAnnotations;

namespace playFusionX.Data.Models;

public class UnifiedTrack
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string TrackId { get; set; } = null!;

    [Required]
    public string Platform { get; set; } = null!;
}
