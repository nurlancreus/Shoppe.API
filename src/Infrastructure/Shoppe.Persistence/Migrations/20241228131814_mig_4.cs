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
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_FirstName_LastName_Email_Country_City_PostalCode_StreetAddress",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId_AddressType",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "Coupons",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Addresses",
                newName: "ShippingAddress_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Coupons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress_UserId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_UserId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AddressType_FirstName_LastName_Email_Country_City_PostalCode_StreetAddress",
                table: "Addresses",
                columns: new[] { "AddressType", "FirstName", "LastName", "Email", "Country", "City", "PostalCode", "StreetAddress" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_BillingAddress_UserId",
                table: "Addresses",
                column: "BillingAddress_UserId",
                unique: true,
                filter: "[BillingAddress_UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ShippingAddress_UserId",
                table: "Addresses",
                column: "ShippingAddress_UserId",
                unique: true,
                filter: "[ShippingAddress_UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_BillingAddress_UserId",
                table: "Addresses",
                column: "BillingAddress_UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_ShippingAddress_UserId",
                table: "Addresses",
                column: "ShippingAddress_UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_BillingAddress_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_ShippingAddress_UserId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AddressType_FirstName_LastName_Email_Country_City_PostalCode_StreetAddress",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_BillingAddress_UserId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ShippingAddress_UserId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "BillingAddress_UserId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Coupons",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_UserId",
                table: "Addresses",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
    }
}
