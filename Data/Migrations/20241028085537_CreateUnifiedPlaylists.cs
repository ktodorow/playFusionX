using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace playFusionX.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateUnifiedPlaylists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnifiedPlaylists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnifiedPlaylists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnifiedTracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnifiedPlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnifiedTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnifiedTracks_UnifiedPlaylists_UnifiedPlaylistId",
                        column: x => x.UnifiedPlaylistId,
                        principalTable: "UnifiedPlaylists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnifiedTracks_UnifiedPlaylistId",
                table: "UnifiedTracks",
                column: "UnifiedPlaylistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnifiedTracks");

            migrationBuilder.DropTable(
                name: "UnifiedPlaylists");
        }
    }
}
