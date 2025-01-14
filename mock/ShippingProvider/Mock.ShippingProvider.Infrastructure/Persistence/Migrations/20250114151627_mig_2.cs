using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.ShippingProvider.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApiClients",
                columns: new[] { "Id", "AddressId", "ApiKey", "CompanyName", "CreatedAt", "IsActive", "SecretKey", "UpdatedAt" },
                values: new object[] { new Guid("37c7560c-d5cd-4777-ac9c-714fc30aaf4c"), new Guid("1c261a2d-4f1c-4e23-a1b4-863c746750fc"), "", "My Company", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApiClients",
                keyColumn: "Id",
                keyValue: new Guid("37c7560c-d5cd-4777-ac9c-714fc30aaf4c"));
        }
    }
}
