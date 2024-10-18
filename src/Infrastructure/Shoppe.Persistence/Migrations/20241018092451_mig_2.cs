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
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProduct_Discounts_DiscountId",
                table: "DiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProduct_Products_ProductId",
                table: "DiscountProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountProduct",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountProduct_ProductId",
                table: "DiscountProduct");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Discounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "DiscountProduct",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "DiscountId",
                table: "DiscountProduct",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "DiscountProduct",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DiscountProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountProduct",
                table: "DiscountProduct",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DiscountCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DiscountCategory_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_DiscountId",
                table: "DiscountProduct",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_ProductId_DiscountId",
                table: "DiscountProduct",
                columns: new[] { "ProductId", "DiscountId" },
                unique: true,
                filter: "[ProductId] IS NOT NULL AND [DiscountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_ProductId_IsActive",
                table: "DiscountProduct",
                columns: new[] { "ProductId", "IsActive" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCategory_CategoryId_DiscountId",
                table: "DiscountCategory",
                columns: new[] { "CategoryId", "DiscountId" },
                unique: true,
                filter: "[CategoryId] IS NOT NULL AND [DiscountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCategory_CategoryId_IsActive",
                table: "DiscountCategory",
                columns: new[] { "CategoryId", "IsActive" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCategory_DiscountId",
                table: "DiscountCategory",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountProduct_Discounts_DiscountId",
                table: "DiscountProduct",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountProduct_Products_ProductId",
                table: "DiscountProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProduct_Discounts_DiscountId",
                table: "DiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProduct_Products_ProductId",
                table: "DiscountProduct");

            migrationBuilder.DropTable(
                name: "DiscountCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountProduct",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountProduct_DiscountId",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountProduct_ProductId_DiscountId",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountProduct_ProductId_IsActive",
                table: "DiscountProduct");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DiscountProduct");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DiscountProduct");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "DiscountProduct",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DiscountId",
                table: "DiscountProduct",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountProduct",
                table: "DiscountProduct",
                columns: new[] { "DiscountId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_ProductId",
                table: "DiscountProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountProduct_Discounts_DiscountId",
                table: "DiscountProduct",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountProduct_Products_ProductId",
                table: "DiscountProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
