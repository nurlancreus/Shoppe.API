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

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reply_Depth",
                table: "Replies",
                sql: "[Depth] >= 0 AND [Depth] <= 3");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reply_Depth",
                table: "Replies");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0df684d7-f1e2-4ce4-ba10-9244f80f564c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1f234ff7-6449-4672-acc9-2367e35f91da"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4645dbc2-cd11-4566-80d4-2721503ddf28"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4a22634d-ce5b-402c-97e7-80f9f83979e9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("64864688-174d-47a7-91e0-9748be9f2e95"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("79ffed00-12c6-4412-9626-db6d3bf308bd"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7b3b90e4-47a6-4224-b7aa-1be6e659ec28"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9641d6a9-c01b-4773-9a3c-9febb34c3a51"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9dba532d-1587-4187-bd04-fe03d73364e9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f259588c-da2c-44ad-aa56-487485f9f0b8"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("0506dba2-f9d1-4aa8-9c4d-ae27e86ca8d1"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("23580eb4-e67e-43c0-8982-e21f6bf454df"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("de96a81f-3ae6-4fff-8268-6af3acb656da"));

            migrationBuilder.DeleteData(
                table: "SocialMediaLinks",
                keyColumn: "Id",
                keyValue: new Guid("f8e5cc42-206b-46a2-9993-264cc3c094e0"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("01a800da-8383-4c89-867f-c17322c72145"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("3a409aaf-d4b2-4a83-a011-d8bfdfa13b33"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("4cac68d5-70fa-4652-8e5e-8aa88820f2be"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("66387255-f5a8-4aa2-b800-4b2981a337de"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("94184f86-1b37-4be5-b464-b3598200519d"));

            migrationBuilder.UpdateData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"),
                column: "CreatedAt",
                value: new DateTime(2024, 11, 16, 12, 23, 43, 851, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ca09e91-f2a9-45e0-8e07-949b90ab8a1d", "AQAAAAIAAYagAAAAENm3XkC+9N2NKQBjshRnyFatf55EyBTI+7qe1LNin3WZTStZVNSczb5QoJqF+j2sgg==", "ba05b545-cc7a-4cf4-86c1-135ef4ffba28" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("29fbeeee-dcb3-4331-8cc0-8ccdfbe7a01c"), new DateTime(2024, 11, 16, 12, 23, 43, 887, DateTimeKind.Utc).AddTicks(5131), "Unique brooches to complement any outfit", "Brooches", "Product", null },
                    { new Guid("2e394add-31b9-484b-9b7e-9b08bc2fc7b4"), new DateTime(2024, 11, 16, 12, 23, 43, 887, DateTimeKind.Utc).AddTicks(5105), "Elegant and modern necklaces", "Necklaces", "Product", null },
                    { new Guid("42b30ac8-3380-400b-91ca-dbf0a7102306"), new DateTime(2024, 11, 16, 12, 23, 43, 854, DateTimeKind.Utc).AddTicks(3598), "Updates on the latest jewelry trends", "Latest Trends", "Blog", null },
                    { new Guid("59c6d7c2-1911-48da-ae7c-29cb46078746"), new DateTime(2024, 11, 16, 12, 23, 43, 854, DateTimeKind.Utc).AddTicks(3585), "Tips on how to take care of your jewelry", "Jewelry Care", "Blog", null },
                    { new Guid("68797f71-e145-49e0-8d86-844c1ecb1e18"), new DateTime(2024, 11, 16, 12, 23, 43, 887, DateTimeKind.Utc).AddTicks(5126), "Rings for engagement, fashion, and more", "Rings", "Product", null },
                    { new Guid("b533e44a-8c48-437e-8eda-ac8d3ee6fcab"), new DateTime(2024, 11, 16, 12, 23, 43, 854, DateTimeKind.Utc).AddTicks(3635), "Guides and inspiration for making your own jewelry", "DIY Jewelry", "Blog", null },
                    { new Guid("bf464cb2-002e-4f86-870a-3dc6c556293b"), new DateTime(2024, 11, 16, 12, 23, 43, 854, DateTimeKind.Utc).AddTicks(3628), "Jewelry gift ideas for various occasions", "Gift Ideas", "Blog", null },
                    { new Guid("cc9bcfc6-d213-4724-8c58-0bb2068fd909"), new DateTime(2024, 11, 16, 12, 23, 43, 887, DateTimeKind.Utc).AddTicks(5116), "Stylish earrings for all occasions", "Earrings", "Product", null },
                    { new Guid("ece4baf5-08cb-4a64-88ac-cbcc0cee5943"), new DateTime(2024, 11, 16, 12, 23, 43, 854, DateTimeKind.Utc).AddTicks(3605), "Learn about different gemstones and their meanings", "Gemstone Guide", "Blog", null },
                    { new Guid("ef55cf4c-c917-4762-8c0b-0a371418e3c5"), new DateTime(2024, 11, 16, 12, 23, 43, 887, DateTimeKind.Utc).AddTicks(5121), "Beautiful bracelets in various styles", "Bracelets", "Product", null }
                });

            migrationBuilder.InsertData(
                table: "SocialMediaLinks",
                columns: new[] { "Id", "AboutId", "CreatedAt", "SocialPlatform", "URL", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1384094b-7281-47ad-9ffc-e87237e9922e"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 16, 12, 23, 43, 901, DateTimeKind.Utc).AddTicks(1825), "Instagram", "https://instagram.com/shoppe", null },
                    { new Guid("5334fa0b-5461-4510-8093-f4dfd2be1fbe"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 16, 12, 23, 43, 901, DateTimeKind.Utc).AddTicks(1816), "X", "https://x.com/shoppe", null },
                    { new Guid("a4996047-92c5-4099-a069-a764e579307e"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 16, 12, 23, 43, 901, DateTimeKind.Utc).AddTicks(1773), "Facebook", "https://facebook.com/shoppe", null },
                    { new Guid("f2d0c8bb-5a8c-4dc1-97bc-76a9bd2616e2"), new Guid("dd37583b-9c78-4159-a1e7-ccdc6a8be9eb"), new DateTime(2024, 11, 16, 12, 23, 43, 901, DateTimeKind.Utc).AddTicks(1833), "Youtube", "https://youtube.com/shoppe", null }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("3be49e7f-74da-4ff1-8981-57513492a00a"), new DateTime(2024, 11, 16, 12, 23, 43, 862, DateTimeKind.Utc).AddTicks(8670), null, "Jewelry Care", "Blog", null },
                    { new Guid("4db58e0c-57df-4216-bf3b-24f189c9e89e"), new DateTime(2024, 11, 16, 12, 23, 43, 862, DateTimeKind.Utc).AddTicks(8662), null, "Fashion", "Blog", null },
                    { new Guid("ac3fb6b8-224a-453c-a763-2cf01b714e0b"), new DateTime(2024, 11, 16, 12, 23, 43, 862, DateTimeKind.Utc).AddTicks(8675), null, "Gemstones", "Blog", null },
                    { new Guid("d85856df-c757-4f6e-9f10-c2095130110d"), new DateTime(2024, 11, 16, 12, 23, 43, 862, DateTimeKind.Utc).AddTicks(8680), null, "DIY Jewelry", "Blog", null },
                    { new Guid("fb428df7-6fa5-481f-b569-320bb2fa9c43"), new DateTime(2024, 11, 16, 12, 23, 43, 862, DateTimeKind.Utc).AddTicks(8685), null, "Trends", "Blog", null }
                });
        }
    }
}
