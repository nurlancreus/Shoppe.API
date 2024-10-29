using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFiles_Slides_Id",
                table: "ApplicationFiles");

            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("c6a645b9-7c55-48a3-8477-fab3993e29b1"));

            migrationBuilder.RenameColumn(
                name: "SliderType",
                table: "Sliders",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "ReplierId",
                table: "Replies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SlideId",
                table: "ApplicationFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("990b90ee-d0be-4614-a51c-8acd701a07b5"), new DateTime(2024, 10, 29, 13, 48, 6, 774, DateTimeKind.Utc).AddTicks(2264), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationFiles_SlideId",
                table: "ApplicationFiles",
                column: "SlideId",
                unique: true,
                filter: "[SlideId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFiles_Slides_SlideId",
                table: "ApplicationFiles",
                column: "SlideId",
                principalTable: "Slides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFiles_Slides_SlideId",
                table: "ApplicationFiles");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationFiles_SlideId",
                table: "ApplicationFiles");

            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: new Guid("990b90ee-d0be-4614-a51c-8acd701a07b5"));

            migrationBuilder.DropColumn(
                name: "SlideId",
                table: "ApplicationFiles");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Sliders",
                newName: "SliderType");

            migrationBuilder.AlterColumn<string>(
                name: "ReplierId",
                table: "Replies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "CreatedAt", "Description", "Email", "Name", "Phone", "Title", "UpdatedAt" },
                values: new object[] { new Guid("c6a645b9-7c55-48a3-8477-fab3993e29b1"), new DateTime(2024, 10, 25, 12, 45, 51, 592, DateTimeKind.Utc).AddTicks(2438), "Who we are and why we do what we do!", "contact@shoppe.com", "Shoppe", "123-456-7890", "", null });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFiles_Slides_Id",
                table: "ApplicationFiles",
                column: "Id",
                principalTable: "Slides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
