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
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFiles_AspNetUsers_UserId",
                table: "ApplicationFiles");

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
                values: new object[] { "9008c711-e96d-4f31-a115-8b23eee5d558", true, "AQAAAAIAAYagAAAAEJvWJJLdUiXPxN+/7spqDzGtoTGZJbWJBmYjNkEyl5q/LOUTZrQswPUYLUdQtbboNg==", "42c68a60-9b53-4657-a808-2a3e611b5cab" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFiles_AspNetUsers_UserId",
                table: "ApplicationFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFiles_AspNetUsers_UserId",
                table: "ApplicationFiles");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFiles_AspNetUsers_UserId",
                table: "ApplicationFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
