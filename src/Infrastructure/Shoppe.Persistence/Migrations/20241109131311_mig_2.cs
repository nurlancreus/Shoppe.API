using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("13561336-7d54-4964-9027-d20c92fc0636"));

            migrationBuilder.AddColumn<string>(
                name: "PreviewUrl",
                table: "ApplicationFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("523ff621-0434-453c-9c92-d125c48dabf2"), null, new DateTime(2024, 11, 9, 13, 13, 10, 714, DateTimeKind.Utc).AddTicks(2698), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e2166774-d182-4ef2-9026-0fb4ce27f87b", "AQAAAAIAAYagAAAAEP7x0aRnpNI5KZmbampcjXwHV1Hvgm8MLT8cs4KKknwxLK2I9YguLe9iiREsh/BWNg==", "41a82e4b-1cd2-42d2-b771-a44679e1a4c2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("523ff621-0434-453c-9c92-d125c48dabf2"));

            migrationBuilder.DropColumn(
                name: "PreviewUrl",
                table: "ApplicationFiles");

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("13561336-7d54-4964-9027-d20c92fc0636"), null, new DateTime(2024, 11, 9, 12, 31, 0, 239, DateTimeKind.Utc).AddTicks(2830), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e45db760-b383-4348-a28d-774ad1feccbc", "AQAAAAIAAYagAAAAELo5dzsWvz33GA+7oQmhXhJ6Kl8+dTNdD85pqbaeOA+swbr6LRB1B/f7mLUWHNsNqQ==", "dc934830-8f15-4653-8e41-87b1d15ba319" });
        }
    }
}
