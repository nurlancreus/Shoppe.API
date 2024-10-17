using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Orders",
                newName: "Note");

            migrationBuilder.AddColumn<Guid>(
                name: "CouponId",
                table: "Baskets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_CouponId",
                table: "Baskets",
                column: "CouponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Coupons_CouponId",
                table: "Baskets",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Coupons_CouponId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_CouponId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "Baskets");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Orders",
                newName: "Description");
        }
    }
}
