using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrangements_Users_CreatedById",
                table: "Arrangements");

            migrationBuilder.DropForeignKey(
                name: "FK_Arrangements_Users_Id",
                table: "Arrangements");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_Id",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Arrangements",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Arrangements_CreatedById",
                table: "Arrangements",
                newName: "IX_Arrangements_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrangements_Users_UserId",
                table: "Arrangements",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrangements_Users_UserId",
                table: "Arrangements");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Arrangements",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Arrangements_UserId",
                table: "Arrangements",
                newName: "IX_Arrangements_CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrangements_Users_CreatedById",
                table: "Arrangements",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrangements_Users_Id",
                table: "Arrangements",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_Id",
                table: "Reservations",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
