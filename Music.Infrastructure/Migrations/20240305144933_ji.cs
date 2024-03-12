using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Music.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Singeres",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Singeres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeMusic",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeMusic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Musices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Subtitle = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    UrlDownload = table.Column<string>(type: "TEXT", nullable: false),
                    SingerId = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musices_Singeres_SingerId",
                        column: x => x.SingerId,
                        principalTable: "Singeres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    MusicId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Musices_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Musices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicTypeMusic",
                columns: table => new
                {
                    MusicsId = table.Column<string>(type: "TEXT", nullable: false),
                    TypeMusicsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicTypeMusic", x => new { x.MusicsId, x.TypeMusicsId });
                    table.ForeignKey(
                        name: "FK_MusicTypeMusic_Musices_MusicsId",
                        column: x => x.MusicsId,
                        principalTable: "Musices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicTypeMusic_TypeMusic_TypeMusicsId",
                        column: x => x.TypeMusicsId,
                        principalTable: "TypeMusic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_MusicId",
                table: "Category",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_Musices_SingerId",
                table: "Musices",
                column: "SingerId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicTypeMusic_TypeMusicsId",
                table: "MusicTypeMusic",
                column: "TypeMusicsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "MusicTypeMusic");

            migrationBuilder.DropTable(
                name: "Musices");

            migrationBuilder.DropTable(
                name: "TypeMusic");

            migrationBuilder.DropTable(
                name: "Singeres");
        }
    }
}
