using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AfriNetLocalApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecharge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "For",
                table: "Bundles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("2301d884-221a-4e7d-b509-0113dcc043e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 8, 25, 12, 29, 50, 103, DateTimeKind.Utc).AddTicks(7221), new DateTime(2023, 8, 25, 12, 29, 50, 103, DateTimeKind.Utc).AddTicks(7251) });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Balance", "CreatedAt", "Type", "UpdatedAt" },
                values: new object[] { new Guid("d3d0a5cb-653c-496d-a188-2eaef509dfee"), 0m, new DateTime(2023, 8, 25, 12, 29, 50, 103, DateTimeKind.Utc).AddTicks(7324), "Retailer", new DateTime(2023, 8, 25, 12, 29, 50, 103, DateTimeKind.Utc).AddTicks(7327) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("198e6aea-6827-4562-b415-242146de9b9b"),
                column: "CreatedAt",
                value: new DateTime(2023, 8, 25, 12, 29, 50, 103, DateTimeKind.Utc).AddTicks(7297));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountId", "CreatedAt", "Email", "Firstname", "Lastname", "PasswordHash", "Phone", "Role", "UpdatedAt" },
                values: new object[] { new Guid("e860d79d-18e5-4d70-93e6-6a09a21dc6ff"), new Guid("d3d0a5cb-653c-496d-a188-2eaef509dfee"), new DateTime(2023, 8, 25, 12, 29, 50, 103, DateTimeKind.Utc).AddTicks(7347), null, "Retailer", "Default", "Test@243", "0997186015", "retailer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e860d79d-18e5-4d70-93e6-6a09a21dc6ff"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("d3d0a5cb-653c-496d-a188-2eaef509dfee"));

            migrationBuilder.DropColumn(
                name: "For",
                table: "Bundles");

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
    }
}
