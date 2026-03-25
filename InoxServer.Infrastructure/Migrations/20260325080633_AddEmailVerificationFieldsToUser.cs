using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InoxServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerificationFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationTokenExpiresAt",
                table: "users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "users");

            migrationBuilder.DropColumn(
                name: "EmailVerificationTokenExpiresAt",
                table: "users");
        }
    }
}
