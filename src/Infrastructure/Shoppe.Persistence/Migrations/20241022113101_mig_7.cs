using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "About",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "About",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "About",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "About",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("3e725483-8b3c-4d73-b6a0-7962e4b9071e"), new DateTime(2024, 10, 22, 11, 31, 0, 551, DateTimeKind.Utc).AddTicks(8862), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.CreateIndex(
                name: "IX_About_Id",
                table: "About",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_About_Id",
                table: "About");

            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("3e725483-8b3c-4d73-b6a0-7962e4b9071e"));

            migrationBuilder.DropColumn(
                name: "Email",
                table: "About");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "About");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "About",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "About",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
