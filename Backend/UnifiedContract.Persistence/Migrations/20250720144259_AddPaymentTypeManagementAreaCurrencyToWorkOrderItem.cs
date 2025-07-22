using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnifiedContract.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTypeManagementAreaCurrencyToWorkOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.EnsureSchema(
                name: "HR");

            migrationBuilder.EnsureSchema(
                name: "Client");

            migrationBuilder.EnsureSchema(
                name: "Resource");

            migrationBuilder.EnsureSchema(
                name: "Analytics");

            migrationBuilder.EnsureSchema(
                name: "Document");

            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFinalState = table.Column<bool>(type: "bit", nullable: false),
                    AllowsEditing = table.Column<bool>(type: "bit", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsScheduled = table.Column<bool>(type: "bit", nullable: false),
                    RequiresOutOfService = table.Column<bool>(type: "bit", nullable: false),
                    TypicalDurationHours = table.Column<int>(type: "int", nullable: true),
                    TypicalIntervalDays = table.Column<int>(type: "int", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPurchasable = table.Column<bool>(type: "bit", nullable: false),
                    IsReceivable = table.Column<bool>(type: "bit", nullable: false),
                    RequiresTracking = table.Column<bool>(type: "bit", nullable: false),
                    HasCost = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriorityLevel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeverityValue = table.Column<int>(type: "int", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetResponseHours = table.Column<int>(type: "int", nullable: true),
                    RequiresImmediateNotification = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsEmployee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    AllowsEditing = table.Column<bool>(type: "bit", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AlternatePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_FormattedAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentTerms = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 0m),
                    SupplierCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplier_SupplierCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "SupplierCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Supplier_SupplierCategory_SupplierCategoryId",
                        column: x => x.SupplierCategoryId,
                        principalTable: "SupplierCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    UploadedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Users_UploadedById",
                        column: x => x.UploadedById,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                schema: "Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AlternatePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_FormattedAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AccountManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Users_AccountManagerId",
                        column: x => x.AccountManagerId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Dashboard",
                schema: "Analytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dashboard_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplate",
                schema: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TemplateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TemplateContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTemplate_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentTemplate_Users_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Auth",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchasableMaterial",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Units"),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    SupplierName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasableMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasableMaterial_MaterialType_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PurchasableMaterial_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Resource",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientContact",
                schema: "Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AlternatePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPrimaryContact = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientContact_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Client",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientMaterials",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SEQ = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaterialMasterCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientMaterials_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Client",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InternalOrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletionPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TargetEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WorkOrderStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriorityLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EngineerInChargeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialsExpense = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LaborExpense = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherExpense = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Client",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrders_PriorityLevel_PriorityLevelId",
                        column: x => x.PriorityLevelId,
                        principalTable: "PriorityLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_EngineerInChargeId",
                        column: x => x.EngineerInChargeId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_WorkOrderStatus_WorkOrderStatusId",
                        column: x => x.WorkOrderStatusId,
                        principalTable: "WorkOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DashboardUser",
                schema: "Analytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DashboardUser_Dashboard_DashboardId",
                        column: x => x.DashboardId,
                        principalSchema: "Analytics",
                        principalTable: "Dashboard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DashboardUser_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DashboardWidget",
                schema: "Analytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChartType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Configuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false, defaultValue: 4),
                    Height = table.Column<int>(type: "int", nullable: false, defaultValue: 300),
                    Position = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DashboardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardWidget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DashboardWidget_Dashboard_DashboardId",
                        column: x => x.DashboardId,
                        principalSchema: "Analytics",
                        principalTable: "Dashboard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                schema: "Analytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExportFormats = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_DocumentTemplate_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "Document",
                        principalTable: "DocumentTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Report_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ActionNeeded",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkOrderId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriorityLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Permits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Authority = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentRef = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permits_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiteReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForemanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForemanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WorkDone = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ActualQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteReports_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriorityLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderActions_PriorityLevel_PriorityLevelId",
                        column: x => x.PriorityLevelId,
                        principalTable: "PriorityLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrderActions_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Receipt = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderExpenses_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderForms_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderInvoices_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderIssues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    ReportedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriorityLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderIssues_PriorityLevel_PriorityLevelId",
                        column: x => x.PriorityLevelId,
                        principalTable: "PriorityLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrderIssues_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstimatedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstimatedPriceWithVAT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualPriceWithVAT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReasonForFinalQuantity = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagementArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderItems_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderPhotos_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderRemarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeopleInvolved = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderRemarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderRemarks_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfirmedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PriorityLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderTasks_PriorityLevel_PriorityLevelId",
                        column: x => x.PriorityLevelId,
                        principalTable: "PriorityLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrderTasks_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiteReportPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteReportPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteReportPhotos_SiteReports_SiteReportId",
                        column: x => x.SiteReportId,
                        principalTable: "SiteReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allowances",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SalaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allowances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkingHours = table.Column<TimeSpan>(type: "time(5)", precision: 5, scale: 2, nullable: true),
                    LateMinutes = table.Column<TimeSpan>(type: "time(5)", precision: 5, scale: 2, nullable: true),
                    EarlyDepartureMinutes = table.Column<TimeSpan>(type: "time(5)", precision: 5, scale: 2, nullable: true),
                    OvertimeHours = table.Column<TimeSpan>(type: "time(5)", precision: 5, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsAbsent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsHalfDay = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOnLeave = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LeaveId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckInLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CheckOutLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SwiftCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Issuer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deductions",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SalaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    HeadCount = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_Department_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    BadgeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HomeStreet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HomeCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HomeState = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HomePostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HomeCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HomeFormattedAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HomeLatitude = table.Column<double>(type: "float", nullable: true),
                    HomeLongitude = table.Column<double>(type: "float", nullable: true),
                    HomeType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PersonalPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IqamaNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DirectManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkTimeRatio = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false),
                    MonthlyHours = table.Column<int>(type: "int", nullable: false),
                    AvgLateMinutes = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentProject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SickLeaveCounter = table.Column<int>(type: "int", nullable: false),
                    OffDays = table.Column<int>(type: "int", nullable: false),
                    SalaryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalaryCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "SAR"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_DirectManagerId",
                        column: x => x.DirectManagerId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Institution = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContacts",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrimaryPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecondaryPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressStreet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AddressCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressState = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AddressCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressFormattedAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AddressLatitude = table.Column<double>(type: "float", nullable: true),
                    AddressLongitude = table.Column<double>(type: "float", nullable: true),
                    IsPrimaryContact = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyContacts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkills",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProficiencyLevel = table.Column<int>(type: "int", nullable: false),
                    AcquiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Certificate = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LastUsedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSkills_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "HR",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_Employees_CurrentOperatorId",
                        column: x => x.CurrentOperatorId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentStatus_EquipmentStatusId",
                        column: x => x.EquipmentStatusId,
                        principalTable: "EquipmentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipment_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Resource",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Identifications",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuingCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identifications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leaves",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leaves_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Leaves_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManpowerAssignment",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HoursAssigned = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    WorkOrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManpowerAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManpowerAssignment_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ManpowerAssignment_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ManpowerAssignment_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceReviews",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReviewPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OverallRating = table.Column<int>(type: "int", nullable: false),
                    ManagerComments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EmployeeComments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    GoalsForNextPeriod = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TrainingRecommendations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Draft"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Employees_ReviewerId",
                        column: x => x.ReviewerId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceivableMaterials",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReturnedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstimatedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ActualQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RemainingQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ReturnedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SourceLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivableMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivableMaterials_ClientMaterials_ClientMaterialId",
                        column: x => x.ClientMaterialId,
                        principalSchema: "Resource",
                        principalTable: "ClientMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivableMaterials_Employees_ReceivedById",
                        column: x => x.ReceivedById,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivableMaterials_Employees_ReturnedById",
                        column: x => x.ReturnedById,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivableMaterials_MaterialType_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivableMaterials_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Salaries",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salaries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationHours = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "SAR"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperiences",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsCurrentEmployer = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ContactReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactReferencePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactReferenceEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Achievements = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ReasonForLeaving = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperiences_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentAssignment",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignment_Employees_OperatorId",
                        column: x => x.OperatorId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignment_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMaintenance",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MaintenanceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaintenanceStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerformedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "SAR"),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ServiceProvider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMaintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMaintenance_Employees_PerformedById",
                        column: x => x.PerformedById,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentMaintenance_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentMaintenance_MaintenanceStatus_MaintenanceStatusId",
                        column: x => x.MaintenanceStatusId,
                        principalTable: "MaintenanceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentMaintenance_MaintenanceType_MaintenanceTypeId",
                        column: x => x.MaintenanceTypeId,
                        principalTable: "MaintenanceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceCriteria",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PerformanceReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceCriteria_PerformanceReviews_PerformanceReviewId",
                        column: x => x.PerformanceReviewId,
                        principalSchema: "HR",
                        principalTable: "PerformanceReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialAssignment",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MaterialType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchasableMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceivableMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoringLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WorkOrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Units"),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialAssignment_Employees_AssignedById",
                        column: x => x.AssignedById,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialAssignment_MaterialType_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialAssignment_PurchasableMaterial_PurchasableMaterialId",
                        column: x => x.PurchasableMaterialId,
                        principalSchema: "Resource",
                        principalTable: "PurchasableMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialAssignment_ReceivableMaterials_ReceivableMaterialId",
                        column: x => x.ReceivableMaterialId,
                        principalSchema: "Resource",
                        principalTable: "ReceivableMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialAssignment_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiteReportMaterialsUsed",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialAssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteReportMaterialsUsed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteReportMaterialsUsed_MaterialAssignment_MaterialAssignmentId",
                        column: x => x.MaterialAssignmentId,
                        principalSchema: "Resource",
                        principalTable: "MaterialAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiteReportMaterialsUsed_SiteReports_SiteReportId",
                        column: x => x.SiteReportId,
                        principalTable: "SiteReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Client",
                table: "Client",
                columns: new[] { "Id", "AccountManagerId", "AlternatePhone", "CompanyName", "ContactPerson", "CreatedAt", "CreatedBy", "Email", "Industry", "IsActive", "IsDeleted", "LastModifiedAt", "LastModifiedBy", "LogoUrl", "Name", "Notes", "Phone", "VatNumber", "Website", "City", "Country", "Address_FormattedAddress", "Address_Latitude", "Address_Longitude", "PostalCode", "State", "Street" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, "0987654321", "Demo Company", "John Doe", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "demo.client@example.com", "Construction", true, false, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "", "Demo Client", "Seeded client for demo work orders", "1234567890", "VAT123456", "www.demo.com", "Riyadh", "Saudi Arabia", "Demo Street, Riyadh, Riyadh 12345, Saudi Arabia", null, null, "12345", "Riyadh", "Demo Street" });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "Id", "Action", "CreatedAt", "CreatedBy", "Description", "IsDeleted", "LastModifiedAt", "LastModifiedBy", "Module", "Name", "NormalizedName", "RoleId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "View", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to view workorders", false, null, "system", "WorkOrder", "WorkOrder.View", "WORKORDER.VIEW", null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Create", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to create workorders", false, null, "system", "WorkOrder", "WorkOrder.Create", "WORKORDER.CREATE", null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Edit", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to edit workorders", false, null, "system", "WorkOrder", "WorkOrder.Edit", "WORKORDER.EDIT", null },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "Delete", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to delete workorders", false, null, "system", "WorkOrder", "WorkOrder.Delete", "WORKORDER.DELETE", null },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "Export", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to export workorders", false, null, "system", "WorkOrder", "WorkOrder.Export", "WORKORDER.EXPORT", null },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "Approve", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to approve workorders", false, null, "system", "WorkOrder", "WorkOrder.Approve", "WORKORDER.APPROVE", null },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "Assign", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to assign workorders", false, null, "system", "WorkOrder", "WorkOrder.Assign", "WORKORDER.ASSIGN", null },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "View", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to view users", false, null, "system", "User", "User.View", "USER.VIEW", null },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "Create", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to create users", false, null, "system", "User", "User.Create", "USER.CREATE", null },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "Edit", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to edit users", false, null, "system", "User", "User.Edit", "USER.EDIT", null },
                    { new Guid("00000000-0000-0000-0000-000000000011"), "Delete", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to delete users", false, null, "system", "User", "User.Delete", "USER.DELETE", null },
                    { new Guid("00000000-0000-0000-0000-000000000012"), "Export", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to export users", false, null, "system", "User", "User.Export", "USER.EXPORT", null },
                    { new Guid("00000000-0000-0000-0000-000000000013"), "Approve", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to approve users", false, null, "system", "User", "User.Approve", "USER.APPROVE", null },
                    { new Guid("00000000-0000-0000-0000-000000000014"), "Assign", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to assign users", false, null, "system", "User", "User.Assign", "USER.ASSIGN", null },
                    { new Guid("00000000-0000-0000-0000-000000000015"), "View", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to view roles", false, null, "system", "Role", "Role.View", "ROLE.VIEW", null },
                    { new Guid("00000000-0000-0000-0000-000000000016"), "Create", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to create roles", false, null, "system", "Role", "Role.Create", "ROLE.CREATE", null },
                    { new Guid("00000000-0000-0000-0000-000000000017"), "Edit", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to edit roles", false, null, "system", "Role", "Role.Edit", "ROLE.EDIT", null },
                    { new Guid("00000000-0000-0000-0000-000000000018"), "Delete", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to delete roles", false, null, "system", "Role", "Role.Delete", "ROLE.DELETE", null },
                    { new Guid("00000000-0000-0000-0000-000000000019"), "Export", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to export roles", false, null, "system", "Role", "Role.Export", "ROLE.EXPORT", null },
                    { new Guid("00000000-0000-0000-0000-000000000020"), "Approve", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to approve roles", false, null, "system", "Role", "Role.Approve", "ROLE.APPROVE", null },
                    { new Guid("00000000-0000-0000-0000-000000000021"), "Assign", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to assign roles", false, null, "system", "Role", "Role.Assign", "ROLE.ASSIGN", null },
                    { new Guid("00000000-0000-0000-0000-000000000022"), "View", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to view hrs", false, null, "system", "HR", "HR.View", "HR.VIEW", null },
                    { new Guid("00000000-0000-0000-0000-000000000023"), "Create", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to create hrs", false, null, "system", "HR", "HR.Create", "HR.CREATE", null },
                    { new Guid("00000000-0000-0000-0000-000000000024"), "Edit", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to edit hrs", false, null, "system", "HR", "HR.Edit", "HR.EDIT", null },
                    { new Guid("00000000-0000-0000-0000-000000000025"), "Delete", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to delete hrs", false, null, "system", "HR", "HR.Delete", "HR.DELETE", null },
                    { new Guid("00000000-0000-0000-0000-000000000026"), "Export", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to export hrs", false, null, "system", "HR", "HR.Export", "HR.EXPORT", null },
                    { new Guid("00000000-0000-0000-0000-000000000027"), "Approve", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to approve hrs", false, null, "system", "HR", "HR.Approve", "HR.APPROVE", null },
                    { new Guid("00000000-0000-0000-0000-000000000028"), "Assign", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to assign hrs", false, null, "system", "HR", "HR.Assign", "HR.ASSIGN", null },
                    { new Guid("00000000-0000-0000-0000-000000000029"), "View", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to view equipments", false, null, "system", "Equipment", "Equipment.View", "EQUIPMENT.VIEW", null },
                    { new Guid("00000000-0000-0000-0000-000000000030"), "Create", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to create equipments", false, null, "system", "Equipment", "Equipment.Create", "EQUIPMENT.CREATE", null },
                    { new Guid("00000000-0000-0000-0000-000000000031"), "Edit", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to edit equipments", false, null, "system", "Equipment", "Equipment.Edit", "EQUIPMENT.EDIT", null },
                    { new Guid("00000000-0000-0000-0000-000000000032"), "Delete", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to delete equipments", false, null, "system", "Equipment", "Equipment.Delete", "EQUIPMENT.DELETE", null },
                    { new Guid("00000000-0000-0000-0000-000000000033"), "Export", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to export equipments", false, null, "system", "Equipment", "Equipment.Export", "EQUIPMENT.EXPORT", null },
                    { new Guid("00000000-0000-0000-0000-000000000034"), "Approve", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to approve equipments", false, null, "system", "Equipment", "Equipment.Approve", "EQUIPMENT.APPROVE", null },
                    { new Guid("00000000-0000-0000-0000-000000000035"), "Assign", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to assign equipments", false, null, "system", "Equipment", "Equipment.Assign", "EQUIPMENT.ASSIGN", null },
                    { new Guid("00000000-0000-0000-0000-000000000036"), "View", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to view materials", false, null, "system", "Material", "Material.View", "MATERIAL.VIEW", null },
                    { new Guid("00000000-0000-0000-0000-000000000037"), "Create", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to create materials", false, null, "system", "Material", "Material.Create", "MATERIAL.CREATE", null },
                    { new Guid("00000000-0000-0000-0000-000000000038"), "Edit", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to edit materials", false, null, "system", "Material", "Material.Edit", "MATERIAL.EDIT", null },
                    { new Guid("00000000-0000-0000-0000-000000000039"), "Delete", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to delete materials", false, null, "system", "Material", "Material.Delete", "MATERIAL.DELETE", null },
                    { new Guid("00000000-0000-0000-0000-000000000040"), "Export", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to export materials", false, null, "system", "Material", "Material.Export", "MATERIAL.EXPORT", null },
                    { new Guid("00000000-0000-0000-0000-000000000041"), "Approve", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to approve materials", false, null, "system", "Material", "Material.Approve", "MATERIAL.APPROVE", null },
                    { new Guid("00000000-0000-0000-0000-000000000042"), "Assign", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to assign materials", false, null, "system", "Material", "Material.Assign", "MATERIAL.ASSIGN", null },
                    { new Guid("00000000-0000-0000-0000-000000000043"), "View", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to view clients", false, null, "system", "Client", "Client.View", "CLIENT.VIEW", null },
                    { new Guid("00000000-0000-0000-0000-000000000044"), "Create", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to create clients", false, null, "system", "Client", "Client.Create", "CLIENT.CREATE", null },
                    { new Guid("00000000-0000-0000-0000-000000000045"), "Edit", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to edit clients", false, null, "system", "Client", "Client.Edit", "CLIENT.EDIT", null },
                    { new Guid("00000000-0000-0000-0000-000000000046"), "Delete", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to delete clients", false, null, "system", "Client", "Client.Delete", "CLIENT.DELETE", null },
                    { new Guid("00000000-0000-0000-0000-000000000047"), "Export", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to export clients", false, null, "system", "Client", "Client.Export", "CLIENT.EXPORT", null },
                    { new Guid("00000000-0000-0000-0000-000000000048"), "Approve", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to approve clients", false, null, "system", "Client", "Client.Approve", "CLIENT.APPROVE", null },
                    { new Guid("00000000-0000-0000-0000-000000000049"), "Assign", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to assign clients", false, null, "system", "Client", "Client.Assign", "CLIENT.ASSIGN", null },
                    { new Guid("00000000-0000-0000-0000-000000000050"), "View", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to view reports", false, null, "system", "Report", "Report.View", "REPORT.VIEW", null },
                    { new Guid("00000000-0000-0000-0000-000000000051"), "Create", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to create reports", false, null, "system", "Report", "Report.Create", "REPORT.CREATE", null },
                    { new Guid("00000000-0000-0000-0000-000000000052"), "Edit", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to edit reports", false, null, "system", "Report", "Report.Edit", "REPORT.EDIT", null },
                    { new Guid("00000000-0000-0000-0000-000000000053"), "Delete", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to delete reports", false, null, "system", "Report", "Report.Delete", "REPORT.DELETE", null },
                    { new Guid("00000000-0000-0000-0000-000000000054"), "Export", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Permission to export reports", false, null, "system", "Report", "Report.Export", "REPORT.EXPORT", null },
                    { new Guid("00000000-0000-0000-0000-000000000055"), "FullAccess", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Full access to all system features", false, null, "system", "Admin", "Admin.FullAccess", "ADMIN.FULLACCESS", null }
                });

            migrationBuilder.InsertData(
                table: "PriorityLevel",
                columns: new[] { "Id", "Code", "ColorCode", "CreatedAt", "CreatedBy", "Description", "DisplayOrder", "IsActive", "IsDeleted", "LastModifiedAt", "LastModifiedBy", "Metadata", "Name", "RequiresImmediateNotification", "SeverityValue", "TargetResponseHours" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "NORMAL", "#2196F3", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Normal priority.", 0, true, false, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "", "Normal", false, 2, 48 },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "HIGH", "#F44336", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "High priority.", 0, true, false, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "", "High", true, 4, 8 }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsDeleted", "LastModifiedAt", "LastModifiedBy", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Administrator role with full access", false, null, "system", "Admin", "ADMIN" },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Engineer role with technical permissions", false, null, "system", "Engineer", "ENGINEER" },
                    { new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Foreman role with supervision permissions", false, null, "system", "Foreman", "FOREMAN" },
                    { new Guid("4f511c53-efa7-4f35-a86c-ba3079493f3c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Worker role with basic permissions", false, null, "system", "Worker", "WORKER" },
                    { new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Client role with limited permissions", false, null, "system", "Client", "CLIENT" },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", "Coordinator role with management permissions", false, null, "system", "Coordinator", "COORDINATOR" }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreatedAt", "CreatedBy", "Email", "EmployeeId", "FullName", "IsActive", "IsDeleted", "LastModifiedAt", "LastModifiedBy", "PasswordHash", "UserName" },
                values: new object[,]
                {
                    { new Guid("7eb08e14-4e4c-4801-93e5-5d821bba7fd2"), null, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "system", "admin@unifiedcontract.com", null, "System Administrator", true, false, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "system", "AQAAAAIAAYagAAAAELbXp1J2NwQxX8K8QxX8K8QxX8K8QxX8K8QxX8K8QxX8K8QxX8QxX8K8Q==", "admin" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "testuser@example.com", null, "Test User", true, false, null, "seed", "849f1575ccfbf3a4d6cf00e6c5641b7fd4da2ed3e212c2d79ba9161a5a432ff0", "testuser" }
                });

            migrationBuilder.InsertData(
                table: "WorkOrderStatus",
                columns: new[] { "Id", "AllowsEditing", "Code", "ColorCode", "CreatedAt", "CreatedBy", "Description", "DisplayOrder", "IsActive", "IsCompleted", "IsDeleted", "LastModifiedAt", "LastModifiedBy", "Metadata", "Name", "RequiresApproval" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), true, "PENDING", "#FFA500", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Work order is pending.", 0, true, false, false, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "", "Pending", false },
                    { new Guid("22222222-2222-2222-2222-222222222222"), false, "COMPLETED", "#4CAF50", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Work order is completed.", 0, true, true, false, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "", "Completed", false }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsDeleted", "LastModifiedAt", "LastModifiedBy", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-000000000053"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000053"), new Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c11"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000003"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c13"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000001"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c14"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000006"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c17"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000005"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c24"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000036"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c25"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000037"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c27"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000035"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c2a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000038"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c30"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000022"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c3b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000029"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c42"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000050"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c46"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000054"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c50"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000042"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c51"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000043"), new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12") },
                    { new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289732e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000042"), new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c") },
                    { new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e72897345"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000029"), new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c") },
                    { new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e72897359"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000035"), new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c") },
                    { new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289735a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000036"), new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c") },
                    { new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289735b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000037"), new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c") },
                    { new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000001"), new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c") },
                    { new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000003"), new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c") },
                    { new Guid("4f511c53-efa7-4f35-a86c-ba3079493f0a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000036"), new Guid("4f511c53-efa7-4f35-a86c-ba3079493f3c") },
                    { new Guid("4f511c53-efa7-4f35-a86c-ba3079493f3d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000001"), new Guid("4f511c53-efa7-4f35-a86c-ba3079493f3c") },
                    { new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000001"), new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2d") },
                    { new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a7d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000050"), new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2d") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598202"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000050"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598203"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000051"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598206"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000054"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598210"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000042"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598211"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000043"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598216"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000044"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598217"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000045"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598250"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000002"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598251"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000003"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598253"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000001"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598255"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000007"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598257"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000005"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598262"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000030"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598263"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000031"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598264"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000036"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598265"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000037"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598267"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000035"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a59826a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000038"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598270"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000022"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a59827a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000028"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") },
                    { new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a59827b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", false, null, "system", new Guid("00000000-0000-0000-0000-000000000029"), new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252") }
                });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "CreatedAt", "CreatedBy", "Id", "IsDeleted", "LastModifiedAt", "LastModifiedBy" },
                values: new object[] { new Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67"), new Guid("7eb08e14-4e4c-4801-93e5-5d821bba7fd2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "system", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), false, null, "system" });

            migrationBuilder.InsertData(
                table: "WorkOrders",
                columns: new[] { "Id", "Category", "Class", "ClientId", "CompletionPercentage", "CreatedAt", "CreatedBy", "Description", "DueDate", "EngineerInChargeId", "EstimatedCost", "InternalOrderNumber", "IsDeleted", "LaborExpense", "LastModifiedAt", "LastModifiedBy", "Location", "MaterialsExpense", "OtherExpense", "PriorityLevelId", "ReceivedDate", "StartDate", "TargetEndDate", "Title", "Type", "WorkOrderNumber", "WorkOrderStatusId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Electrical", "A", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 0m, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Demo work order for testing.", new DateTime(2024, 7, 25, 17, 0, 0, 0, DateTimeKind.Utc), null, 1000m, "INT-1001", false, 300m, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Riyadh", 200m, 50m, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 7, 15, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 16, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 27, 17, 0, 0, 0, DateTimeKind.Utc), "Demo Work Order 1", "Project", "WO-1001", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Mechanical", "B", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 100m, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Second demo work order.", new DateTime(2024, 7, 18, 17, 0, 0, 0, DateTimeKind.Utc), null, 2500m, "INT-1002", false, 800m, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Jeddah", 500m, 100m, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2024, 7, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 11, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 19, 17, 0, 0, 0, DateTimeKind.Utc), "Demo Work Order 2", "Maintenance", "WO-1002", new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "WorkOrderItems",
                columns: new[] { "Id", "ActualPrice", "ActualPriceWithVAT", "ActualQuantity", "CreatedAt", "CreatedBy", "Currency", "Description", "EstimatedPrice", "EstimatedPriceWithVAT", "EstimatedQuantity", "IsDeleted", "ItemNumber", "LastModifiedAt", "LastModifiedBy", "ManagementArea", "PaymentType", "ReasonForFinalQuantity", "Unit", "UnitPrice", "WorkOrderId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), 1000m, 1150m, 10m, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "SAR", "Concrete Mix - Grade 30 for foundation work", 1000m, 1150m, 10m, false, "WOI-1001-001", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Default Area", "Fixed Price", "Used as planned", "m³", 100m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), 600m, 690m, 5m, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "SAR", "Steel Bars - 12mm for structural support", 600m, 690m, 5m, false, "WOI-1001-002", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Default Area", "Fixed Price", "Used as planned", "ton", 120m, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), 2500m, 2875m, 2m, new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "SAR", "Electrical Wiring - 2.5mm² for power distribution", 2500m, 2875m, 2m, false, "WOI-1002-001", new DateTime(2024, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed", "Default Area", "Fixed Price", "Used as planned", "m", 1250m, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") }
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

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedAt",
                schema: "Common",
                table: "ActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EntityId",
                schema: "Common",
                table: "ActivityLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EntityType",
                schema: "Common",
                table: "ActivityLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EntityType_EntityId",
                schema: "Common",
                table: "ActivityLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                schema: "Common",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Allowances_EffectiveDate",
                schema: "HR",
                table: "Allowances",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_Allowances_EndDate",
                schema: "HR",
                table: "Allowances",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Allowances_IsTaxable",
                schema: "HR",
                table: "Allowances",
                column: "IsTaxable");

            migrationBuilder.CreateIndex(
                name: "IX_Allowances_SalaryId",
                schema: "HR",
                table: "Allowances",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Allowances_Type",
                schema: "HR",
                table: "Allowances",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_FileType",
                schema: "Common",
                table: "Attachments",
                column: "FileType");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploadDate",
                schema: "Common",
                table: "Attachments",
                column: "UploadDate");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploadDate_UploadedById",
                schema: "Common",
                table: "Attachments",
                columns: new[] { "UploadDate", "UploadedById" });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploadedById",
                schema: "Common",
                table: "Attachments",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_Date",
                schema: "HR",
                table: "Attendances",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_Date_IsAbsent",
                schema: "HR",
                table: "Attendances",
                columns: new[] { "Date", "IsAbsent" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId",
                schema: "HR",
                table: "Attendances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId_Date",
                schema: "HR",
                table: "Attendances",
                columns: new[] { "EmployeeId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_IsAbsent",
                schema: "HR",
                table: "Attendances",
                column: "IsAbsent");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_IsHalfDay",
                schema: "HR",
                table: "Attendances",
                column: "IsHalfDay");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_IsOnLeave",
                schema: "HR",
                table: "Attendances",
                column: "IsOnLeave");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_LeaveId",
                schema: "HR",
                table: "Attendances",
                column: "LeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_AccountNumber",
                schema: "HR",
                table: "BankAccounts",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankName",
                schema: "HR",
                table: "BankAccounts",
                column: "BankName");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_EmployeeId",
                schema: "HR",
                table: "BankAccounts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_EmployeeId_IsPrimary",
                schema: "HR",
                table: "BankAccounts",
                columns: new[] { "EmployeeId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_IBAN",
                schema: "HR",
                table: "BankAccounts",
                column: "IBAN");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_IsPrimary",
                schema: "HR",
                table: "BankAccounts",
                column: "IsPrimary");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_EmployeeId",
                schema: "HR",
                table: "Certificates",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ExpiryDate",
                schema: "HR",
                table: "Certificates",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_IssueDate",
                schema: "HR",
                table: "Certificates",
                column: "IssueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_Issuer",
                schema: "HR",
                table: "Certificates",
                column: "Issuer");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_Verified",
                schema: "HR",
                table: "Certificates",
                column: "Verified");

            migrationBuilder.CreateIndex(
                name: "IX_Client_AccountManagerId",
                schema: "Client",
                table: "Client",
                column: "AccountManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CompanyName",
                schema: "Client",
                table: "Client",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CompanyName_IsActive",
                schema: "Client",
                table: "Client",
                columns: new[] { "CompanyName", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Email",
                schema: "Client",
                table: "Client",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Client_IsActive",
                schema: "Client",
                table: "Client",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Name",
                schema: "Client",
                table: "Client",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Name_IsActive",
                schema: "Client",
                table: "Client",
                columns: new[] { "Name", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Phone",
                schema: "Client",
                table: "Client",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Client_VatNumber",
                schema: "Client",
                table: "Client",
                column: "VatNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContact_ClientId",
                schema: "Client",
                table: "ClientContact",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContact_ClientId_Email",
                schema: "Client",
                table: "ClientContact",
                columns: new[] { "ClientId", "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientContact_ClientId_IsPrimaryContact",
                schema: "Client",
                table: "ClientContact",
                columns: new[] { "ClientId", "IsPrimaryContact" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientContact_Email",
                schema: "Client",
                table: "ClientContact",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContact_IsPrimaryContact",
                schema: "Client",
                table: "ClientContact",
                column: "IsPrimaryContact");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContact_Name",
                schema: "Client",
                table: "ClientContact",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContact_Phone",
                schema: "Client",
                table: "ClientContact",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMaterials_ClientId",
                schema: "Resource",
                table: "ClientMaterials",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMaterials_ClientId_MaterialMasterCode",
                schema: "Resource",
                table: "ClientMaterials",
                columns: new[] { "ClientId", "MaterialMasterCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientMaterials_MaterialMasterCode",
                schema: "Resource",
                table: "ClientMaterials",
                column: "MaterialMasterCode");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_IsDefault",
                schema: "Analytics",
                table: "Dashboard",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_IsPublic",
                schema: "Analytics",
                table: "Dashboard",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_IsSystem",
                schema: "Analytics",
                table: "Dashboard",
                column: "IsSystem");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_Module",
                schema: "Analytics",
                table: "Dashboard",
                column: "Module");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_Module_IsDefault",
                schema: "Analytics",
                table: "Dashboard",
                columns: new[] { "Module", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_Name",
                schema: "Analytics",
                table: "Dashboard",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_OwnerId",
                schema: "Analytics",
                table: "Dashboard",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_OwnerId_IsPublic",
                schema: "Analytics",
                table: "Dashboard",
                columns: new[] { "OwnerId", "IsPublic" });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardUser_CanEdit",
                schema: "Analytics",
                table: "DashboardUser",
                column: "CanEdit");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardUser_DashboardId",
                schema: "Analytics",
                table: "DashboardUser",
                column: "DashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardUser_DashboardId_UserId",
                schema: "Analytics",
                table: "DashboardUser",
                columns: new[] { "DashboardId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DashboardUser_UserId",
                schema: "Analytics",
                table: "DashboardUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidget_ChartType",
                schema: "Analytics",
                table: "DashboardWidget",
                column: "ChartType");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidget_DashboardId",
                schema: "Analytics",
                table: "DashboardWidget",
                column: "DashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidget_DashboardId_IsActive",
                schema: "Analytics",
                table: "DashboardWidget",
                columns: new[] { "DashboardId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidget_DashboardId_Position",
                schema: "Analytics",
                table: "DashboardWidget",
                columns: new[] { "DashboardId", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidget_IsActive",
                schema: "Analytics",
                table: "DashboardWidget",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidget_Position",
                schema: "Analytics",
                table: "DashboardWidget",
                column: "Position");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidget_Title",
                schema: "Analytics",
                table: "DashboardWidget",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_DashboardWidget_Type",
                schema: "Analytics",
                table: "DashboardWidget",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_EffectiveDate",
                schema: "HR",
                table: "Deductions",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_EndDate",
                schema: "HR",
                table: "Deductions",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_IsMandatory",
                schema: "HR",
                table: "Deductions",
                column: "IsMandatory");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_SalaryId",
                schema: "HR",
                table: "Deductions",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_Type",
                schema: "HR",
                table: "Deductions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Department_ManagerId",
                table: "Department",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_ParentDepartmentId",
                table: "Department",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_ContentType",
                schema: "Document",
                table: "DocumentTemplate",
                column: "ContentType");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_CreatedById",
                schema: "Document",
                table: "DocumentTemplate",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_IsActive",
                schema: "Document",
                table: "DocumentTemplate",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_LastModifiedById",
                schema: "Document",
                table: "DocumentTemplate",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_Name",
                schema: "Document",
                table: "DocumentTemplate",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_Name_Version",
                schema: "Document",
                table: "DocumentTemplate",
                columns: new[] { "Name", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_Type",
                schema: "Document",
                table: "DocumentTemplate",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_Type_IsActive",
                schema: "Document",
                table: "DocumentTemplate",
                columns: new[] { "Type", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Degree",
                schema: "HR",
                table: "Educations",
                column: "Degree");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_EmployeeId",
                schema: "HR",
                table: "Educations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_EmployeeId_IsVerified",
                schema: "HR",
                table: "Educations",
                columns: new[] { "EmployeeId", "IsVerified" });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_FieldOfStudy",
                schema: "HR",
                table: "Educations",
                column: "FieldOfStudy");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Institution",
                schema: "HR",
                table: "Educations",
                column: "Institution");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_StartDate_EndDate",
                schema: "HR",
                table: "Educations",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_EmployeeId",
                schema: "HR",
                table: "EmergencyContacts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_EmployeeId_IsPrimaryContact",
                schema: "HR",
                table: "EmergencyContacts",
                columns: new[] { "EmployeeId", "IsPrimaryContact" });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_EmployeeId_Name",
                schema: "HR",
                table: "EmergencyContacts",
                columns: new[] { "EmployeeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_IsPrimaryContact",
                schema: "HR",
                table: "EmergencyContacts",
                column: "IsPrimaryContact");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_Name",
                schema: "HR",
                table: "EmergencyContacts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_Relationship",
                schema: "HR",
                table: "EmergencyContacts",
                column: "Relationship");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BadgeNumber",
                schema: "HR",
                table: "Employees",
                column: "BadgeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                schema: "HR",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DirectManagerId",
                schema: "HR",
                table: "Employees",
                column: "DirectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_IqamaNumber",
                schema: "HR",
                table: "Employees",
                column: "IqamaNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobTitle",
                schema: "HR",
                table: "Employees",
                column: "JobTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JoinDate",
                schema: "HR",
                table: "Employees",
                column: "JoinDate");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Name",
                schema: "HR",
                table: "Employees",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Nationality",
                schema: "HR",
                table: "Employees",
                column: "Nationality");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkLocation",
                schema: "HR",
                table: "Employees",
                column: "WorkLocation");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_EmployeeId",
                schema: "HR",
                table: "EmployeeSkills",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_EmployeeId_SkillId",
                schema: "HR",
                table: "EmployeeSkills",
                columns: new[] { "EmployeeId", "SkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_IsFeatured",
                schema: "HR",
                table: "EmployeeSkills",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_LastUsedDate",
                schema: "HR",
                table: "EmployeeSkills",
                column: "LastUsedDate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_ProficiencyLevel",
                schema: "HR",
                table: "EmployeeSkills",
                column: "ProficiencyLevel");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_SkillId",
                schema: "HR",
                table: "EmployeeSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_CurrentOperatorId",
                table: "Equipment",
                column: "CurrentOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentStatusId",
                table: "Equipment",
                column: "EquipmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_SupplierId",
                table: "Equipment",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignment_EndDate",
                schema: "Resource",
                table: "EquipmentAssignment",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignment_EquipmentId",
                schema: "Resource",
                table: "EquipmentAssignment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignment_OperatorId",
                schema: "Resource",
                table: "EquipmentAssignment",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignment_StartDate",
                schema: "Resource",
                table: "EquipmentAssignment",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignment_WorkOrderId",
                schema: "Resource",
                table: "EquipmentAssignment",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_CompletedDate",
                schema: "Resource",
                table: "EquipmentMaintenance",
                column: "CompletedDate");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_EquipmentId",
                schema: "Resource",
                table: "EquipmentMaintenance",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_EquipmentId_ScheduledDate",
                schema: "Resource",
                table: "EquipmentMaintenance",
                columns: new[] { "EquipmentId", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_MaintenanceStatusId",
                schema: "Resource",
                table: "EquipmentMaintenance",
                column: "MaintenanceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_MaintenanceTypeId",
                schema: "Resource",
                table: "EquipmentMaintenance",
                column: "MaintenanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_PerformedById",
                schema: "Resource",
                table: "EquipmentMaintenance",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenance_ScheduledDate",
                schema: "Resource",
                table: "EquipmentMaintenance",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_EmployeeId",
                schema: "HR",
                table: "Identifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_ExpiryDate",
                schema: "HR",
                table: "Identifications",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_IssueDate",
                schema: "HR",
                table: "Identifications",
                column: "IssueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_IssuingCountry",
                schema: "HR",
                table: "Identifications",
                column: "IssuingCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_Number",
                schema: "HR",
                table: "Identifications",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_Type",
                schema: "HR",
                table: "Identifications",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_Type_Number",
                schema: "HR",
                table: "Identifications",
                columns: new[] { "Type", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_ApprovedById",
                schema: "HR",
                table: "Leaves",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_EmployeeId",
                schema: "HR",
                table: "Leaves",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_EmployeeId_Status",
                schema: "HR",
                table: "Leaves",
                columns: new[] { "EmployeeId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_EndDate",
                schema: "HR",
                table: "Leaves",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_StartDate",
                schema: "HR",
                table: "Leaves",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_StartDate_EndDate",
                schema: "HR",
                table: "Leaves",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_Status",
                schema: "HR",
                table: "Leaves",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_Type",
                schema: "HR",
                table: "Leaves",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_BadgeNumber",
                schema: "Resource",
                table: "ManpowerAssignment",
                column: "BadgeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_EmployeeId",
                schema: "Resource",
                table: "ManpowerAssignment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_EmployeeId_StartDate",
                schema: "Resource",
                table: "ManpowerAssignment",
                columns: new[] { "EmployeeId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_EndDate",
                schema: "Resource",
                table: "ManpowerAssignment",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_Name",
                schema: "Resource",
                table: "ManpowerAssignment",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_Role",
                schema: "Resource",
                table: "ManpowerAssignment",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_StartDate",
                schema: "Resource",
                table: "ManpowerAssignment",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_UserId",
                schema: "Resource",
                table: "ManpowerAssignment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_WorkOrderId",
                schema: "Resource",
                table: "ManpowerAssignment",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ManpowerAssignment_WorkOrderId_StartDate",
                schema: "Resource",
                table: "ManpowerAssignment",
                columns: new[] { "WorkOrderId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_AssignDate",
                schema: "Resource",
                table: "MaterialAssignment",
                column: "AssignDate");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_AssignedById",
                schema: "Resource",
                table: "MaterialAssignment",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_MaterialType",
                schema: "Resource",
                table: "MaterialAssignment",
                column: "MaterialType");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_MaterialType_AssignDate",
                schema: "Resource",
                table: "MaterialAssignment",
                columns: new[] { "MaterialType", "AssignDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_MaterialTypeId",
                schema: "Resource",
                table: "MaterialAssignment",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_PurchasableMaterialId",
                schema: "Resource",
                table: "MaterialAssignment",
                column: "PurchasableMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_ReceivableMaterialId",
                schema: "Resource",
                table: "MaterialAssignment",
                column: "ReceivableMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_WorkOrderId",
                schema: "Resource",
                table: "MaterialAssignment",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignment_WorkOrderId_AssignDate",
                schema: "Resource",
                table: "MaterialAssignment",
                columns: new[] { "WorkOrderId", "AssignDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                schema: "Common",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EntityId_EntityType",
                schema: "Common",
                table: "Notifications",
                columns: new[] { "EntityId", "EntityType" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsRead",
                schema: "Common",
                table: "Notifications",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "Common",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead",
                schema: "Common",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceCriteria_Name",
                schema: "HR",
                table: "PerformanceCriteria",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceCriteria_PerformanceReviewId",
                schema: "HR",
                table: "PerformanceCriteria",
                column: "PerformanceReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceCriteria_Rating",
                schema: "HR",
                table: "PerformanceCriteria",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_CompletionDate",
                schema: "HR",
                table: "PerformanceReviews",
                column: "CompletionDate");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_DueDate",
                schema: "HR",
                table: "PerformanceReviews",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_EmployeeId",
                schema: "HR",
                table: "PerformanceReviews",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_OverallRating",
                schema: "HR",
                table: "PerformanceReviews",
                column: "OverallRating");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewerId",
                schema: "HR",
                table: "PerformanceReviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewPeriodEnd",
                schema: "HR",
                table: "PerformanceReviews",
                column: "ReviewPeriodEnd");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewPeriodStart",
                schema: "HR",
                table: "PerformanceReviews",
                column: "ReviewPeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_Status",
                schema: "HR",
                table: "PerformanceReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Module_Action",
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "Module", "Action" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_NormalizedName",
                schema: "Auth",
                table: "Permissions",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                schema: "Auth",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permits_WorkOrderId",
                table: "Permits",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasableMaterial_DeliveryDate",
                schema: "Resource",
                table: "PurchasableMaterial",
                column: "DeliveryDate");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasableMaterial_MaterialTypeId",
                schema: "Resource",
                table: "PurchasableMaterial",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasableMaterial_Name",
                schema: "Resource",
                table: "PurchasableMaterial",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasableMaterial_OrderDate",
                schema: "Resource",
                table: "PurchasableMaterial",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasableMaterial_Status",
                schema: "Resource",
                table: "PurchasableMaterial",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasableMaterial_Status_MaterialTypeId",
                schema: "Resource",
                table: "PurchasableMaterial",
                columns: new[] { "Status", "MaterialTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasableMaterial_SupplierId",
                schema: "Resource",
                table: "PurchasableMaterial",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasableMaterial_SupplierId_Status",
                schema: "Resource",
                table: "PurchasableMaterial",
                columns: new[] { "SupplierId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ReceivableMaterials_ClientMaterialId",
                schema: "Resource",
                table: "ReceivableMaterials",
                column: "ClientMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivableMaterials_MaterialTypeId",
                schema: "Resource",
                table: "ReceivableMaterials",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivableMaterials_ReceivedById",
                schema: "Resource",
                table: "ReceivableMaterials",
                column: "ReceivedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivableMaterials_ReturnedById",
                schema: "Resource",
                table: "ReceivableMaterials",
                column: "ReturnedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivableMaterials_Status",
                schema: "Resource",
                table: "ReceivableMaterials",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivableMaterials_WorkOrderId",
                schema: "Resource",
                table: "ReceivableMaterials",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_CreatedByUserId",
                schema: "Analytics",
                table: "Report",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_CreatedByUserId_IsPublic",
                schema: "Analytics",
                table: "Report",
                columns: new[] { "CreatedByUserId", "IsPublic" });

            migrationBuilder.CreateIndex(
                name: "IX_Report_IsPublic",
                schema: "Analytics",
                table: "Report",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_Report_IsSystem",
                schema: "Analytics",
                table: "Report",
                column: "IsSystem");

            migrationBuilder.CreateIndex(
                name: "IX_Report_Name",
                schema: "Analytics",
                table: "Report",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Report_TemplateId",
                schema: "Analytics",
                table: "Report",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_Type",
                schema: "Analytics",
                table: "Report",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Report_Type_IsPublic",
                schema: "Analytics",
                table: "Report",
                columns: new[] { "Type", "IsPublic" });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EffectiveDate",
                schema: "HR",
                table: "Salaries",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EffectiveDate_EndDate",
                schema: "HR",
                table: "Salaries",
                columns: new[] { "EffectiveDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EmployeeId",
                schema: "HR",
                table: "Salaries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EmployeeId_EffectiveDate",
                schema: "HR",
                table: "Salaries",
                columns: new[] { "EmployeeId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EmployeeId_IsActive",
                schema: "HR",
                table: "Salaries",
                columns: new[] { "EmployeeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EndDate",
                schema: "HR",
                table: "Salaries",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_IsActive",
                schema: "HR",
                table: "Salaries",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SiteReportMaterialsUsed_MaterialAssignmentId",
                table: "SiteReportMaterialsUsed",
                column: "MaterialAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteReportMaterialsUsed_SiteReportId",
                table: "SiteReportMaterialsUsed",
                column: "SiteReportId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteReportPhotos_SiteReportId",
                table: "SiteReportPhotos",
                column: "SiteReportId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteReports_WorkOrderId",
                table: "SiteReports",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Category",
                schema: "HR",
                table: "Skills",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                schema: "HR",
                table: "Skills",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_CategoryId",
                schema: "Resource",
                table: "Supplier",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Email",
                schema: "Resource",
                table: "Supplier",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Email_IsActive",
                schema: "Resource",
                table: "Supplier",
                columns: new[] { "Email", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_IsActive",
                schema: "Resource",
                table: "Supplier",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Name",
                schema: "Resource",
                table: "Supplier",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Name_IsActive",
                schema: "Resource",
                table: "Supplier",
                columns: new[] { "Name", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Phone",
                schema: "Resource",
                table: "Supplier",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_SupplierCategoryId",
                schema: "Resource",
                table: "Supplier",
                column: "SupplierCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_VatNumber",
                schema: "Resource",
                table: "Supplier",
                column: "VatNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_EmployeeId",
                schema: "HR",
                table: "Trainings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_EmployeeId_Status",
                schema: "HR",
                table: "Trainings",
                columns: new[] { "EmployeeId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_EndDate",
                schema: "HR",
                table: "Trainings",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_Provider",
                schema: "HR",
                table: "Trainings",
                column: "Provider");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_Score",
                schema: "HR",
                table: "Trainings",
                column: "Score");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_StartDate",
                schema: "HR",
                table: "Trainings",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_StartDate_EndDate",
                schema: "HR",
                table: "Trainings",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_Status",
                schema: "HR",
                table: "Trainings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_Title",
                schema: "HR",
                table: "Trainings",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Auth",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                schema: "Auth",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "Auth",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                schema: "Auth",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsEmployee",
                schema: "Auth",
                table: "Users",
                column: "IsEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "Auth",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_CompanyName",
                schema: "HR",
                table: "WorkExperiences",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_EmployeeId",
                schema: "HR",
                table: "WorkExperiences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_EmployeeId_IsCurrentEmployer",
                schema: "HR",
                table: "WorkExperiences",
                columns: new[] { "EmployeeId", "IsCurrentEmployer" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_IsCurrentEmployer",
                schema: "HR",
                table: "WorkExperiences",
                column: "IsCurrentEmployer");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_Position",
                schema: "HR",
                table: "WorkExperiences",
                column: "Position");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_StartDate_EndDate",
                schema: "HR",
                table: "WorkExperiences",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderActions_PriorityLevelId",
                table: "WorkOrderActions",
                column: "PriorityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderActions_WorkOrderId",
                table: "WorkOrderActions",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderExpenses_WorkOrderId",
                table: "WorkOrderExpenses",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderForms_WorkOrderId",
                table: "WorkOrderForms",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderInvoices_WorkOrderId",
                table: "WorkOrderInvoices",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderIssues_PriorityLevelId",
                table: "WorkOrderIssues",
                column: "PriorityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderIssues_WorkOrderId",
                table: "WorkOrderIssues",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderItems_WorkOrderId",
                table: "WorkOrderItems",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderPhotos_WorkOrderId",
                table: "WorkOrderPhotos",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderRemarks_WorkOrderId",
                table: "WorkOrderRemarks",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ClientId",
                table: "WorkOrders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_EngineerInChargeId",
                table: "WorkOrders",
                column: "EngineerInChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_PriorityLevelId",
                table: "WorkOrders",
                column: "PriorityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_WorkOrderStatusId",
                table: "WorkOrders",
                column: "WorkOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderTasks_PriorityLevelId",
                table: "WorkOrderTasks",
                column: "PriorityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderTasks_WorkOrderId",
                table: "WorkOrderTasks",
                column: "WorkOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Allowances_Salaries_SalaryId",
                schema: "HR",
                table: "Allowances",
                column: "SalaryId",
                principalSchema: "HR",
                principalTable: "Salaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Employees_EmployeeId",
                schema: "HR",
                table: "Attendances",
                column: "EmployeeId",
                principalSchema: "HR",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Leaves_LeaveId",
                schema: "HR",
                table: "Attendances",
                column: "LeaveId",
                principalSchema: "HR",
                principalTable: "Leaves",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Employees_EmployeeId",
                schema: "HR",
                table: "BankAccounts",
                column: "EmployeeId",
                principalSchema: "HR",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Employees_EmployeeId",
                schema: "HR",
                table: "Certificates",
                column: "EmployeeId",
                principalSchema: "HR",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deductions_Salaries_SalaryId",
                schema: "HR",
                table: "Deductions",
                column: "SalaryId",
                principalSchema: "HR",
                principalTable: "Salaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Employees_ManagerId",
                table: "Department",
                column: "ManagerId",
                principalSchema: "HR",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_Employees_ManagerId",
                table: "Department");

            migrationBuilder.DropTable(
                name: "ActionNeeded");

            migrationBuilder.DropTable(
                name: "ActivityLogs",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Allowances",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Attendances",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "BankAccounts",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Certificates",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "ClientContact",
                schema: "Client");

            migrationBuilder.DropTable(
                name: "DashboardUser",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DashboardWidget",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "Deductions",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Educations",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EmergencyContacts",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EmployeeSkills",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EquipmentAssignment",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "EquipmentMaintenance",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "Identifications",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "ManpowerAssignment",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "PerformanceCriteria",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Permits");

            migrationBuilder.DropTable(
                name: "Report",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "SiteReportMaterialsUsed");

            migrationBuilder.DropTable(
                name: "SiteReportPhotos");

            migrationBuilder.DropTable(
                name: "Trainings",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "WorkExperiences",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "WorkOrderActions");

            migrationBuilder.DropTable(
                name: "WorkOrderExpenses");

            migrationBuilder.DropTable(
                name: "WorkOrderForms");

            migrationBuilder.DropTable(
                name: "WorkOrderInvoices");

            migrationBuilder.DropTable(
                name: "WorkOrderIssues");

            migrationBuilder.DropTable(
                name: "WorkOrderItems");

            migrationBuilder.DropTable(
                name: "WorkOrderPhotos");

            migrationBuilder.DropTable(
                name: "WorkOrderRemarks");

            migrationBuilder.DropTable(
                name: "WorkOrderTasks");

            migrationBuilder.DropTable(
                name: "Leaves",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Dashboard",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "Salaries",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "MaintenanceStatus");

            migrationBuilder.DropTable(
                name: "MaintenanceType");

            migrationBuilder.DropTable(
                name: "PerformanceReviews",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "DocumentTemplate",
                schema: "Document");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "MaterialAssignment",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "SiteReports");

            migrationBuilder.DropTable(
                name: "EquipmentStatus");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "PurchasableMaterial",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "ReceivableMaterials",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "ClientMaterials",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "MaterialType");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "SupplierCategory");

            migrationBuilder.DropTable(
                name: "Client",
                schema: "Client");

            migrationBuilder.DropTable(
                name: "PriorityLevel");

            migrationBuilder.DropTable(
                name: "WorkOrderStatus");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
