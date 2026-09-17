using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLiKhoHang.Migrations
{
    /// <inheritdoc />
    public partial class InitDB_New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountPayables",
                columns: table => new
                {
                    APId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AmountDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPayables", x => x.APId);
                });

            migrationBuilder.CreateTable(
                name: "AccountReceivables",
                columns: table => new
                {
                    ARId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AmountDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountReceivables", x => x.ARId);
                });

            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    BenefitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BenefitName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenefitType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.BenefitId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DefaultCurrency = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    VehicleInfo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AssignedOrdersCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltySettings",
                columns: table => new
                {
                    LoyaltySettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettingKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SettingName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SettingValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltySettings", x => x.LoyaltySettingsId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "RedemptionRules",
                columns: table => new
                {
                    RedemptionRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsRequired = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RewardType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValidityDays = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedemptionRules", x => x.RedemptionRuleId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultPermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SupplierCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PrimaryContactName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "TierConfigs",
                columns: table => new
                {
                    TierConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    TierName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TierConfigs", x => x.TierConfigId);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    VoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageLimitPerCustomer = table.Column<int>(type: "int", nullable: true),
                    TotalUsageLimit = table.Column<int>(type: "int", nullable: true),
                    AppliedTo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.VoucherId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                columns: table => new
                {
                    SettingKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchScoped = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.SettingKey);
                    table.ForeignKey(
                        name: "FK_ApplicationSettings_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PreferredBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Branches_PreferredBranchId",
                        column: x => x.PreferredBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportsCache",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Period = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsCache", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ReportsCache_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantTables",
                columns: table => new
                {
                    TableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsOutdoor = table.Column<bool>(type: "bit", nullable: false),
                    MapX = table.Column<int>(type: "int", nullable: true),
                    MapY = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantTables", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_RestaurantTables_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_Shifts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageLocations",
                columns: table => new
                {
                    StorageLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Capacity = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageLocations", x => x.StorageLocationId);
                    table.ForeignKey(
                        name: "FK_StorageLocations_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolePermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmploymentType = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Staff_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Staff_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TierBenefits",
                columns: table => new
                {
                    TierBenefitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TierConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BenefitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TierBenefits", x => x.TierBenefitId);
                    table.ForeignKey(
                        name: "FK_TierBenefits_Benefits_BenefitId",
                        column: x => x.BenefitId,
                        principalTable: "Benefits",
                        principalColumn: "BenefitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TierBenefits_TierConfigs_TierConfigId",
                        column: x => x.TierConfigId,
                        principalTable: "TierConfigs",
                        principalColumn: "TierConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyAccounts",
                columns: table => new
                {
                    LoyaltyAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    TierEffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TierEffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalEarnedPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRedeemedPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyAccounts", x => x.LoyaltyAccountId);
                    table.ForeignKey(
                        name: "FK_LoyaltyAccounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitchenSections",
                columns: table => new
                {
                    KitchenSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrinterDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenSections", x => x.KitchenSectionId);
                    table.ForeignKey(
                        name: "FK_KitchenSections_Devices_PrinterDeviceId",
                        column: x => x.PrinterDeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReservedTableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PartySize = table.Column<int>(type: "int", nullable: false),
                    ReserveStartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReserveEndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_RestaurantTables_ReservedTableId",
                        column: x => x.ReservedTableId,
                        principalTable: "RestaurantTables",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CurrentQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ReorderLevel = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ReorderQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CostPerUnit = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ShelfLifeDays = table.Column<int>(type: "int", nullable: true),
                    StorageCondition = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StorageLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_InventoryItems_StorageLocations_StorageLocationId",
                        column: x => x.StorageLocationId,
                        principalTable: "StorageLocations",
                        principalColumn: "StorageLocationId");
                    table.ForeignKey(
                        name: "FK_InventoryItems_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClockInAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockOutAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClockType = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_Staff_ApprovedByStaffId",
                        column: x => x.ApprovedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    ChangedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangeSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditId);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Staff_ChangedByStaffId",
                        column: x => x.ChangedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidTo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceiptImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expenses_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Staff_CreatedByStaffId",
                        column: x => x.CreatedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RelatedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Account = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_Staff_CreatedByStaffId",
                        column: x => x.CreatedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    OrderSource = table.Column<int>(type: "int", nullable: false),
                    PlacedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoundingAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    IsVoided = table.Column<bool>(type: "bit", nullable: false),
                    VoidReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_RestaurantTables_TableId",
                        column: x => x.TableId,
                        principalTable: "RestaurantTables",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Staff_ClosedByStaffId",
                        column: x => x.ClosedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Staff_CreatedByStaffId",
                        column: x => x.CreatedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollRecords",
                columns: table => new
                {
                    PayrollId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollPeriodStart = table.Column<DateOnly>(type: "date", nullable: false),
                    PayrollPeriodEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    GrossPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deductions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Taxes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollRecords", x => x.PayrollId);
                    table.ForeignKey(
                        name: "FK_PayrollRecords_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.PurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Staff_CreatedByStaffId",
                        column: x => x.CreatedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffPermissions",
                columns: table => new
                {
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffPermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffPermissions", x => new { x.StaffId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_StaffPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffPermissions_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffShifts",
                columns: table => new
                {
                    StaffShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffShifts", x => x.StaffShiftId);
                    table.ForeignKey(
                        name: "FK_StaffShifts_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffShifts_Staff_ApprovedByStaffId",
                        column: x => x.ApprovedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffShifts_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockCounts",
                columns: table => new
                {
                    StockCountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PerformedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CountDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCounts", x => x.StockCountId);
                    table.ForeignKey(
                        name: "FK_StockCounts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockCounts_Staff_PerformedByStaffId",
                        column: x => x.PerformedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultServingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailableOnline = table.Column<bool>(type: "bit", nullable: false),
                    PreparationTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    KitchenSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsComposite = table.Column<bool>(type: "bit", nullable: false),
                    PortionSize = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Taxable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.DishId);
                    table.ForeignKey(
                        name: "FK_Dishes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Dishes_KitchenSections_KitchenSectionId",
                        column: x => x.KitchenSectionId,
                        principalTable: "KitchenSections",
                        principalColumn: "KitchenSectionId");
                });

            migrationBuilder.CreateTable(
                name: "StockAlertNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ThresholdQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    RecipientEmails = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    NotificationStatus = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAlertNotifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_StockAlertNotifications_InventoryItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAlertThresholds",
                columns: table => new
                {
                    ThresholdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinimumThreshold = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AlertEmails = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAlertThresholds", x => x.ThresholdId);
                    table.ForeignKey(
                        name: "FK_StockAlertThresholds_InventoryItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    StockTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.StockTransactionId);
                    table.ForeignKey(
                        name: "FK_StockTransactions_InventoryItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Staff_CreatedByStaffId",
                        column: x => x.CreatedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StorageItems",
                columns: table => new
                {
                    StorageItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StorageLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LotNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItems", x => x.StorageItemId);
                    table.ForeignKey(
                        name: "FK_StorageItems_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageItems_StorageLocations_StorageLocationId",
                        column: x => x.StorageLocationId,
                        principalTable: "StorageLocations",
                        principalColumn: "StorageLocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOrders",
                columns: table => new
                {
                    DeliveryOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    RecipientPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOrders", x => x.DeliveryOrderId);
                    table.ForeignKey(
                        name: "FK_DeliveryOrders_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyTransactions",
                columns: table => new
                {
                    LoyaltyTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoyaltyAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyTransactions", x => x.LoyaltyTransactionId);
                    table.ForeignKey(
                        name: "FK_LoyaltyTransactions_LoyaltyAccounts_LoyaltyAccountId",
                        column: x => x.LoyaltyAccountId,
                        principalTable: "LoyaltyAccounts",
                        principalColumn: "LoyaltyAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoyaltyTransactions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CardLast4 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    AuthCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProcessedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Staff_ProcessedByStaffId",
                        column: x => x.ProcessedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLines",
                columns: table => new
                {
                    POLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityOrdered = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityReceived = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLines", x => x.POLineId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_InventoryItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierInvoices",
                columns: table => new
                {
                    SupplierInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LinkedPOId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierInvoices", x => x.SupplierInvoiceId);
                    table.ForeignKey(
                        name: "FK_SupplierInvoices_PurchaseOrders_LinkedPOId",
                        column: x => x.LinkedPOId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierInvoices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockCountLines",
                columns: table => new
                {
                    StockCountLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockCountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountedQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SystemQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Variance = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCountLines", x => x.StockCountLineId);
                    table.ForeignKey(
                        name: "FK_StockCountLines_InventoryItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockCountLines_StockCounts_StockCountId",
                        column: x => x.StockCountId,
                        principalTable: "StockCounts",
                        principalColumn: "StockCountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishIngredients",
                columns: table => new
                {
                    DishIngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityPerPortion = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitMultiplier = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishIngredients", x => x.DishIngredientId);
                    table.ForeignKey(
                        name: "FK_DishIngredients_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "DishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishIngredients_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuPriceHistory",
                columns: table => new
                {
                    MenuPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChangedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPriceHistory", x => x.MenuPriceId);
                    table.ForeignKey(
                        name: "FK_MenuPriceHistory_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "DishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPriceHistory_Staff_ChangedByStaffId",
                        column: x => x.ChangedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    RefundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalPaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundMethod = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_Refunds_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Refunds_Payments_OriginalPaymentId",
                        column: x => x.OriginalPaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Refunds_Staff_ProcessedByStaffId",
                        column: x => x.ProcessedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentOrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    KitchenSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDiscounted = table.Column<bool>(type: "bit", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "DishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_KitchenSections_KitchenSectionId",
                        column: x => x.KitchenSectionId,
                        principalTable: "KitchenSections",
                        principalColumn: "KitchenSectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuPriceHistory_MenuPriceId",
                        column: x => x.MenuPriceId,
                        principalTable: "MenuPriceHistory",
                        principalColumn: "MenuPriceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_OrderItems_ParentOrderItemId",
                        column: x => x.ParentOrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitchenTickets",
                columns: table => new
                {
                    KitchenTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TicketNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    KitchenSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PrintedByStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitchenTickets", x => x.KitchenTicketId);
                    table.ForeignKey(
                        name: "FK_KitchenTickets_KitchenSections_KitchenSectionId",
                        column: x => x.KitchenSectionId,
                        principalTable: "KitchenSections",
                        principalColumn: "KitchenSectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KitchenTickets_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KitchenTickets_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KitchenTickets_Staff_PrintedByStaffId",
                        column: x => x.PrintedByStaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId");
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "Address", "Code", "CreatedAt", "DefaultCurrency", "IsActive", "Name", "Phone", "TimeZone", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("550e8400-e29b-41d4-a716-446655440001"), "123 Đường Lê Lợi, Quận 1, TP.HCM", "CN001", new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(4504), "VND", true, "Chi nhánh Trung tâm", "028-1234-5678", "SE Asia Standard Time", null },
                    { new Guid("550e8400-e29b-41d4-a716-446655440002"), "456 Đường Nguyễn Kiệm, Quận Phú Nhuận, TP.HCM", "CN002", new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(4522), "VND", true, "Chi nhánh Phú Nhuận", "028-8765-4321", "SE Asia Standard Time", null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Active", "CreatedAt", "Description", "DisplayOrder", "Name", "ParentCategoryId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("990e8400-e29b-41d4-a716-446655440001"), true, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5099), "Các món ăn chính", 1, "Món chính", null, null },
                    { new Guid("990e8400-e29b-41d4-a716-446655440002"), true, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5102), "Các loại đồ uống", 2, "Đồ uống", null, null }
                });

            migrationBuilder.InsertData(
                table: "KitchenSections",
                columns: new[] { "KitchenSectionId", "CreatedAt", "Description", "Name", "PrinterDeviceId", "UpdatedAt" },
                values: new object[] { new Guid("110e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5499), "Bếp nấu các món chính", "Bếp chính", null, null });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("770e8400-e29b-41d4-a716-446655440001"), "Xem dashboard", "Dashboard.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440010"), "Xem danh mục món ăn", "Menu.Category.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440011"), "Tạo danh mục", "Menu.Category.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440012"), "Sửa danh mục", "Menu.Category.Edit" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440013"), "Xóa danh mục", "Menu.Category.Delete" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440020"), "Xem món ăn", "Menu.Dish.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440021"), "Tạo món ăn", "Menu.Dish.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440022"), "Sửa món ăn", "Menu.Dish.Edit" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440023"), "Xóa món ăn", "Menu.Dish.Delete" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440030"), "Xem lịch sử giá", "Menu.PriceHistory.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440031"), "Tạo lịch sử giá", "Menu.PriceHistory.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440040"), "Xem nguyên liệu", "Menu.DishIngredient.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440041"), "Tạo nguyên liệu", "Menu.DishIngredient.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440042"), "Sửa nguyên liệu", "Menu.DishIngredient.Edit" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440050"), "Xem tồn kho", "Inventory.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440051"), "Nhập kho", "Inventory.Import" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440052"), "Xuất kho", "Inventory.Export" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440053"), "Kiểm kê kho", "Inventory.StockTaking" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440060"), "Xem danh sách nhân viên", "Staff.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440061"), "Tạo nhân viên", "Staff.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440062"), "Sửa thông tin nhân viên", "Staff.Edit" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440063"), "Xóa nhân viên", "Staff.Delete" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440070"), "Xem bảng lương", "Staff.Payroll.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440071"), "Quản lý bảng lương", "Staff.Payroll.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440080"), "Quản lý phân quyền", "Staff.RolesAndPermissions.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440090"), "Xem lịch làm việc", "Staff.Shift.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440091"), "Tạo ca làm việc", "Staff.Shift.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440092"), "Sửa ca làm việc", "Staff.Shift.Edit" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440100"), "Xem chấm công", "Staff.Attendance.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440101"), "Quản lý chấm công", "Staff.Attendance.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440110"), "Xem danh sách khách hàng", "Customer.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440111"), "Tạo khách hàng", "Customer.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440112"), "Sửa khách hàng", "Customer.Edit" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440113"), "Xóa khách hàng", "Customer.Delete" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440120"), "Quản lý chương trình thân thiết", "Customer.Loyalty.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440121"), "Quản lý voucher", "Customer.Voucher.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440130"), "Xem danh sách bàn", "POS.Table.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440131"), "Quản lý bàn ăn", "POS.Table.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440132"), "Xem dashboard quản lý bàn", "POS.TableManagement.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440140"), "Tạo đơn hàng", "POS.Order.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440141"), "Xem đơn hàng", "POS.Order.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440142"), "Sửa đơn hàng", "POS.Order.Edit" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440143"), "Xóa đơn hàng", "POS.Order.Delete" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440150"), "Xem báo cáo bán hàng", "POS.SalesReport.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440160"), "Xem đặt bàn", "Reservation.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440161"), "Tạo đặt bàn", "Reservation.Create" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440162"), "Sửa đặt bàn", "Reservation.Edit" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440163"), "Xóa đặt bàn", "Reservation.Delete" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440170"), "Quản lý đặt món online", "OnlineOrder.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440171"), "Quản lý giao hàng", "Delivery.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440180"), "Xem nhà cung cấp", "Supplier.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440181"), "Quản lý nhà cung cấp", "Supplier.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440182"), "Quản lý công nợ", "Supplier.Debt.Manage" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440190"), "Xem báo cáo doanh thu", "Report.Revenue.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440191"), "Xem báo cáo tài chính", "Report.Finance.View" },
                    { new Guid("770e8400-e29b-41d4-a716-446655440192"), "Xem phân tích hoạt động", "Report.Analytics.View" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "CreatedAt", "DefaultPermissions", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("660e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(4775), "all", "Quyền quản trị viên hệ thống", "Administrator", null },
                    { new Guid("660e8400-e29b-41d4-a716-446655440002"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(4778), "branch_management,staff_management,inventory_management", "Quyền quản lý chi nhánh", "Manager", null },
                    { new Guid("660e8400-e29b-41d4-a716-446655440003"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(4781), "order_management,customer_service", "Quyền nhân viên phục vụ", "Staff", null }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Address", "CreatedAt", "Currency", "Email", "IsActive", "Name", "PaymentTerms", "Phone", "PrimaryContactName", "SupplierCode", "UpdatedAt" },
                values: new object[] { new Guid("bb0e8400-e29b-41d4-a716-446655440009"), "789 Đường Cách Mạng Tháng 8, Quận 3, TP.HCM", new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5217), "VND", "contact@thucphamabc.vn", true, "Công ty TNHH Thực phẩm ABC", "Net 30", "028-1111-2222", "Nguyễn Văn B", null, null });

            migrationBuilder.InsertData(
                table: "Vouchers",
                columns: new[] { "VoucherId", "AppliedTo", "Code", "CreatedAt", "Description", "DiscountType", "DiscountValue", "IsActive", "MinimumOrderAmount", "TotalUsageLimit", "UpdatedAt", "UsageLimitPerCustomer", "ValidFrom", "ValidTo" },
                values: new object[] { new Guid("ff0e8400-e29b-41d4-a716-446655440014"), null, "WELCOME10", new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5971), "Giảm 10% cho đơn hàng đầu tiên", 0, 10m, true, 100000m, 100, null, null, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5968), new DateTime(2026, 5, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5968) });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "CreatedAt", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "Notes", "PhoneNumber", "PreferredBranchId", "UpdatedAt" },
                values: new object[] { new Guid("ee0e8400-e29b-41d4-a716-446655440001"), null, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5430), new DateOnly(1990, 1, 1), "nguyenvana@gmail.com", "Nguyễn", true, "Văn A", null, "0912345678", new Guid("550e8400-e29b-41d4-a716-446655440001"), null });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "DishId", "Active", "CategoryId", "CreatedAt", "DefaultServingPrice", "Description", "ImageUrl", "IsAvailableOnline", "IsComposite", "KitchenSectionId", "Name", "PortionSize", "PreparationTimeMinutes", "Sku", "Taxable", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("aa0e8400-e29b-41d4-a716-446655440001"), true, new Guid("990e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5142), 45000m, "Phở bò truyền thống với thịt bò tươi ngon", "https://fohlafood.vn/cdn/shop/articles/bi-quyet-nau-phi-bo-ngon-tuyet-dinh.jpg?v=1712213789", true, true, null, "Phở bò", null, 15, null, true, null },
                    { new Guid("aa0e8400-e29b-41d4-a716-446655440002"), true, new Guid("990e8400-e29b-41d4-a716-446655440002"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5148), 25000m, "Cà phê sữa đá đậm đà", "https://lyoncoffee.com.vn/wp-content/uploads/cong-thuc-pha-ca-phe-sua-da-sai-gon.jpg", true, false, null, "Cà phê sữa đá", null, 5, null, true, null }
                });

            migrationBuilder.InsertData(
                table: "RestaurantTables",
                columns: new[] { "TableId", "Area", "BranchId", "Code", "CreatedAt", "IsOutdoor", "MapX", "MapY", "Name", "Seats", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("ff0e8400-e29b-41d4-a716-446655440001"), null, new Guid("550e8400-e29b-41d4-a716-446655440001"), "T01", new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5461), false, null, null, "Bàn số 1", 4, 0, null },
                    { new Guid("ff0e8400-e29b-41d4-a716-446655440002"), null, new Guid("550e8400-e29b-41d4-a716-446655440001"), "T02", new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5469), false, null, null, "Bàn số 2", 6, 0, null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "RolePermissionId" },
                values: new object[,]
                {
                    { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("d522f545-a058-43e4-90e6-f09385e310e5") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440010"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("1a51c39b-9317-47ce-a438-c2f745b0d726") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440011"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("ef3158f5-76cb-4a4b-a771-170594dd1e23") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440012"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("7e59bfd0-224c-4208-aeb8-894dc6166a44") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440013"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("12ec2ac8-9b21-470b-8975-a9eb06c31947") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("392f6014-8afb-4c06-ae82-d8a28991c61f") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440021"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("9494edea-05e2-457e-aff7-31d4093efe75") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440022"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("b9e302bf-5261-4e24-8699-62eb3a0137aa") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440023"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("12275193-d31d-446a-a44c-095a972be0dd") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440030"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("8a3b4d83-958e-432a-8e5a-8303f06b12bc") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440031"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("92774b1c-db75-4b5f-adee-7bdaa8dddf67") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440040"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("0fdad17f-3be5-4b0b-8e2a-5498fc40a4b8") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440041"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("afdb9d8f-a88d-4e61-9e2f-23acf7fd49c7") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440042"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("0b492abc-0c39-42f4-bfff-67f764fbcade") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440050"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("661d66b4-4ba4-4365-8c06-22743e7be47b") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440051"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("f970e856-1523-47ba-a5e4-dc8759179937") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440052"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("766ce593-6901-46cc-9ab4-3055e6ef3e7d") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440053"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("5defe221-e085-4a9a-999a-f247734c2a1b") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440060"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("4fe1caae-f0c9-4f77-b622-a01141b5f761") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440061"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("c826b263-92c0-47f9-a81b-0dbb82d61bb0") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440062"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("c09706d8-0d22-4048-9a7d-ff32799cae56") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440063"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("2cedf8c9-15de-44a1-a718-c975c978d943") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440070"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("3a2a9873-c8d3-414f-ab6e-24c26513adf7") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440071"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("cdc884c0-fda5-4071-852d-fec3aea9a39c") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440080"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("022c2063-e828-48bb-b3c5-06c9672edd9f") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440090"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("55cadccf-792d-4445-8d63-b5039a52a47a") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440091"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("53f8fbbd-4fd1-44cc-8cb1-8e942bf33493") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440092"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("8320345d-feb3-4f4a-a468-b8069dfc262f") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440100"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("3e48092c-a93e-4507-85b0-07493df9890c") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440101"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("7c0ddda2-a344-4f88-a821-6b3f55ced03e") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("d5f7a426-9afa-4f27-9dbd-fa5f3394e4dd") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440111"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("15d6779e-54c1-420b-b165-5cc7c78c3acf") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440112"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("b1acc2ed-6b71-4839-9602-ec5f216f9a43") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440113"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("e16b0191-5181-48be-9114-2cf08aed1b93") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440120"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("69436140-f609-4d76-83cb-4bb75fa965d4") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440121"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("ac50974f-6989-4468-887a-891d6375fc7e") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("4b8e8542-15ae-49de-83e8-abd32e198368") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440131"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("3561257d-8c19-4f64-9e34-cbb491cb59bc") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("f913f4f8-1a01-4a56-921e-9af72133c86f") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440140"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("6f41e711-8ad4-4d29-9703-689720139a4b") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("262d572d-717b-4ff7-962a-a48d9c8bc35c") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440142"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("6cfc2913-cecd-4a4f-9886-e9ef32784701") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440143"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("334204ef-08e4-4117-b3c2-e449fcf2c7c1") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440150"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("56c41db7-9647-41f3-84a9-adc31cfa02fa") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440160"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("73db2caf-2fe6-4785-a404-97e8b1ce694b") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440161"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("298cf19b-7f40-4af9-b5b0-451cf011c92c") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440162"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("65bc2f67-dd54-4331-9132-f63ceba27ea8") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440163"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("ae50fdaf-c76f-4675-9360-bd9a6b64cb4a") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440170"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("455bcb13-dc68-4066-bdf6-46c0e7067ede") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440171"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("400bb6ec-5929-46a5-b765-0dff2a0197f3") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440180"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("e827f83e-fe3a-40aa-8b03-a9c7d5663861") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440181"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("2cfdd030-946f-409d-8b3e-216f6949135e") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440182"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("34efab7b-f992-4c0e-a592-67229cb33933") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440190"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("07aad8e2-96af-4207-a9f9-51aaae34212c") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440191"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("73cb12d8-5957-48cf-8e51-d44b94673378") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440192"), new Guid("660e8400-e29b-41d4-a716-446655440001"), new Guid("7427a457-e751-48bb-9c51-9931be124cf1") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("6cef6cb9-f0a4-490e-a323-1504fb5ce8b3") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440010"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("d954d469-dec1-4d5b-a244-126b4a35b82e") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440011"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("8d08cbfb-e0fa-4b41-bb67-6bc31d2aaec0") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440012"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("bb289dc2-82a0-4c05-9354-b59aa28c15c0") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("b50619ef-eac7-44ed-a8d2-f5f36e25bb36") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440021"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("f775a8a2-dc1c-4723-80b7-fec3b5f3e997") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440022"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("820273bb-75a6-4026-bc67-073658ff9f70") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440030"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("e790e05e-bbdf-4d1b-9cfc-36e1122280ee") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440031"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("cc357185-cac9-4933-be79-4f9712797957") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440040"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("6e1b2640-85f5-4be2-afb4-2e67edff1f46") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440041"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("8c41229e-95d4-4da9-9389-71781eb1c51f") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440042"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("eb0da11d-cb04-4b9e-aceb-5ed37baaa26e") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440050"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("fc5205e2-1f09-4e45-83ba-4ea48fcdedce") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440051"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("532e2a69-b118-4a18-b4a4-26320e4390b9") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440052"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("2e0ab48b-4213-4121-bb9b-e3d825549db6") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440053"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("e8c279ec-22e9-40a0-b437-33beb974eac6") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440060"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("cec26a60-0301-4c96-8e0c-8af5a067352e") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440061"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("ec90bfd6-98b7-4e5a-b1cd-e3e64e98c679") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440062"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("a3447466-a190-433e-a0ba-14a4f2564f59") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440070"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("100128ab-0f00-4b49-b1aa-5116730b65e4") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440071"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("4c530857-db9f-4cb5-b8cb-a079f7b25530") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440090"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("a839136c-e774-4fb0-a08a-13bc727e1ecc") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440091"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("cbbcfe48-ccf6-41cb-82ad-a519cf7f1adb") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440092"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("8f67b1a5-8905-4122-9801-c0863cb6689e") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440100"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("ad401f50-5987-4b22-9ea9-c695ed3f88f3") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440101"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("5ec93176-c050-4869-88a9-fe4d7ada62cf") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("a10c83b4-e558-48ce-96bf-424d55b4c1f8") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440111"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("695e501a-002e-4b48-811e-9972e3028a6b") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440112"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("82447e8b-d68a-4a46-a649-8c1c8eb851b0") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440120"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("b5a15b5d-67b9-4809-90d2-774832794148") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440121"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("4d13ce01-3433-429b-832a-a4930c178755") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("5909a697-0e67-468d-8ba1-ccfe92dd6f3e") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440131"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("624b6c4b-c2a4-4cc7-a4ba-0b13ce09d404") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("e3bb4346-9dfb-4694-abeb-9ab5d40496fd") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("fe07f11a-d393-4320-9079-3bbe2c5a3438") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440150"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("22d6410d-7c9a-420c-b978-ca4d511a0647") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440160"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("fb880a8f-353f-4881-9ff1-41ad4ba94b46") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440161"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("7e626787-bae6-437d-a235-46989b46945f") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440162"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("23d377db-3b57-49e7-bc91-0b7db5fc7802") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440190"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("730c2663-730c-45ce-a0c1-102a8514779f") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440191"), new Guid("660e8400-e29b-41d4-a716-446655440002"), new Guid("4decfbd3-0fc9-41ca-8d90-1296e0e127ef") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440003"), new Guid("21a10192-d0e5-4984-8943-b26c0915c035") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440003"), new Guid("5247082e-8d26-49a6-b723-e39aa0c0222a") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440003"), new Guid("2d5e4318-c447-45ce-b7da-329eb7f81208") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440003"), new Guid("8469a915-0e1e-4031-8d05-6bd8d2cf2117") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440003"), new Guid("6d5fa3ac-367d-4b7a-a5ee-3efecdacb597") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440140"), new Guid("660e8400-e29b-41d4-a716-446655440003"), new Guid("ae0c8b88-8484-4944-b7c8-05591f450ca4") },
                    { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440003"), new Guid("006d7008-79e3-4047-a74b-c9abce9d71c4") }
                });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "ShiftId", "BranchId", "CreatedAt", "EndTime", "Name", "StartTime", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("220e8400-e29b-41d4-a716-446655440001"), new Guid("550e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5527), new TimeSpan(0, 14, 0, 0, 0), "Ca sáng", new TimeSpan(0, 6, 0, 0, 0), null },
                    { new Guid("220e8400-e29b-41d4-a716-446655440002"), new Guid("550e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5530), new TimeSpan(0, 22, 0, 0, 0), "Ca tối", new TimeSpan(0, 14, 0, 0, 0), null }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "Address", "AvatarUrl", "BaseSalary", "BranchId", "CreatedAt", "Email", "EmployeeNumber", "EmploymentType", "FirstName", "HireDate", "HourlyRate", "IsActive", "LastName", "Password", "PhoneNumber", "RoleId", "TaxId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("880e8400-e29b-41d4-a716-446655440001"), null, null, 15000000m, new Guid("550e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5049), "admin@quanlikhoahang.vn", "ADM001", 0, "Nguyễn", new DateTime(2023, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5030), null, true, "Văn Admin", "admin123", "0901234567", new Guid("660e8400-e29b-41d4-a716-446655440001"), null, null },
                    { new Guid("880e8400-e29b-41d4-a716-446655440002"), null, null, 12000000m, new Guid("550e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5057), "manager@quanlikhoahang.vn", "MGR001", 0, "Trần", new DateTime(2024, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5055), null, true, "Thị Manager", "manager123", "0902345678", new Guid("660e8400-e29b-41d4-a716-446655440002"), null, null }
                });

            migrationBuilder.InsertData(
                table: "StorageLocations",
                columns: new[] { "StorageLocationId", "Address", "BranchId", "Capacity", "Condition", "CreatedAt", "Description", "IsActive", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("aa0e8400-e29b-41d4-a716-446655440001"), "Khu A, Tầng 1", new Guid("550e8400-e29b-41d4-a716-446655440001"), 1000m, 2, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5250), "Kho lạnh để bảo quản thịt, cá", true, "Kho lạnh A1", 1, null },
                    { new Guid("aa0e8400-e29b-41d4-a716-446655440002"), "Khu B, Tầng 2", new Guid("550e8400-e29b-41d4-a716-446655440001"), 2000m, 4, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5255), "Kho khô để bảo quản cà phê, đường, muối", true, "Kho khô B2", 3, null }
                });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "AttendanceId", "ApprovedByStaffId", "ClockInAt", "ClockOutAt", "ClockType", "CreatedAt", "Location", "Notes", "ShiftId", "StaffId", "Status", "UpdatedAt" },
                values: new object[] { new Guid("aa0e8400-e29b-41d4-a716-446655440008"), new Guid("880e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 16, 18, 28, 22, 965, DateTimeKind.Local).AddTicks(5833), new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5834), 2, new DateTime(2025, 11, 16, 18, 28, 22, 965, DateTimeKind.Local).AddTicks(5838), null, null, new Guid("220e8400-e29b-41d4-a716-446655440001"), new Guid("880e8400-e29b-41d4-a716-446655440002"), 1, new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5839) });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "ItemId", "CostPerUnit", "CreatedAt", "CurrentQuantity", "ImageUrl", "IsActive", "Name", "ReorderLevel", "ReorderQuantity", "ShelfLifeDays", "Sku", "StorageCondition", "StorageLocationId", "SupplierId", "Unit", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("cc0e8400-e29b-41d4-a716-446655440001"), 250000m, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5342), 50m, null, true, "Thịt bò", 10m, 25m, null, "THITBO001", 2, new Guid("aa0e8400-e29b-41d4-a716-446655440001"), new Guid("bb0e8400-e29b-41d4-a716-446655440009"), "kg", null },
                    { new Guid("cc0e8400-e29b-41d4-a716-446655440002"), 150000m, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5349), 20m, null, true, "Hạt cà phê", 5m, 10m, null, "CAFE001", 4, new Guid("aa0e8400-e29b-41d4-a716-446655440002"), new Guid("bb0e8400-e29b-41d4-a716-446655440009"), "kg", null }
                });

            migrationBuilder.InsertData(
                table: "LoyaltyAccounts",
                columns: new[] { "LoyaltyAccountId", "CreatedAt", "CustomerId", "PointsBalance", "Tier", "TierEffectiveFrom", "TierEffectiveTo", "TotalEarnedPoints", "TotalRedeemedPoints", "UpdatedAt" },
                values: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440005"), new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5704), new Guid("ee0e8400-e29b-41d4-a716-446655440001"), 77m, 2, new DateTime(2025, 5, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5703), null, 77m, 0m, null });

            migrationBuilder.InsertData(
                table: "MenuPriceHistory",
                columns: new[] { "MenuPriceId", "ChangedByStaffId", "CreatedAt", "DishId", "EffectiveFrom", "EffectiveTo", "Price", "Reason", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("bb0e8400-e29b-41d4-a716-446655440001"), new Guid("880e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 5, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5176), new Guid("aa0e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 5, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5171), null, 45000m, "Giá khởi tạo", null },
                    { new Guid("bb0e8400-e29b-41d4-a716-446655440002"), new Guid("880e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 5, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5182), new Guid("aa0e8400-e29b-41d4-a716-446655440002"), new DateTime(2025, 5, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5180), null, 25000m, "Giá khởi tạo", null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "BalanceDue", "BranchId", "ClosedByStaffId", "CreatedAt", "CreatedByStaffId", "CustomerId", "DiscountAmount", "IsVoided", "Notes", "OrderNumber", "OrderSource", "OrderType", "PaidAmount", "PaymentStatus", "PlacedAt", "RoundingAdjustment", "ServedAt", "ServiceChargeAmount", "Status", "SubTotal", "TableId", "TaxAmount", "TotalAmount", "UpdatedAt", "VoidReason" },
                values: new object[,]
                {
                    { new Guid("330e8400-e29b-41d4-a716-446655440001"), 0m, new Guid("550e8400-e29b-41d4-a716-446655440001"), null, new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(5570), new Guid("880e8400-e29b-41d4-a716-446655440002"), new Guid("ee0e8400-e29b-41d4-a716-446655440001"), 0m, false, "Khách hàng thân thiết", "ORD001", 0, 0, 77000m, 2, new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(5565), 0m, null, 0m, 5, 70000m, new Guid("ff0e8400-e29b-41d4-a716-446655440001"), 7000m, 77000m, new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5571), null },
                    { new Guid("cc0e8400-e29b-41d4-a716-446655440001"), 172500m, new Guid("550e8400-e29b-41d4-a716-446655440001"), null, new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(6090), new Guid("880e8400-e29b-41d4-a716-446655440001"), new Guid("ee0e8400-e29b-41d4-a716-446655440001"), 0m, false, null, "ORD-20251020-001", 0, 0, 0m, 0, new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(6085), 0m, null, 7500m, 2, 150000m, new Guid("ff0e8400-e29b-41d4-a716-446655440001"), 15000m, 172500m, null, null },
                    { new Guid("cc0e8400-e29b-41d4-a716-446655440002"), 0m, new Guid("550e8400-e29b-41d4-a716-446655440001"), new Guid("880e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(6100), new Guid("880e8400-e29b-41d4-a716-446655440001"), null, 0m, false, null, "ORD-20251020-002", 0, 1, 93500m, 2, new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(6095), 0m, new DateTime(2025, 11, 17, 1, 58, 22, 965, DateTimeKind.Local).AddTicks(6096), 0m, 3, 85000m, null, 8500m, 93500m, new DateTime(2025, 11, 17, 1, 58, 22, 965, DateTimeKind.Local).AddTicks(6101), null },
                    { new Guid("cc0e8400-e29b-41d4-a716-446655440003"), 0m, new Guid("550e8400-e29b-41d4-a716-446655440001"), new Guid("880e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 16, 21, 28, 22, 965, DateTimeKind.Local).AddTicks(6112), new Guid("880e8400-e29b-41d4-a716-446655440001"), new Guid("ee0e8400-e29b-41d4-a716-446655440001"), 20000m, false, null, "ORD-20251019-001", 0, 0, 207000m, 2, new DateTime(2025, 11, 16, 21, 28, 22, 965, DateTimeKind.Local).AddTicks(6107), 0m, new DateTime(2025, 11, 16, 22, 28, 22, 965, DateTimeKind.Local).AddTicks(6108), 9000m, 5, 200000m, new Guid("ff0e8400-e29b-41d4-a716-446655440001"), 18000m, 207000m, new DateTime(2025, 11, 16, 22, 28, 22, 965, DateTimeKind.Local).AddTicks(6113), null }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "PurchaseOrderId", "BranchId", "CreatedAt", "CreatedByStaffId", "Currency", "ExpectedDeliveryDate", "Notes", "OrderedAt", "PONumber", "Status", "SupplierId", "TotalAmount", "UpdatedAt" },
                values: new object[] { new Guid("cc0e8400-e29b-41d4-a716-446655440010"), new Guid("550e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 10, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5870), new Guid("880e8400-e29b-41d4-a716-446655440002"), "VND", new DateTime(2025, 11, 12, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5867), "Đơn hàng nhập hàng tháng 10", new DateTime(2025, 11, 10, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5865), "PO001", 3, new Guid("bb0e8400-e29b-41d4-a716-446655440009"), 10000000m, new DateTime(2025, 11, 12, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5871) });

            migrationBuilder.InsertData(
                table: "StaffShifts",
                columns: new[] { "StaffShiftId", "ApprovedAt", "ApprovedByStaffId", "CreatedAt", "Notes", "RejectionReason", "ShiftId", "StaffId", "Status", "UpdatedAt", "WorkDate" },
                values: new object[,]
                {
                    { new Guid("aa0e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 1, 58, 22, 965, DateTimeKind.Local).AddTicks(6001), new Guid("880e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(6004), "Ca làm việc chính", null, new Guid("220e8400-e29b-41d4-a716-446655440001"), new Guid("880e8400-e29b-41d4-a716-446655440001"), 1, null, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Local) },
                    { new Guid("aa0e8400-e29b-41d4-a716-446655440002"), null, null, new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(6008), "Ca làm việc phụ", null, new Guid("220e8400-e29b-41d4-a716-446655440002"), new Guid("880e8400-e29b-41d4-a716-446655440002"), 0, null, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Local) },
                    { new Guid("aa0e8400-e29b-41d4-a716-446655440003"), null, null, new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(6014), "Ca làm việc ngày mai", null, new Guid("220e8400-e29b-41d4-a716-446655440001"), new Guid("880e8400-e29b-41d4-a716-446655440001"), 0, null, new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Local) }
                });

            migrationBuilder.InsertData(
                table: "DishIngredients",
                columns: new[] { "DishIngredientId", "CreatedAt", "DishId", "InventoryItemId", "IsOptional", "QuantityPerPortion", "UnitMultiplier", "UpdatedAt" },
                values: new object[] { new Guid("dd0e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5384), new Guid("aa0e8400-e29b-41d4-a716-446655440001"), new Guid("cc0e8400-e29b-41d4-a716-446655440001"), false, 0.3m, 1m, null });

            migrationBuilder.InsertData(
                table: "LoyaltyTransactions",
                columns: new[] { "LoyaltyTransactionId", "CreatedAt", "LoyaltyAccountId", "OrderId", "Points", "Reason", "Type", "UpdatedAt" },
                values: new object[] { new Guid("880e8400-e29b-41d4-a716-446655440006"), new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5730), new Guid("770e8400-e29b-41d4-a716-446655440005"), new Guid("330e8400-e29b-41d4-a716-446655440001"), 77m, "Tích điểm từ đơn hàng ORD001", 0, null });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "CreatedAt", "DiscountAmount", "DishId", "IsDiscounted", "KitchenSectionId", "LineTotal", "MenuPriceId", "OrderId", "ParentOrderItemId", "Quantity", "RequestedAt", "ServedAt", "SpecialInstructions", "Status", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("440e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(5599), 0m, new Guid("aa0e8400-e29b-41d4-a716-446655440001"), false, new Guid("110e8400-e29b-41d4-a716-446655440001"), 45000m, new Guid("bb0e8400-e29b-41d4-a716-446655440001"), new Guid("330e8400-e29b-41d4-a716-446655440001"), null, 1m, new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(5597), null, "Ít cay", 4, 45000m, new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5600) },
                    { new Guid("440e8400-e29b-41d4-a716-446655440002"), new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(5606), 0m, new Guid("aa0e8400-e29b-41d4-a716-446655440002"), false, null, 25000m, new Guid("bb0e8400-e29b-41d4-a716-446655440002"), new Guid("330e8400-e29b-41d4-a716-446655440001"), null, 1m, new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(5605), null, null, 4, 25000m, new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5607) }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "AuthCode", "CardLast4", "CardType", "CreatedAt", "Notes", "OrderId", "PaymentDate", "PaymentMethod", "ProcessedByStaffId", "Status", "TransactionReference", "UpdatedAt" },
                values: new object[] { new Guid("550e8400-e29b-41d4-a716-446655440003"), 77000m, null, null, null, new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5644), "Thanh toán bằng tiền mặt", new Guid("330e8400-e29b-41d4-a716-446655440001"), new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5639), 0, new Guid("880e8400-e29b-41d4-a716-446655440002"), 2, "PAY001", null });

            migrationBuilder.InsertData(
                table: "PurchaseOrderLines",
                columns: new[] { "POLineId", "CreatedAt", "ItemId", "LineTotal", "PurchaseOrderId", "QuantityOrdered", "QuantityReceived", "UnitCost", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("dd0e8400-e29b-41d4-a716-446655440011"), new DateTime(2025, 11, 10, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5900), new Guid("cc0e8400-e29b-41d4-a716-446655440001"), 6000000m, new Guid("cc0e8400-e29b-41d4-a716-446655440010"), 25m, 25m, 240000m, new DateTime(2025, 11, 12, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5901) },
                    { new Guid("dd0e8400-e29b-41d4-a716-446655440012"), new DateTime(2025, 11, 10, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5906), new Guid("cc0e8400-e29b-41d4-a716-446655440002"), 1400000m, new Guid("cc0e8400-e29b-41d4-a716-446655440010"), 10m, 10m, 140000m, new DateTime(2025, 11, 12, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5907) }
                });

            migrationBuilder.InsertData(
                table: "StockTransactions",
                columns: new[] { "StockTransactionId", "CreatedAt", "CreatedByStaffId", "ExpiryDate", "ItemId", "LotNumber", "Note", "Quantity", "ReferenceId", "SupplierId", "TotalCost", "TransactionType", "UnitCost", "UpdatedAt" },
                values: new object[] { new Guid("990e8400-e29b-41d4-a716-446655440007"), new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5805), new Guid("880e8400-e29b-41d4-a716-446655440002"), null, new Guid("cc0e8400-e29b-41d4-a716-446655440001"), "LOT001", "Xuất kho cho đơn hàng ORD001", 0.3m, new Guid("330e8400-e29b-41d4-a716-446655440001"), null, 75000m, 1, 250000m, null });

            migrationBuilder.InsertData(
                table: "SupplierInvoices",
                columns: new[] { "SupplierInvoiceId", "BalanceDue", "CreatedAt", "DueDate", "InvoiceDate", "InvoiceNumber", "LinkedPOId", "Status", "SupplierId", "TotalAmount", "UpdatedAt" },
                values: new object[] { new Guid("ee0e8400-e29b-41d4-a716-446655440013"), 0m, new DateTime(2025, 11, 12, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5939), new DateOnly(2025, 12, 12), new DateOnly(2025, 11, 12), "INV001", new Guid("cc0e8400-e29b-41d4-a716-446655440010"), "Paid", new Guid("bb0e8400-e29b-41d4-a716-446655440009"), 7400000m, new DateTime(2025, 11, 14, 2, 28, 22, 965, DateTimeKind.Local).AddTicks(5940) });

            migrationBuilder.InsertData(
                table: "KitchenTickets",
                columns: new[] { "KitchenTicketId", "CreatedAt", "KitchenSectionId", "Notes", "OrderId", "OrderItemId", "PrintedByStaffId", "ReceivedAt", "SentAt", "Status", "TicketNumber", "UpdatedAt" },
                values: new object[] { new Guid("660e8400-e29b-41d4-a716-446655440004"), new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(5674), new Guid("110e8400-e29b-41d4-a716-446655440001"), "Món chính", new Guid("330e8400-e29b-41d4-a716-446655440001"), new Guid("440e8400-e29b-41d4-a716-446655440001"), new Guid("880e8400-e29b-41d4-a716-446655440002"), new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5672), new DateTime(2025, 11, 17, 0, 28, 22, 965, DateTimeKind.Local).AddTicks(5671), 2, "KT001", new DateTime(2025, 11, 17, 1, 28, 22, 965, DateTimeKind.Local).AddTicks(5675) });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPayable_DueDate",
                table: "AccountPayables",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPayable_PartyId",
                table: "AccountPayables",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPayable_ReferenceId",
                table: "AccountPayables",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPayable_Status",
                table: "AccountPayables",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AccountReceivable_DueDate",
                table: "AccountReceivables",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_AccountReceivable_PartyId",
                table: "AccountReceivables",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountReceivable_ReferenceId",
                table: "AccountReceivables",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountReceivable_Status",
                table: "AccountReceivables",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSetting_BranchId",
                table: "ApplicationSettings",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSetting_BranchScoped",
                table: "ApplicationSettings",
                column: "BranchScoped");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSetting_UpdatedAt",
                table: "ApplicationSettings",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_ApprovedByStaffId",
                table: "Attendances",
                column: "ApprovedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_ClockInAt",
                table: "Attendances",
                column: "ClockInAt");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_ShiftId",
                table: "Attendances",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StaffId",
                table: "Attendances",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Status",
                table: "Attendances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Action",
                table: "AuditLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ChangedAt",
                table: "AuditLogs",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ChangedByStaffId",
                table: "AuditLogs",
                column: "ChangedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_EntityId",
                table: "AuditLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_EntityName",
                table: "AuditLogs",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_BenefitType",
                table: "Benefits",
                column: "BenefitType");

            migrationBuilder.CreateIndex(
                name: "IX_Category_DisplayOrder",
                table: "Categories",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PreferredBranchId",
                table: "Customers",
                column: "PreferredBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrder_DriverId",
                table: "DeliveryOrders",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrder_OrderId",
                table: "DeliveryOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_BranchId",
                table: "Devices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_Identifier",
                table: "Devices",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Device_IsActive",
                table: "Devices",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Device_LastSeenAt",
                table: "Devices",
                column: "LastSeenAt");

            migrationBuilder.CreateIndex(
                name: "IX_Device_Name",
                table: "Devices",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Device_Type",
                table: "Devices",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_CategoryId",
                table: "Dishes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_KitchenSectionId",
                table: "Dishes",
                column: "KitchenSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredient_DishId",
                table: "DishIngredients",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredient_InventoryItemId",
                table: "DishIngredients",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_IsActive",
                table: "Drivers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_Name",
                table: "Drivers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_PhoneNumber",
                table: "Drivers",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_BranchId",
                table: "Expenses",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_Category",
                table: "Expenses",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CreatedByStaffId",
                table: "Expenses",
                column: "CreatedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_PaidAt",
                table: "Expenses",
                column: "PaidAt");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_Account",
                table: "FinancialTransactions",
                column: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_BranchId",
                table: "FinancialTransactions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_CreatedByStaffId",
                table: "FinancialTransactions",
                column: "CreatedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_Date",
                table: "FinancialTransactions",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_Type",
                table: "FinancialTransactions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_IsActive",
                table: "InventoryItems",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_Name",
                table: "InventoryItems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_Sku_Supplier",
                table: "InventoryItems",
                columns: new[] { "Sku", "SupplierId" },
                unique: true,
                filter: "[Sku] IS NOT NULL AND [SupplierId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_Name",
                table: "InventoryItems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_StorageLocationId",
                table: "InventoryItems",
                column: "StorageLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SupplierId",
                table: "InventoryItems",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenSections_PrinterDeviceId",
                table: "KitchenSections",
                column: "PrinterDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTicket_KitchenSectionId",
                table: "KitchenTickets",
                column: "KitchenSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTicket_OrderId",
                table: "KitchenTickets",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTickets_OrderItemId",
                table: "KitchenTickets",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_KitchenTickets_PrintedByStaffId",
                table: "KitchenTickets",
                column: "PrintedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyAccount_CustomerId",
                table: "LoyaltyAccounts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltySettings_SettingKey",
                table: "LoyaltySettings",
                column: "SettingKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyTransaction_LoyaltyAccountId",
                table: "LoyaltyTransactions",
                column: "LoyaltyAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyTransaction_OrderId",
                table: "LoyaltyTransactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPriceHistory_ChangedByStaffId",
                table: "MenuPriceHistory",
                column: "ChangedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPriceHistory_DishId",
                table: "MenuPriceHistory",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPriceHistory_EffectivePeriod",
                table: "MenuPriceHistory",
                columns: new[] { "EffectiveFrom", "EffectiveTo" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_DishId",
                table: "OrderItems",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_KitchenSectionId",
                table: "OrderItems",
                column: "KitchenSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuPriceId",
                table: "OrderItems",
                column: "MenuPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ParentOrderItemId",
                table: "OrderItems",
                column: "ParentOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Branch_OrderNumber",
                table: "Orders",
                columns: new[] { "BranchId", "OrderNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_PlacedAt",
                table: "Orders",
                column: "PlacedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId_OrderNumber",
                table: "Orders",
                columns: new[] { "BranchId", "OrderNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClosedByStaffId",
                table: "Orders",
                column: "ClosedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedByStaffId",
                table: "Orders",
                column: "CreatedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PlacedAt",
                table: "Orders",
                column: "PlacedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TableId",
                table: "Orders",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ProcessedByStaffId",
                table: "Payments",
                column: "ProcessedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecord_PaidAt",
                table: "PayrollRecords",
                column: "PaidAt");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecord_PayrollPeriodEnd",
                table: "PayrollRecords",
                column: "PayrollPeriodEnd");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecord_PayrollPeriodStart",
                table: "PayrollRecords",
                column: "PayrollPeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecord_StaffId",
                table: "PayrollRecords",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_ItemId",
                table: "PurchaseOrderLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_PurchaseOrderId",
                table: "PurchaseOrderLines",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_BranchId",
                table: "PurchaseOrders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_OrderedAt",
                table: "PurchaseOrders",
                column: "OrderedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_PONumber",
                table: "PurchaseOrders",
                column: "PONumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_Status",
                table: "PurchaseOrders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CreatedByStaffId",
                table: "PurchaseOrders",
                column: "CreatedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId_Status",
                table: "PurchaseOrders",
                columns: new[] { "SupplierId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_RedemptionRules_RewardType",
                table: "RedemptionRules",
                column: "RewardType");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_OrderId",
                table: "Refunds",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_OriginalPaymentId",
                table: "Refunds",
                column: "OriginalPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_ProcessedByStaffId",
                table: "Refunds",
                column: "ProcessedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsCache_BranchId",
                table: "ReportsCache",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsCache_GeneratedAt",
                table: "ReportsCache",
                column: "GeneratedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsCache_Period",
                table: "ReportsCache",
                column: "Period");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsCache_ReportType",
                table: "ReportsCache",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Branch_StartTime",
                table: "Reservations",
                columns: new[] { "BranchId", "ReserveStartAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservedTableId",
                table: "Reservations",
                column: "ReservedTableId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTable_Branch_Code",
                table: "RestaurantTables",
                columns: new[] { "BranchId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shift_BranchId",
                table: "Shifts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_Name",
                table: "Shifts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_BranchId",
                table: "Staff",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Email",
                table: "Staff",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_EmployeeNumber",
                table: "Staff",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_IsActive",
                table: "Staff",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_RoleId",
                table: "Staff",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffPermission_PermissionId",
                table: "StaffPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffPermission_StaffId",
                table: "StaffPermissions",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffShift_ShiftId",
                table: "StaffShifts",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffShift_StaffId",
                table: "StaffShifts",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffShift_Status",
                table: "StaffShifts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StaffShift_WorkDate",
                table: "StaffShifts",
                column: "WorkDate");

            migrationBuilder.CreateIndex(
                name: "IX_StaffShifts_ApprovedByStaffId",
                table: "StaffShifts",
                column: "ApprovedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffShifts_StaffId_WorkDate",
                table: "StaffShifts",
                columns: new[] { "StaffId", "WorkDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockAlertNotification_CreatedAt",
                table: "StockAlertNotifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlertNotification_ItemId",
                table: "StockAlertNotifications",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlertNotification_Status",
                table: "StockAlertNotifications",
                column: "NotificationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlertThreshold_ItemId",
                table: "StockAlertThresholds",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockCountLine_ItemId",
                table: "StockCountLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCountLine_StockCountId",
                table: "StockCountLines",
                column: "StockCountId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCount_BranchId",
                table: "StockCounts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCount_CountDate",
                table: "StockCounts",
                column: "CountDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockCount_PerformedByStaffId",
                table: "StockCounts",
                column: "PerformedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCount_Status",
                table: "StockCounts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransaction_CreatedAt",
                table: "StockTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransaction_ItemId",
                table: "StockTransactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransaction_TransactionType",
                table: "StockTransactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_CreatedByStaffId",
                table: "StockTransactions",
                column: "CreatedByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_ExpiryDate",
                table: "StorageItems",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_InventoryItemId",
                table: "StorageItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_StorageLocationId",
                table: "StorageItems",
                column: "StorageLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLocation_BranchId",
                table: "StorageLocations",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLocation_IsActive",
                table: "StorageLocations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoice_DueDate",
                table: "SupplierInvoices",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoice_InvoiceDate",
                table: "SupplierInvoices",
                column: "InvoiceDate");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoice_InvoiceNumber",
                table: "SupplierInvoices",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoice_Status",
                table: "SupplierInvoices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoice_SupplierId",
                table: "SupplierInvoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoices_LinkedPOId",
                table: "SupplierInvoices",
                column: "LinkedPOId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Email",
                table: "Suppliers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_IsActive",
                table: "Suppliers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Name",
                table: "Suppliers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_SupplierCode",
                table: "Suppliers",
                column: "SupplierCode",
                unique: true,
                filter: "[SupplierCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TierBenefit_Unique",
                table: "TierBenefits",
                columns: new[] { "TierConfigId", "BenefitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TierBenefits_BenefitId",
                table: "TierBenefits",
                column: "BenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_TierConfigs_Tier",
                table: "TierConfigs",
                column: "Tier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_Code",
                table: "Vouchers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_ValidityPeriod",
                table: "Vouchers",
                columns: new[] { "ValidFrom", "ValidTo" });

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_Code",
                table: "Vouchers",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPayables");

            migrationBuilder.DropTable(
                name: "AccountReceivables");

            migrationBuilder.DropTable(
                name: "ApplicationSettings");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "DeliveryOrders");

            migrationBuilder.DropTable(
                name: "DishIngredients");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "FinancialTransactions");

            migrationBuilder.DropTable(
                name: "KitchenTickets");

            migrationBuilder.DropTable(
                name: "LoyaltySettings");

            migrationBuilder.DropTable(
                name: "LoyaltyTransactions");

            migrationBuilder.DropTable(
                name: "PayrollRecords");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLines");

            migrationBuilder.DropTable(
                name: "RedemptionRules");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropTable(
                name: "ReportsCache");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "StaffPermissions");

            migrationBuilder.DropTable(
                name: "StaffShifts");

            migrationBuilder.DropTable(
                name: "StockAlertNotifications");

            migrationBuilder.DropTable(
                name: "StockAlertThresholds");

            migrationBuilder.DropTable(
                name: "StockCountLines");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "StorageItems");

            migrationBuilder.DropTable(
                name: "SupplierInvoices");

            migrationBuilder.DropTable(
                name: "TierBenefits");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "LoyaltyAccounts");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "StockCounts");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.DropTable(
                name: "TierConfigs");

            migrationBuilder.DropTable(
                name: "MenuPriceHistory");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "StorageLocations");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "RestaurantTables");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "KitchenSections");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
