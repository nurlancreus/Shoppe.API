using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Order",
                table: "Slides",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { "admin-role-id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Admin", "ADMIN", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DeactivatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenEndDate", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { "admin-user-id", 0, "aff87da7-f8cf-4bb2-9cbd-b8aa5e8ee59d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "nurlancreus@example.com", false, null, false, null, false, null, "NURLANCREUS@EXAMPLE.COM", "NURLANCREUS", "AQAAAAIAAYagAAAAEFl1R5wPH0ZmB0ba2GUHV1lrpxPQmRU3ylGlGCPtl6UqBuoD4XNL8tB5FoTg0nkLDQ==", null, false, null, null, "8771217e-bc4b-40be-8642-3dfc82f37537", false, null, "nurlancreus" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "admin-role-id", "admin-user-id" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Slide_Order",
                table: "Slides",
                sql: "[Order] >= 0 AND [Order] <= 255");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Slide_Order",
                table: "Slides");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "admin-role-id", "admin-user-id" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Slides");
        }
    }
}
