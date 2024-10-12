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
            migrationBuilder.RenameColumn(
                name: "Material",
                table: "ProductDetails",
                newName: "Materials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Materials",
                table: "ProductDetails",
                newName: "Material");
        }
    }
}
