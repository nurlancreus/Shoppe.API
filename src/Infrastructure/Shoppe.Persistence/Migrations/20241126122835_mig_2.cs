using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Products_Id",
                table: "ProductDetails");

            migrationBuilder.DropTable(
                name: "DiscountCategory");

            migrationBuilder.DropTable(
                name: "DiscountProduct");

            migrationBuilder.DropTable(
                name: "ProductDimensions");

            migrationBuilder.DropColumn(
                name: "ProductDetailsId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductDetails",
                newName: "ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "DiscountId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "ProductDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "ProductDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<Guid>(
                name: "DiscountId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DiscountId",
                table: "Products",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DiscountId",
                table: "Categories",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Discounts_DiscountId",
                table: "Categories",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Products_ProductId",
                table: "ProductDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Discounts_DiscountId",
                table: "Products",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Discounts_DiscountId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Products_ProductId",
                table: "ProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Discounts_DiscountId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DiscountId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DiscountId",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2085a723-bf6c-4880-8ad4-53d1aa350288"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4544e0a4-4b54-4d5d-8ed6-ea01a249b7c9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("482a284a-38d0-4291-81df-4e532f687296"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8ce8c629-683c-4171-ae12-21a089abc8d4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a2cda516-2a79-4817-b8a5-fee8a950742c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bce1b94b-33f7-43ac-8603-42bb6914cd80"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c8d7eede-b651-4d44-ad7d-5382f6a73409"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cc4eb6b4-f095-4070-8254-6c1c7a0355d1"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ec12c74e-4b05-4c75-89c0-a87f6f0853dc"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f7020dd4-fa02-4d76-a3c6-a7b45d77c515"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("44f00d58-f9b6-40d3-98c1-e0bac48d06e5"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("47cbf884-2877-48ad-a2e2-0a7cb8e96a59"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("4833c60e-0c7f-4077-8ea0-26db033b7cb0"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("7177a8d2-2602-40e2-bce8-20322637a9fc"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("0bdfb969-8ab2-4878-ad90-b92b4d2395f9"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("2e20f584-20ea-4ab7-942a-cece301fa45f"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("30296037-592f-4af1-9058-a764a281b27e"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("a0115f97-0260-4197-9414-56f8119c4680"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("b4920d85-ec88-4489-91ed-d1eea4aec69f"));

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductDetails",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductDetailsId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountCategory_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountProduct_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDimensions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Width = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDimensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDimensions_ProductDetails_Id",
                        column: x => x.Id,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"),
                column: "CreatedAt",
                value: new DateTime(2024, 11, 22, 13, 35, 31, 796, DateTimeKind.Utc).AddTicks(4750));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80d3fabe-7f50-467d-a6ce-ed9972bafe91", "AQAAAAIAAYagAAAAENcMe5rfCTSwr8qIdy5PFPjlxBWmZmWFJSxLLa4YKuCB5OjOrDuKyVKvFWIgaSDVkg==", "40bb6371-88b6-49f9-a615-cf866f023fe7" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("395f86da-00d3-41cf-920c-1710dbf9c9cb"), new DateTime(2024, 11, 22, 13, 35, 31, 798, DateTimeKind.Utc).AddTicks(2786), "Guides and inspiration for making your own jewelry", "DIY Jewelry", "Blog", null },
                    { new Guid("61b1b6f4-5300-4503-a008-424081233a62"), new DateTime(2024, 11, 22, 13, 35, 31, 798, DateTimeKind.Utc).AddTicks(2760), "Tips on how to take care of your jewelry", "Jewelry Care", "Blog", null },
                    { new Guid("84867f4d-9a91-4797-9231-6ea84df162bf"), new DateTime(2024, 11, 22, 13, 35, 31, 821, DateTimeKind.Utc).AddTicks(6522), "Unique brooches to complement any outfit", "Brooches", "Product", null },
                    { new Guid("aaf65213-624b-4d1a-a2bf-1e0db70174fc"), new DateTime(2024, 11, 22, 13, 35, 31, 821, DateTimeKind.Utc).AddTicks(6495), "Elegant and modern necklaces", "Necklaces", "Product", null },
                    { new Guid("b7dc85dd-626f-4463-96b2-4245fe264317"), new DateTime(2024, 11, 22, 13, 35, 31, 798, DateTimeKind.Utc).AddTicks(2784), "Jewelry gift ideas for various occasions", "Gift Ideas", "Blog", null },
                    { new Guid("cc3471d3-1475-44b0-8f4f-b7bc53ccd956"), new DateTime(2024, 11, 22, 13, 35, 31, 798, DateTimeKind.Utc).AddTicks(2781), "Learn about different gemstones and their meanings", "Gemstone Guide", "Blog", null },
                    { new Guid("d1b47eff-ba1c-4bb2-9ba3-e8d9ae7c401f"), new DateTime(2024, 11, 22, 13, 35, 31, 821, DateTimeKind.Utc).AddTicks(6513), "Stylish earrings for all occasions", "Earrings", "Product", null },
                    { new Guid("e93de8ad-3b6b-41a7-b373-cc708129fafa"), new DateTime(2024, 11, 22, 13, 35, 31, 821, DateTimeKind.Utc).AddTicks(6519), "Rings for engagement, fashion, and more", "Rings", "Product", null },
                    { new Guid("ee9c04e4-3bf5-4fb3-ac4f-e2b8e66d40b6"), new DateTime(2024, 11, 22, 13, 35, 31, 798, DateTimeKind.Utc).AddTicks(2765), "Updates on the latest jewelry trends", "Latest Trends", "Blog", null },
                    { new Guid("f921097c-be86-4efc-9e78-f3f7b167118a"), new DateTime(2024, 11, 22, 13, 35, 31, 821, DateTimeKind.Utc).AddTicks(6516), "Beautiful bracelets in various styles", "Bracelets", "Product", null }
                });

            migrationBuilder.InsertData(
                table: "SocialMediaLinks",
                columns: new[] { "Id", "AboutId", "CreatedAt", "SocialPlatform", "URL", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("08779f9f-b74e-4cf0-a3d0-a39e62db01e2"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 22, 13, 35, 31, 828, DateTimeKind.Utc).AddTicks(2767), "Instagram", "https://instagram.com/shoppe", null },
                    { new Guid("88290fc0-78ab-4f71-9367-250d3e17b08b"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 22, 13, 35, 31, 828, DateTimeKind.Utc).AddTicks(2770), "Youtube", "https://youtube.com/shoppe", null },
                    { new Guid("bcb14f6b-2221-466e-aa4b-bb5379b1828c"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 22, 13, 35, 31, 828, DateTimeKind.Utc).AddTicks(2764), "X", "https://x.com/shoppe", null },
                    { new Guid("e6d4953c-88d2-4320-a661-3af03ddcee1c"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 22, 13, 35, 31, 828, DateTimeKind.Utc).AddTicks(2759), "Facebook", "https://facebook.com/shoppe", null }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2341d635-4e44-4861-9776-ce7975ae2fb7"), new DateTime(2024, 11, 22, 13, 35, 31, 814, DateTimeKind.Utc).AddTicks(4864), null, "Gemstones", "Blog", null },
                    { new Guid("6e29d249-3d4f-4056-8576-9152ca3bbaeb"), new DateTime(2024, 11, 22, 13, 35, 31, 814, DateTimeKind.Utc).AddTicks(4871), null, "Trends", "Blog", null },
                    { new Guid("c5e99232-8dbc-4cf0-89ad-dc86accec705"), new DateTime(2024, 11, 22, 13, 35, 31, 814, DateTimeKind.Utc).AddTicks(4868), null, "DIY Jewelry", "Blog", null },
                    { new Guid("d484e0a1-68a3-4548-b85c-c1c31d044789"), new DateTime(2024, 11, 22, 13, 35, 31, 814, DateTimeKind.Utc).AddTicks(4860), null, "Jewelry Care", "Blog", null },
                    { new Guid("ecba83f0-f1da-42af-be2a-37d0b79b5cd8"), new DateTime(2024, 11, 22, 13, 35, 31, 814, DateTimeKind.Utc).AddTicks(4852), null, "Fashion", "Blog", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCategory_CategoryId_DiscountId",
                table: "DiscountCategory",
                columns: new[] { "CategoryId", "DiscountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCategory_DiscountId",
                table: "DiscountCategory",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_DiscountId",
                table: "DiscountProduct",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_ProductId_DiscountId",
                table: "DiscountProduct",
                columns: new[] { "ProductId", "DiscountId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Products_Id",
                table: "ProductDetails",
                column: "Id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
