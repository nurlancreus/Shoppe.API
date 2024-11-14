using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLinks_About_AboutId",
                table: "SocialMediaLinks");

            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("e228fed4-7cfd-4d5f-aab9-48193791c309"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AboutId",
                table: "SocialMediaLinks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), null, new DateTime(2024, 11, 14, 11, 29, 50, 389, DateTimeKind.Utc).AddTicks(3952), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c9377adc-b946-4d5a-9125-b96294affa28", "AQAAAAIAAYagAAAAEGwZCfJMGAZHLQw7ip4WOP0zQNm2bio6evVleS0pdbJ8uHrjBT1tA+G/CoW2Fd1lxA==", "e3bf8171-feb6-4474-b346-0b9b5dfce0c2" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0a9d90d2-c1db-4ebd-b629-28293a72d5c9"), new DateTime(2024, 11, 14, 11, 29, 50, 391, DateTimeKind.Utc).AddTicks(1569), "Updates on the latest jewelry trends", "Latest Trends", "Blog", null },
                    { new Guid("2a6a77d3-23a3-4d73-870d-c9be8b9d0555"), new DateTime(2024, 11, 14, 11, 29, 50, 391, DateTimeKind.Utc).AddTicks(1575), "Jewelry gift ideas for various occasions", "Gift Ideas", "Blog", null },
                    { new Guid("37ec125a-cb3e-4633-aebe-765f14ad3ffb"), new DateTime(2024, 11, 14, 11, 29, 50, 399, DateTimeKind.Utc).AddTicks(7107), "Rings for engagement, fashion, and more", "Rings", "Product", null },
                    { new Guid("6f719a55-ea0c-4334-be0b-658de44961df"), new DateTime(2024, 11, 14, 11, 29, 50, 399, DateTimeKind.Utc).AddTicks(7100), "Stylish earrings for all occasions", "Earrings", "Product", null },
                    { new Guid("a010490d-2e18-4274-9bf9-2d5b4b5efa79"), new DateTime(2024, 11, 14, 11, 29, 50, 391, DateTimeKind.Utc).AddTicks(1578), "Guides and inspiration for making your own jewelry", "DIY Jewelry", "Blog", null },
                    { new Guid("a5e7f063-353d-4d51-9bf3-43144e359712"), new DateTime(2024, 11, 14, 11, 29, 50, 391, DateTimeKind.Utc).AddTicks(1572), "Learn about different gemstones and their meanings", "Gemstone Guide", "Blog", null },
                    { new Guid("b6f2a0d3-023b-43d8-8584-67a0177eb70d"), new DateTime(2024, 11, 14, 11, 29, 50, 399, DateTimeKind.Utc).AddTicks(7103), "Beautiful bracelets in various styles", "Bracelets", "Product", null },
                    { new Guid("ce96dc00-381b-4b1f-baba-8aac959d0858"), new DateTime(2024, 11, 14, 11, 29, 50, 399, DateTimeKind.Utc).AddTicks(7095), "Elegant and modern necklaces", "Necklaces", "Product", null },
                    { new Guid("cf01d319-7c54-4e8b-a0ae-4f267be50eac"), new DateTime(2024, 11, 14, 11, 29, 50, 399, DateTimeKind.Utc).AddTicks(7110), "Unique brooches to complement any outfit", "Brooches", "Product", null },
                    { new Guid("fa9cf34e-7f8f-4d09-aef2-39db52fd44f4"), new DateTime(2024, 11, 14, 11, 29, 50, 391, DateTimeKind.Utc).AddTicks(1564), "Tips on how to take care of your jewelry", "Jewelry Care", "Blog", null }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("291b385f-79c8-4dbf-96b2-64ef0aeb74e3"), new DateTime(2024, 11, 14, 11, 29, 50, 394, DateTimeKind.Utc).AddTicks(2959), null, "DIY Jewelry", "Blog", null },
                    { new Guid("3ed6467f-ce3d-4d3a-a75d-2a6e9bd215bc"), new DateTime(2024, 11, 14, 11, 29, 50, 394, DateTimeKind.Utc).AddTicks(2956), null, "Gemstones", "Blog", null },
                    { new Guid("a4ed4bd6-2a5d-4132-a7b5-f6f1c939e69a"), new DateTime(2024, 11, 14, 11, 29, 50, 394, DateTimeKind.Utc).AddTicks(2935), null, "Fashion", "Blog", null },
                    { new Guid("b7954637-d3ca-4d8f-9085-9a280f167d64"), new DateTime(2024, 11, 14, 11, 29, 50, 394, DateTimeKind.Utc).AddTicks(2961), null, "Trends", "Blog", null },
                    { new Guid("d968fa55-9fd3-420e-aee4-a1388694729e"), new DateTime(2024, 11, 14, 11, 29, 50, 394, DateTimeKind.Utc).AddTicks(2939), null, "Jewelry Care", "Blog", null }
                });

            migrationBuilder.InsertData(
                table: "SocialMediaLinks",
                columns: new[] { "Id", "AboutId", "CreatedAt", "SocialPlatform", "URL", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6b53913a-8e9d-46de-ad51-56229baf5182"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 14, 11, 29, 50, 405, DateTimeKind.Utc).AddTicks(5938), "Youtube", "https://youtube.com/shoppe", null },
                    { new Guid("a7477752-518e-4de6-93d9-b2e99f7a225b"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 14, 11, 29, 50, 405, DateTimeKind.Utc).AddTicks(5935), "Instagram", "https://instagram.com/shoppe", null },
                    { new Guid("e12a4c6b-eabb-48d4-9da9-de9393ada406"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 14, 11, 29, 50, 405, DateTimeKind.Utc).AddTicks(5932), "X", "https://x.com/shoppe", null },
                    { new Guid("e1aff70e-bdb8-403b-a976-9e8f1ab11ac5"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 14, 11, 29, 50, 405, DateTimeKind.Utc).AddTicks(5927), "Facebook", "https://facebook.com/shoppe", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLinks_About_AboutId",
                table: "SocialMediaLinks",
                column: "AboutId",
                principalTable: "About",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLinks_About_AboutId",
                table: "SocialMediaLinks");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0a9d90d2-c1db-4ebd-b629-28293a72d5c9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2a6a77d3-23a3-4d73-870d-c9be8b9d0555"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("37ec125a-cb3e-4633-aebe-765f14ad3ffb"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6f719a55-ea0c-4334-be0b-658de44961df"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a010490d-2e18-4274-9bf9-2d5b4b5efa79"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a5e7f063-353d-4d51-9bf3-43144e359712"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b6f2a0d3-023b-43d8-8584-67a0177eb70d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ce96dc00-381b-4b1f-baba-8aac959d0858"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cf01d319-7c54-4e8b-a0ae-4f267be50eac"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fa9cf34e-7f8f-4d09-aef2-39db52fd44f4"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("6b53913a-8e9d-46de-ad51-56229baf5182"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("a7477752-518e-4de6-93d9-b2e99f7a225b"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("e12a4c6b-eabb-48d4-9da9-de9393ada406"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("e1aff70e-bdb8-403b-a976-9e8f1ab11ac5"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("291b385f-79c8-4dbf-96b2-64ef0aeb74e3"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("3ed6467f-ce3d-4d3a-a75d-2a6e9bd215bc"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("a4ed4bd6-2a5d-4132-a7b5-f6f1c939e69a"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("b7954637-d3ca-4d8f-9085-9a280f167d64"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("d968fa55-9fd3-420e-aee4-a1388694729e"));

            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AboutId",
                table: "SocialMediaLinks",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("e228fed4-7cfd-4d5f-aab9-48193791c309"), null, new DateTime(2024, 11, 11, 11, 45, 20, 983, DateTimeKind.Utc).AddTicks(7579), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e20998fb-8819-43d2-a3cd-8f642aa86596", "AQAAAAIAAYagAAAAEKKW2FNpAFYVT8iXVX+blZPYjIe/qS8RvwTbbpEv7UChVjD9pN7UweQifhHLmS1Wkw==", "933f8d32-1deb-4b7a-abc1-cc19f282ac52" });

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLinks_About_AboutId",
                table: "SocialMediaLinks",
                column: "AboutId",
                principalTable: "About",
                principalColumn: "Id");
        }
    }
}
