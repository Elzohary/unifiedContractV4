using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnifiedContract.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CleanUpWarningsAndActionNeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_Department_ParentDepartmentId",
                table: "Department");

            migrationBuilder.DropTable(
                name: "ActionNeeded");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Department",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Department",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Department",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Department",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Department_ParentDepartmentId",
                table: "Department",
                column: "ParentDepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_Department_ParentDepartmentId",
                table: "Department");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Department",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Department",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Department",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Department",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateTable(
                name: "ActionNeeded",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkOrderId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    PriorityLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionNeeded", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionNeeded_PriorityLevel_PriorityLevelId",
                        column: x => x.PriorityLevelId,
                        principalTable: "PriorityLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionNeeded_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionNeeded_Users_CompletedById",
                        column: x => x.CompletedById,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionNeeded_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionNeeded_WorkOrders_WorkOrderId1",
                        column: x => x.WorkOrderId1,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionNeeded_AssignedToId",
                table: "ActionNeeded",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionNeeded_CompletedById",
                table: "ActionNeeded",
                column: "CompletedById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionNeeded_PriorityLevelId",
                table: "ActionNeeded",
                column: "PriorityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionNeeded_WorkOrderId",
                table: "ActionNeeded",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionNeeded_WorkOrderId1",
                table: "ActionNeeded",
                column: "WorkOrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Department_ParentDepartmentId",
                table: "Department",
                column: "ParentDepartmentId",
                principalTable: "Department",
                principalColumn: "Id");
        }
    }
}
