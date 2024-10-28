using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using playFusionX.Data.Models;

namespace playFusionX.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SpotifyToken> SpotifyTokens { get; set; }
        public virtual DbSet<UnifiedPlaylist> UnifiedPlaylists { get; set; }
        public virtual DbSet<UnifiedTrack>  UnifiedTracks { get; set; }

    }
}
