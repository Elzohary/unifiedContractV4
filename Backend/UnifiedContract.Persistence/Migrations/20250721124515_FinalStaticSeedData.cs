using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnifiedContract.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FinalStaticSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4f511c53-efa7-4f35-a86c-ba3079493f3c"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2d"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 21, 12, 42, 28, 754, DateTimeKind.Utc).AddTicks(9633), null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 21, 12, 42, 28, 755, DateTimeKind.Utc).AddTicks(1671), null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 21, 12, 42, 28, 755, DateTimeKind.Utc).AddTicks(1687), null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("4f511c53-efa7-4f35-a86c-ba3079493f3c"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 21, 12, 42, 28, 755, DateTimeKind.Utc).AddTicks(1691), null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2d"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 21, 12, 42, 28, 755, DateTimeKind.Utc).AddTicks(1702), null });

            migrationBuilder.UpdateData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252"),
                columns: new[] { "CreatedAt", "LastModifiedAt" },
                values: new object[] { new DateTime(2025, 7, 21, 12, 42, 28, 755, DateTimeKind.Utc).AddTicks(1706), null });
        }
    }
}
