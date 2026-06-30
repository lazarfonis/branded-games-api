using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandedGames.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGameFormUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "GameForms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameForms_UserId",
                table: "GameForms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameForms_AspNetUsers_UserId",
                table: "GameForms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameForms_AspNetUsers_UserId",
                table: "GameForms");

            migrationBuilder.DropIndex(
                name: "IX_GameForms_UserId",
                table: "GameForms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GameForms");
        }
    }
}
