using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Arrangements",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Arrangements_CreatedById",
                table: "Arrangements",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrangements_Users_CreatedById",
                table: "Arrangements",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_Id",
                table: "Reservations",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrangements_Users_CreatedById",
                table: "Arrangements");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_Id",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Arrangements_CreatedById",
                table: "Arrangements");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Arrangements");
        }
    }
}
