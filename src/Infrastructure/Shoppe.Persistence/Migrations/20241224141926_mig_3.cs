using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "PostCode",
                table: "Addresses",
                newName: "Phone");

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_FirstName_LastName_Email_Country_City_PostalCode_StreetAddress",
                table: "Addresses",
                columns: new[] { "FirstName", "LastName", "Email", "Country", "City", "PostalCode", "StreetAddress" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId_AddressType",
                table: "Addresses",
                columns: new[] { "UserId", "AddressType" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_FirstName_LastName_Email_Country_City_PostalCode_StreetAddress",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId_AddressType",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Addresses",
                newName: "PostCode");

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), null, new DateTime(2024, 11, 26, 12, 28, 34, 314, DateTimeKind.Utc).AddTicks(2800), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { "admin-role-id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "SuperAdmin", "SUPERADMIN", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DeactivatedAt", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenEndDate", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { "admin-user-id", 0, "7c8788f1-5b5d-427a-961e-401c30a8bab3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "nurlancreus@example.com", false, "Nurlan", "Shukurov", false, null, "NURLANCREUS@EXAMPLE.COM", "NURLANCREUS", "AQAAAAIAAYagAAAAEMfOIoNMTP34sJ1BKv3Z9Jm3PkOMZqmmNZVDHIFATMRQJtW06+zQeoJr7NX2FVvzow==", null, false, null, null, "c35fbb5f-9b35-40fc-bfb1-5999436c2547", false, null, "nurlancreus" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountId", "Name", "Type", "UpdatedAt" },
                values: new object[] { new Guid("2085a723-bf6c-4880-8ad4-53d1aa350288"), new DateTime(2024, 11, 26, 12, 28, 34, 325, DateTimeKind.Utc).AddTicks(6972), "Elegant and modern necklaces", null, "Necklaces", "Product", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[] { new Guid("4544e0a4-4b54-4d5d-8ed6-ea01a249b7c9"), new DateTime(2024, 11, 26, 12, 28, 34, 316, DateTimeKind.Utc).AddTicks(3817), "Learn about different gemstones and their meanings", "Gemstone Guide", "Blog", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountId", "Name", "Type", "UpdatedAt" },
                values: new object[] { new Guid("482a284a-38d0-4291-81df-4e532f687296"), new DateTime(2024, 11, 26, 12, 28, 34, 325, DateTimeKind.Utc).AddTicks(6978), "Stylish earrings for all occasions", null, "Earrings", "Product", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[] { new Guid("8ce8c629-683c-4171-ae12-21a089abc8d4"), new DateTime(2024, 11, 26, 12, 28, 34, 316, DateTimeKind.Utc).AddTicks(3823), "Guides and inspiration for making your own jewelry", "DIY Jewelry", "Blog", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountId", "Name", "Type", "UpdatedAt" },
                values: new object[] { new Guid("a2cda516-2a79-4817-b8a5-fee8a950742c"), new DateTime(2024, 11, 26, 12, 28, 34, 325, DateTimeKind.Utc).AddTicks(7011), "Unique brooches to complement any outfit", null, "Brooches", "Product", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("bce1b94b-33f7-43ac-8603-42bb6914cd80"), new DateTime(2024, 11, 26, 12, 28, 34, 316, DateTimeKind.Utc).AddTicks(3810), "Tips on how to take care of your jewelry", "Jewelry Care", "Blog", null },
                    { new Guid("c8d7eede-b651-4d44-ad7d-5382f6a73409"), new DateTime(2024, 11, 26, 12, 28, 34, 316, DateTimeKind.Utc).AddTicks(3820), "Jewelry gift ideas for various occasions", "Gift Ideas", "Blog", null },
                    { new Guid("cc4eb6b4-f095-4070-8254-6c1c7a0355d1"), new DateTime(2024, 11, 26, 12, 28, 34, 316, DateTimeKind.Utc).AddTicks(3814), "Updates on the latest jewelry trends", "Latest Trends", "Blog", null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountId", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("ec12c74e-4b05-4c75-89c0-a87f6f0853dc"), new DateTime(2024, 11, 26, 12, 28, 34, 325, DateTimeKind.Utc).AddTicks(6980), "Beautiful bracelets in various styles", null, "Bracelets", "Product", null },
                    { new Guid("f7020dd4-fa02-4d76-a3c6-a7b45d77c515"), new DateTime(2024, 11, 26, 12, 28, 34, 325, DateTimeKind.Utc).AddTicks(7008), "Rings for engagement, fashion, and more", null, "Rings", "Product", null }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0bdfb969-8ab2-4878-ad90-b92b4d2395f9"), new DateTime(2024, 11, 26, 12, 28, 34, 320, DateTimeKind.Utc).AddTicks(1256), null, "Fashion", "Blog", null },
                    { new Guid("2e20f584-20ea-4ab7-942a-cece301fa45f"), new DateTime(2024, 11, 26, 12, 28, 34, 320, DateTimeKind.Utc).AddTicks(1266), null, "DIY Jewelry", "Blog", null },
                    { new Guid("30296037-592f-4af1-9058-a764a281b27e"), new DateTime(2024, 11, 26, 12, 28, 34, 320, DateTimeKind.Utc).AddTicks(1260), null, "Jewelry Care", "Blog", null },
                    { new Guid("a0115f97-0260-4197-9414-56f8119c4680"), new DateTime(2024, 11, 26, 12, 28, 34, 320, DateTimeKind.Utc).AddTicks(1268), null, "Trends", "Blog", null },
                    { new Guid("b4920d85-ec88-4489-91ed-d1eea4aec69f"), new DateTime(2024, 11, 26, 12, 28, 34, 320, DateTimeKind.Utc).AddTicks(1263), null, "Gemstones", "Blog", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "admin-role-id", "admin-user-id" });

            migrationBuilder.InsertData(
                table: "SocialMediaLinks",
                columns: new[] { "Id", "AboutId", "CreatedAt", "SocialPlatform", "URL", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("44f00d58-f9b6-40d3-98c1-e0bac48d06e5"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 26, 12, 28, 34, 333, DateTimeKind.Utc).AddTicks(8610), "Youtube", "https://youtube.com/shoppe", null },
                    { new Guid("47cbf884-2877-48ad-a2e2-0a7cb8e96a59"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 26, 12, 28, 34, 333, DateTimeKind.Utc).AddTicks(8601), "Facebook", "https://facebook.com/shoppe", null },
                    { new Guid("4833c60e-0c7f-4077-8ea0-26db033b7cb0"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 26, 12, 28, 34, 333, DateTimeKind.Utc).AddTicks(8605), "X", "https://x.com/shoppe", null },
                    { new Guid("7177a8d2-2602-40e2-bce8-20322637a9fc"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 26, 12, 28, 34, 333, DateTimeKind.Utc).AddTicks(8607), "Instagram", "https://instagram.com/shoppe", null }
                });
        }
    }
}
