using System;
using System.ComponentModel.DataAnnotations;

namespace playFusionX.Data.Models;

public class SpotifyToken
{
    [Key]
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime TokenExpiry { get; set; }
}
