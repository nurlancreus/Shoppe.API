using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Tags",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Tags",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("990b90ee-d0be-4614-a51c-8acd701a07b5"), new DateTime(2024, 10, 29, 13, 48, 6, 774, DateTimeKind.Utc).AddTicks(2264), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });
        }
    }
}
