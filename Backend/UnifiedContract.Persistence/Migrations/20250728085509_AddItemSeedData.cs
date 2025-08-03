using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnifiedContract.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddItemSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "ClientId", "CreatedAt", "CreatedBy", "Currency", "Description", "IsActive", "IsDeleted", "ItemNumber", "LastModifiedAt", "LastModifiedBy", "ManagementArea", "PaymentType", "Unit", "UnitPrice" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "SAR", "Concrete Mix - Grade 30 for foundation work", true, false, "ITEM-001", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Construction", "Fixed Price", "m³", 100m },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "SAR", "Steel Reinforcement - Grade 60", true, false, "ITEM-002", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Construction", "Fixed Price", "kg", 5m },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "SAR", "Electrical Cable - 3x2.5mm²", true, false, "ITEM-003", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Electrical", "Fixed Price", "m", 15m },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "SAR", "PVC Pipe - 110mm diameter", true, false, "ITEM-004", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Plumbing", "Fixed Price", "m", 25m },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "SAR", "Paint - Interior White", true, false, "ITEM-005", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Finishing", "Fixed Price", "L", 50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000005"));
        }
    }
}
