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
            migrationBuilder.DropTable(
                name: "BlogBlogImage");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropColumn(
                name: "PreviewUrl",
                table: "ApplicationFiles");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "ApplicationFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e20998fb-8819-43d2-a3cd-8f642aa86596", "AQAAAAIAAYagAAAAEKKW2FNpAFYVT8iXVX+blZPYjIe/qS8RvwTbbpEv7UChVjD9pN7UweQifhHLmS1Wkw==", "933f8d32-1deb-4b7a-abc1-cc19f282ac52" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationFiles_BlogId",
                table: "ApplicationFiles",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFiles_Blogs_BlogId",
                table: "ApplicationFiles",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFiles_Blogs_BlogId",
                table: "ApplicationFiles");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationFiles_BlogId",
                table: "ApplicationFiles");

            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("e228fed4-7cfd-4d5f-aab9-48193791c309"));

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "ApplicationFiles");

            migrationBuilder.AddColumn<string>(
                name: "PreviewUrl",
                table: "ApplicationFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Order = table.Column<byte>(type: "TINYINT", nullable: false),
                    TextBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.CheckConstraint("CK_Section_Order", "[Order] >= 0 AND [Order] <= 255");
                    table.ForeignKey(
                        name: "FK_Sections_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogBlogImage",
                columns: table => new
                {
                    BlogSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogBlogImage", x => new { x.BlogSectionId, x.BlogImageId });
                    table.ForeignKey(
                        name: "FK_BlogBlogImage_ApplicationFiles_BlogImageId",
                        column: x => x.BlogImageId,
                        principalTable: "ApplicationFiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlogBlogImage_Sections_BlogSectionId",
                        column: x => x.BlogSectionId,
                        principalTable: "Sections",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("523ff621-0434-453c-9c92-d125c48dabf2"), null, new DateTime(2024, 11, 9, 13, 13, 10, 714, DateTimeKind.Utc).AddTicks(2698), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e2166774-d182-4ef2-9026-0fb4ce27f87b", "AQAAAAIAAYagAAAAEP7x0aRnpNI5KZmbampcjXwHV1Hvgm8MLT8cs4KKknwxLK2I9YguLe9iiREsh/BWNg==", "41a82e4b-1cd2-42d2-b771-a44679e1a4c2" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogBlogImage_BlogImageId",
                table: "BlogBlogImage",
                column: "BlogImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_BlogId",
                table: "Sections",
                column: "BlogId");
        }
    }
}
