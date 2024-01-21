using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaFlow.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_595 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 20, 16, 53, 53, 229, DateTimeKind.Local).AddTicks(7749));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 20, 16, 53, 53, 229, DateTimeKind.Local).AddTicks(7769));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 20, 16, 50, 6, 968, DateTimeKind.Local).AddTicks(4316));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 20, 16, 50, 6, 968, DateTimeKind.Local).AddTicks(4334));
        }
    }
}
