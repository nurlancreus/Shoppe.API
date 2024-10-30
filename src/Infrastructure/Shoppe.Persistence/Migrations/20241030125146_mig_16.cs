using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d6a07db5-2f72-45b0-8245-3ea9140b3ff0", "Nurlan", "Shukurov", "AQAAAAIAAYagAAAAEPo1Aj9ZWP7Tx38C/rcFa6IcrBuaupeYb5TTPcGV0zFdLlGcsXs5Ww1BSdWsnzcLQQ==", "60b249e7-ad0d-4168-92b6-f965f599b8b3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aff87da7-f8cf-4bb2-9cbd-b8aa5e8ee59d", null, null, "AQAAAAIAAYagAAAAEFl1R5wPH0ZmB0ba2GUHV1lrpxPQmRU3ylGlGCPtl6UqBuoD4XNL8tB5FoTg0nkLDQ==", "8771217e-bc4b-40be-8642-3dfc82f37537" });
        }
    }
}
