using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnifiedContract.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttachmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_UploadDate_UploadedById",
                schema: "Common",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Common",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "Common",
                table: "Attachments",
                newName: "FilePath");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadDate",
                schema: "Common",
                table: "Attachments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                schema: "Common",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                schema: "Common",
                table: "Attachments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_EntityType_EntityId",
                schema: "Common",
                table: "Attachments",
                columns: new[] { "EntityType", "EntityId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_EntityType_EntityId",
                schema: "Common",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "Common",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "EntityType",
                schema: "Common",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                schema: "Common",
                table: "Attachments",
                newName: "Url");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadDate",
                schema: "Common",
                table: "Attachments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Common",
                table: "Attachments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploadDate_UploadedById",
                schema: "Common",
                table: "Attachments",
                columns: new[] { "UploadDate", "UploadedById" });
        }
    }
}
