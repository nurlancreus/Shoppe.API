using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlogImage_ApplicationFiles_BlogImageId",
                table: "BlogBlogImage");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlogImage_Blogs_BlogId",
                table: "BlogBlogImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Slider_SliderId",
                table: "Slides");

            migrationBuilder.DropTable(
                name: "Slider");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogBlogImage",
                table: "BlogBlogImage");

            migrationBuilder.DropIndex(
                name: "IX_BlogBlogImage_BlogId_BlogImageId",
                table: "BlogBlogImage");

            migrationBuilder.DropIndex(
                name: "IX_BlogBlogImage_BlogId_IsMain",
                table: "BlogBlogImage");

            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("3e725483-8b3c-4d73-b6a0-7962e4b9071e"));

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BlogBlogImage");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "BlogBlogImage");

            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Categories",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "BlogId",
                table: "BlogBlogImage",
                newName: "BlogSectionId");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Order",
                table: "Sections",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "TextBody",
                table: "Sections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Reviews",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogCoverId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogBlogImage",
                table: "BlogBlogImage",
                columns: new[] { "BlogSectionId", "BlogImageId" });

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ReplierId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replies_AspNetUsers_ReplierId",
                        column: x => x.ReplierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Replies_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Replies_Replies_ParentReplyId",
                        column: x => x.ParentReplyId,
                        principalTable: "Replies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SliderType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlogReactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReplyReactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reactions_Replies_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Replies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogBlogTag",
                columns: table => new
                {
                    BlogsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogBlogTag", x => new { x.BlogsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_BlogBlogTag_Blogs_BlogsId",
                        column: x => x.BlogsId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogBlogTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("c6a645b9-7c55-48a3-8477-fab3993e29b1"), new DateTime(2024, 10, 25, 12, 45, 51, 592, DateTimeKind.Utc).AddTicks(2438), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_BlogId",
                table: "Sections",
                column: "BlogId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Section_Order",
                table: "Sections",
                sql: "[Order] >= 0 AND [Order] <= 255");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogCoverId",
                table: "Blogs",
                column: "BlogCoverId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogBlogTag_TagsId",
                table: "BlogBlogTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_BlogId",
                table: "Reactions",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ReplyId",
                table: "Reactions",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId_BlogId",
                table: "Reactions",
                columns: new[] { "UserId", "BlogId" },
                unique: true,
                filter: "[BlogId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId_ReplyId",
                table: "Reactions",
                columns: new[] { "UserId", "ReplyId" },
                unique: true,
                filter: "[ReplyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_BlogId",
                table: "Replies",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ParentReplyId",
                table: "Replies",
                column: "ParentReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ReplierId",
                table: "Replies",
                column: "ReplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlogImage_ApplicationFiles_BlogImageId",
                table: "BlogBlogImage",
                column: "BlogImageId",
                principalTable: "ApplicationFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlogImage_Sections_BlogSectionId",
                table: "BlogBlogImage",
                column: "BlogSectionId",
                principalTable: "Sections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_ApplicationFiles_BlogCoverId",
                table: "Blogs",
                column: "BlogCoverId",
                principalTable: "ApplicationFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewerId",
                table: "Reviews",
                column: "ReviewerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Blogs_BlogId",
                table: "Sections",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Sliders_SliderId",
                table: "Slides",
                column: "SliderId",
                principalTable: "Sliders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlogImage_ApplicationFiles_BlogImageId",
                table: "BlogBlogImage");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlogImage_Sections_BlogSectionId",
                table: "BlogBlogImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_ApplicationFiles_BlogCoverId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Blogs_BlogId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Sliders_SliderId",
                table: "Slides");

            migrationBuilder.DropTable(
                name: "BlogBlogTag");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Sections_BlogId",
                table: "Sections");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Section_Order",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_BlogCoverId",
                table: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogBlogImage",
                table: "BlogBlogImage");

            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("c6a645b9-7c55-48a3-8477-fab3993e29b1"));

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "TextBody",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "ReviewerId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BlogCoverId",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Categories",
                newName: "Discriminator");

            migrationBuilder.RenameColumn(
                name: "BlogSectionId",
                table: "BlogBlogImage",
                newName: "BlogId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Reviews",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "BlogBlogImage",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "BlogBlogImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogBlogImage",
                table: "BlogBlogImage",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Slider",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slider", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("3e725483-8b3c-4d73-b6a0-7962e4b9071e"), new DateTime(2024, 10, 22, 11, 31, 0, 551, DateTimeKind.Utc).AddTicks(8862), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlogImage_ApplicationFiles_BlogImageId",
                table: "BlogBlogImage",
                column: "BlogImageId",
                principalTable: "ApplicationFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlogImage_Blogs_BlogId",
                table: "BlogBlogImage",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Slider_SliderId",
                table: "Slides",
                column: "SliderId",
                principalTable: "Slider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
