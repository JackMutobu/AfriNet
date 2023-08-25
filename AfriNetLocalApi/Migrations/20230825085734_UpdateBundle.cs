using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AfriNetLocalApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBundle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "ActiveBundles",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalBalance",
                table: "ActiveBundles",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("2301d884-221a-4e7d-b509-0113dcc043e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 8, 25, 8, 57, 33, 971, DateTimeKind.Utc).AddTicks(6716), new DateTime(2023, 8, 25, 8, 57, 33, 971, DateTimeKind.Utc).AddTicks(6742) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("198e6aea-6827-4562-b415-242146de9b9b"),
                column: "CreatedAt",
                value: new DateTime(2023, 8, 25, 8, 57, 33, 971, DateTimeKind.Utc).AddTicks(6914));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "ActiveBundles");

            migrationBuilder.DropColumn(
                name: "OriginalBalance",
                table: "ActiveBundles");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("2301d884-221a-4e7d-b509-0113dcc043e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 8, 10, 20, 2, 0, 555, DateTimeKind.Utc).AddTicks(2263), new DateTime(2023, 8, 10, 20, 2, 0, 555, DateTimeKind.Utc).AddTicks(2279) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("198e6aea-6827-4562-b415-242146de9b9b"),
                column: "CreatedAt",
                value: new DateTime(2023, 8, 10, 20, 2, 0, 555, DateTimeKind.Utc).AddTicks(2381));
        }
    }
}
