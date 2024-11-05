using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7443bb1d-1f91-45c2-ab62-0630bf7a0692", true, "AQAAAAIAAYagAAAAEEhZioof/8JmbCSHszgEjvCVlp+W1IuyzXGXCi+j9ZKI3PwmQ/JQAXFN3/lrJV3z7Q==", "01c6ad1a-e71b-4ef4-ab0f-332abdf41eef" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bcff2512-2ccf-4b2a-a936-5c3c33f6002b", false, "AQAAAAIAAYagAAAAEMoau2773iETH+BxHSEvs3j2uDGm82YpsJpJiMCeH6kyS6rR1mx5oEem0C0T1snNfQ==", "76834b2d-2242-4e70-a0e9-e732e4882eab" });
        }
    }
}
