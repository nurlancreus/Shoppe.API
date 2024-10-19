using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCategory_Categories_CategoryId",
                table: "DiscountCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCategory_Discounts_DiscountId",
                table: "DiscountCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProduct_Discounts_DiscountId",
                table: "DiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProduct_Products_ProductId",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountProduct_ProductId_DiscountId",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountProduct_ProductId_IsActive",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCategory_CategoryId_DiscountId",
                table: "DiscountCategory");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCategory_CategoryId_IsActive",
                table: "DiscountCategory");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DiscountProduct");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DiscountCategory");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Addresses");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "DiscountId",
                table: "DiscountCategory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "DiscountCategory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_ProductId_DiscountId",
                table: "DiscountProduct",
                columns: new[] { "ProductId", "DiscountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCategory_CategoryId_DiscountId",
                table: "DiscountCategory",
                columns: new[] { "CategoryId", "DiscountId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCategory_Categories_CategoryId",
                table: "DiscountCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCategory_Discounts_DiscountId",
                table: "DiscountCategory",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCategory_Categories_CategoryId",
                table: "DiscountCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCategory_Discounts_DiscountId",
                table: "DiscountCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProduct_Discounts_DiscountId",
                table: "DiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProduct_Products_ProductId",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountProduct_ProductId_DiscountId",
                table: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCategory_CategoryId_DiscountId",
                table: "DiscountCategory");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DiscountProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "DiscountId",
                table: "DiscountCategory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "DiscountCategory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DiscountCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCategory_Categories_CategoryId",
                table: "DiscountCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCategory_Discounts_DiscountId",
                table: "DiscountCategory",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

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
    }
}
