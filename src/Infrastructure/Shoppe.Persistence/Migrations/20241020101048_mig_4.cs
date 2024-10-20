using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogBlogImage",
                table: "BlogBlogImage");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationFiles_Id_IsMain",
                table: "ApplicationFiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BlogBlogImage");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BlogBlogImage");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "ApplicationFiles");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "BlogBlogImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "ApplicationFiles",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ApplicationFiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogBlogImage",
                table: "BlogBlogImage",
                column: "Id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Discount_DiscountPercentage",
                table: "Discounts",
                sql: "[DiscountPercentage] > 0 AND [DiscountPercentage] <= 100");

            migrationBuilder.CreateIndex(
                name: "IX_BlogBlogImage_BlogId_BlogImageId",
                table: "BlogBlogImage",
                columns: new[] { "BlogId", "BlogImageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogBlogImage_BlogId_IsMain",
                table: "BlogBlogImage",
                columns: new[] { "BlogId", "IsMain" },
                unique: true,
                filter: "[IsMain] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationFiles_UserId_IsMain",
                table: "ApplicationFiles",
                columns: new[] { "UserId", "IsMain" },
                unique: true,
                filter: "[IsMain] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFiles_AspNetUsers_UserId",
                table: "ApplicationFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFiles_AspNetUsers_UserId",
                table: "ApplicationFiles");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Discount_DiscountPercentage",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogBlogImage",
                table: "BlogBlogImage");

            migrationBuilder.DropIndex(
                name: "IX_BlogBlogImage_BlogId_BlogImageId",
                table: "BlogBlogImage");

            migrationBuilder.DropIndex(
                name: "IX_BlogBlogImage_BlogId_IsMain",
                table: "BlogBlogImage");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationFiles_UserId_IsMain",
                table: "ApplicationFiles");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "BlogBlogImage");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ApplicationFiles");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BlogBlogImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BlogBlogImage",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "ApplicationFiles",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(34)",
                oldMaxLength: 34);

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "ApplicationFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogBlogImage",
                table: "BlogBlogImage",
                columns: new[] { "BlogId", "BlogImageId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationFiles_Id_IsMain",
                table: "ApplicationFiles",
                columns: new[] { "Id", "IsMain" },
                unique: true,
                filter: "[IsMain] = 1");
        }
    }
}
