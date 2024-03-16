using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Music.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addmodelSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Musices_MusicId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicEntityTypeMusic_Musices_MusicsId",
                table: "MusicEntityTypeMusic");

            migrationBuilder.DropForeignKey(
                name: "FK_Musices_Singeres_SingerId",
                table: "Musices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Singeres",
                table: "Singeres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Musices",
                table: "Musices");

            migrationBuilder.RenameTable(
                name: "Singeres",
                newName: "Singers");

            migrationBuilder.RenameTable(
                name: "Musices",
                newName: "Musics");

            migrationBuilder.RenameIndex(
                name: "IX_Musices_SingerId",
                table: "Musics",
                newName: "IX_Musics_SingerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Singers",
                table: "Singers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Musics",
                table: "Musics",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expired = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Musics_MusicId",
                table: "Category",
                column: "MusicId",
                principalTable: "Musics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicEntityTypeMusic_Musics_MusicsId",
                table: "MusicEntityTypeMusic",
                column: "MusicsId",
                principalTable: "Musics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Musics_Singers_SingerId",
                table: "Musics",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Musics_MusicId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicEntityTypeMusic_Musics_MusicsId",
                table: "MusicEntityTypeMusic");

            migrationBuilder.DropForeignKey(
                name: "FK_Musics_Singers_SingerId",
                table: "Musics");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Singers",
                table: "Singers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Musics",
                table: "Musics");

            migrationBuilder.RenameTable(
                name: "Singers",
                newName: "Singeres");

            migrationBuilder.RenameTable(
                name: "Musics",
                newName: "Musices");

            migrationBuilder.RenameIndex(
                name: "IX_Musics_SingerId",
                table: "Musices",
                newName: "IX_Musices_SingerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Singeres",
                table: "Singeres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Musices",
                table: "Musices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Musices_MusicId",
                table: "Category",
                column: "MusicId",
                principalTable: "Musices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicEntityTypeMusic_Musices_MusicsId",
                table: "MusicEntityTypeMusic",
                column: "MusicsId",
                principalTable: "Musices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Musices_Singeres_SingerId",
                table: "Musices",
                column: "SingerId",
                principalTable: "Singeres",
                principalColumn: "Id");
        }
    }
}
