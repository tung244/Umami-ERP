using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiKhoHang.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierIdToStockTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_Sku",
                table: "InventoryItems");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440008"),
                columns: new[] { "ClockInAt", "ClockOutAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 17, 6, 13, 409, DateTimeKind.Local).AddTicks(3503), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3504), new DateTime(2025, 11, 16, 17, 6, 13, 409, DateTimeKind.Local).AddTicks(3506), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3507) });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2266));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2281));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("990e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2916));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("990e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2919));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: new Guid("ee0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3140));

            migrationBuilder.UpdateData(
                table: "DishIngredients",
                keyColumn: "DishIngredientId",
                keyValue: new Guid("dd0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3108));

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "DishId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2949));

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "DishId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2953));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ItemId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3076));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ItemId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3081));

            migrationBuilder.UpdateData(
                table: "KitchenSections",
                keyColumn: "KitchenSectionId",
                keyValue: new Guid("110e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3220));

            migrationBuilder.UpdateData(
                table: "KitchenTickets",
                keyColumn: "KitchenTicketId",
                keyValue: new Guid("660e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "CreatedAt", "ReceivedAt", "SentAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3409), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3407), new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3407), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3410) });

            migrationBuilder.UpdateData(
                table: "LoyaltyAccounts",
                keyColumn: "LoyaltyAccountId",
                keyValue: new Guid("770e8400-e29b-41d4-a716-446655440005"),
                columns: new[] { "CreatedAt", "TierEffectiveFrom" },
                values: new object[] { new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3434), new DateTime(2025, 5, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3433) });

            migrationBuilder.UpdateData(
                table: "LoyaltyTransactions",
                keyColumn: "LoyaltyTransactionId",
                keyValue: new Guid("880e8400-e29b-41d4-a716-446655440006"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3455));

            migrationBuilder.UpdateData(
                table: "MenuPriceHistory",
                keyColumn: "MenuPriceId",
                keyValue: new Guid("bb0e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "EffectiveFrom" },
                values: new object[] { new DateTime(2025, 5, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2981), new DateTime(2025, 5, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2977) });

            migrationBuilder.UpdateData(
                table: "MenuPriceHistory",
                keyColumn: "MenuPriceId",
                keyValue: new Guid("bb0e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "EffectiveFrom" },
                values: new object[] { new DateTime(2025, 5, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2986), new DateTime(2025, 5, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2984) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: new Guid("440e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "RequestedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3349), new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3347), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3350) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: new Guid("440e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "RequestedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3356), new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3355), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3357) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("330e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "PlacedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3311), new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3292), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3312) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "PlacedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3816), new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3811) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "PlacedAt", "ServedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3824), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3820), new DateTime(2025, 11, 17, 0, 36, 13, 409, DateTimeKind.Local).AddTicks(3821), new DateTime(2025, 11, 17, 0, 36, 13, 409, DateTimeKind.Local).AddTicks(3825) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "CreatedAt", "PlacedAt", "ServedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 20, 6, 13, 409, DateTimeKind.Local).AddTicks(3836), new DateTime(2025, 11, 16, 20, 6, 13, 409, DateTimeKind.Local).AddTicks(3830), new DateTime(2025, 11, 16, 21, 6, 13, 409, DateTimeKind.Local).AddTicks(3831), new DateTime(2025, 11, 16, 21, 6, 13, 409, DateTimeKind.Local).AddTicks(3837) });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "CreatedAt", "PaymentDate" },
                values: new object[] { new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3383), new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3379) });

            migrationBuilder.UpdateData(
                table: "PurchaseOrderLines",
                keyColumn: "POLineId",
                keyValue: new Guid("dd0e8400-e29b-41d4-a716-446655440011"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 10, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3562), new DateTime(2025, 11, 12, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3563) });

            migrationBuilder.UpdateData(
                table: "PurchaseOrderLines",
                keyColumn: "POLineId",
                keyValue: new Guid("dd0e8400-e29b-41d4-a716-446655440012"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 10, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3567), new DateTime(2025, 11, 12, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3567) });

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "PurchaseOrderId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440010"),
                columns: new[] { "CreatedAt", "ExpectedDeliveryDate", "OrderedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 10, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3534), new DateTime(2025, 11, 12, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3531), new DateTime(2025, 11, 10, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3528), new DateTime(2025, 11, 12, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3535) });

            migrationBuilder.UpdateData(
                table: "RestaurantTables",
                keyColumn: "TableId",
                keyValue: new Guid("ff0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3193));

            migrationBuilder.UpdateData(
                table: "RestaurantTables",
                keyColumn: "TableId",
                keyValue: new Guid("ff0e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3196));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("d9c15292-0135-4cfa-9ee6-fcfaa8fc4e8f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440010"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("5cd6761e-f988-40c1-afcc-8aaa08d478b0"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440011"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("48c2773c-592b-4dbe-815f-8b22a0904fed"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440012"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("f00e153f-c3af-4146-9a9e-350b68e38f39"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440013"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("9240db0d-80b4-4b72-be7e-d8b42c997c93"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("72158f21-f508-41c9-b57e-533c3eaf0d34"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440021"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("f6f8fe82-52f6-4323-b6f4-00ebd2240e09"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440022"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("711aa1f8-d25a-49a0-acdf-453d1557ac49"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440023"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("90428055-2541-4818-a18b-4534ec5f1e12"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440030"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("9ef64919-c744-4504-8ddf-087550218d52"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440031"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("32e6e512-9a2e-41ba-867c-860866b1b2f8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440040"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("541db300-251e-450d-b2ed-744bd45f63b7"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440041"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("479af386-0eac-4c1a-9712-5fa8dc5853da"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440042"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("8f7ea423-62a7-45a9-a075-8365f8d25055"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440050"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e9db7cf6-e352-45c5-af72-70bff96e005c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440051"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("8ad5dda7-b0f1-4bf4-882f-926d19212f15"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440052"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("cddcfe2c-2090-4f4d-9569-dc457902468c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440053"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("bb598334-b129-4847-a0e6-b024122aaca6"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440060"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e0dfe4d6-1c73-4330-b140-a42954c6fa03"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440061"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("c817f731-80cd-4a0a-8cc8-998e20f76cb4"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440062"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("8a48f8d6-8227-4346-b7a5-8284e7606cce"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440063"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("51733303-8b40-46e0-92c1-7134d73c9e8a"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440070"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("ec5ebe64-b2ca-42ad-97a1-046306c76c06"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440071"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("1584594f-8068-4265-8a76-fae856b54848"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440080"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("13c4b08c-36b1-43fa-a435-05ed05dfa894"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440090"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("ff3686ef-383a-4f4c-bd76-96c53b56d54a"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440091"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("dcb48775-759b-412a-aea5-67e727616b68"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440092"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("f48287c8-e8ea-40a4-821d-f24a4993dc8f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440100"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("9dd28396-e1fc-4449-85ff-c8cd35b20b70"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440101"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("81d75d9a-30de-4152-9218-d672814163e8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("3cfb7dec-275f-41c8-98c2-72eeb651dd0b"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440111"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("3bc9bc91-149f-481e-b72d-a42d6b44ff55"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440112"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("3dfc1916-c2a5-4967-8f63-93938cedf6a8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440113"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e0a65ee1-0d10-4a6d-8f30-4f933e84dc98"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440120"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("30d0f3fa-4dc3-4977-b913-2a54dc2b8b52"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440121"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e41dec54-7b51-4673-8956-7692a9c5255e"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e0d9821a-9ab2-4f6d-a97d-c7b86f0daa32"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440131"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("d5d68556-54ee-4875-9966-44e0c47638e6"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("5bb57e75-6fd7-475a-b380-5dbe555ee7b3"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440140"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("13ae6d1d-2d0f-4de9-b0ff-0171e2d49d59"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("308529ff-1c23-43a7-9eca-2bba876ff9c1"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440142"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("a139bbcd-18a5-4287-87e7-47df138a26ea"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440143"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("ab4ce7e6-4cd3-437b-ae84-a3562a8a193c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440150"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("be53c935-6648-4d86-aa6d-75b77394d2cf"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440160"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e4518d52-c0cb-4495-b84e-40d6e0616e35"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440161"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("432d3ed7-bacc-4d80-b4b8-215c756c5ee8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440162"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e8e1bcb1-20a6-4bc5-93b0-402ce2ad1112"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440163"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("412d3e7e-baf0-429c-a150-3a2e33386ceb"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440170"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("f2890906-a00a-4720-9f60-cd155a70cebe"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440171"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("449f4e65-c5b5-4bf4-8c1d-f03e7ede228c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440180"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("bad32105-e160-45ea-ab38-1e78a724f555"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440181"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("8ee0e5c9-749d-4040-bd7b-0c61fa55c1c7"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440182"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("fa335ff8-4ca8-4588-8b69-0d34464e09fc"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440190"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("44ee26b6-8156-4f92-a7c2-f92d8dce98bd"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440191"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("83f9078b-2952-48b4-ba80-d17819e92b46"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440192"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("cf337590-e8d5-4b09-96a9-b48f05724b1c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("1ff0c0a4-d466-4f75-9cbe-62c239773398"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440010"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("21ba643b-8e2b-4c91-bb65-28d058d254d0"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440011"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("6badd4ce-b473-4c18-bc28-30bb85b43c46"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440012"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("74c102fb-3231-49f6-93e4-aa3f345043e1"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("ff8c76f9-6b49-4287-a0ff-c7379fdb05c7"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440021"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("8a6a367b-ceb4-4826-ae1c-ad67d5c1d371"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440022"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("f14869e0-a1fc-4555-96b9-6f2a83c7ba61"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440030"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("05417d5f-af7a-41ee-9469-d165b7179310"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440031"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("b6b8aa96-0084-440a-9564-f86a900ea506"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440040"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("6ec47392-5402-4cd7-813e-d1d65141e057"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440041"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("95988b90-7014-48db-a58b-472ace064eb6"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440042"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("fb692a30-ecae-43ff-ba42-36422dcdc632"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440050"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("cacb8e36-941a-4741-9ca9-45185dc05d5c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440051"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("9a328d64-b252-4b61-9347-d560ad93f131"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440052"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("6420b822-eb9c-41b1-9592-760d00139e8a"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440053"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("ff56fc48-823b-4aa8-a672-723970058b68"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440060"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("ffa8006d-5aff-479e-828d-4c9d03bc2e80"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440061"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("5eb271be-06d5-4601-a514-95dc2291e479"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440062"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("8d5ef7f3-a9ab-4e57-a0f0-83c4fe7a95c4"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440070"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("cf2aca67-a9c3-44e7-b4b4-db72a24f625c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440071"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("347d7905-edd5-4196-a4da-93d03a3db196"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440090"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("7ce52e38-76c5-4ae5-b7e4-8a93a38e616f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440091"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("f3d95abd-3cdc-4bee-bdb2-198f30a70e7e"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440092"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("687a031e-8509-4fe0-adce-f0c6ca8d350b"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440100"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("32f0e1b3-f86c-4bf5-8e1d-ee26041e26fb"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440101"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("4074e38c-84bd-4d78-8f2b-30b4e99cafdc"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("d4d94ddf-301b-4a39-9838-452dd27ccc6c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440111"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("817beca5-e270-481a-904e-86da7ab91139"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440112"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("663cfb20-1251-410f-9f87-c455a1ecd3ac"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440120"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("0ecadd2e-5aa8-444f-9331-4c797ff12394"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440121"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("f42452d2-52ef-43f0-9d49-5a26926bad05"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("3a146fd7-4d37-4824-8aa5-78187a968be8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440131"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("e6a7c9bd-fa72-42f7-96ec-c38386008996"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("761d5cd7-1953-4509-9138-70222dcf666e"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("31983a50-f723-474f-8ff8-c18bbca41194"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440150"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("cf68c3b3-962f-4e14-a30f-61514e0b4596"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440160"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("725be31f-dd09-4aa7-94d5-f96cf25e47a3"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440161"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("8e837cc4-352e-435a-8fe1-f409cfffbe0f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440162"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("59d946c8-eea4-4d08-aafe-c085d1522abb"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440190"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("3f9104e4-6d2d-4308-b8dc-3ad0b9d83310"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440191"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("63572df4-3cca-4e27-92cd-43b71106344a"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("7873d998-4c57-40d0-bd44-be540c8070d5"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("11ee5ee1-2400-422c-8812-c8822c5daf81"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("369d63ad-e54d-4ba1-8e1e-167425719550"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("0a08a543-0a5b-4b2d-b588-82513f6357a4"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("ba7ca30b-7103-46ee-81ac-21841bf8e92c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440140"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("bf4be2e4-87b2-46f1-acd5-eed20f4ead91"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("22b08d90-5030-4478-9188-1d89ebc653b5"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("660e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2573));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("660e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2576));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("660e8400-e29b-41d4-a716-446655440003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2578));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: new Guid("220e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3244));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: new Guid("220e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3247));

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: new Guid("880e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "HireDate" },
                values: new object[] { new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2882), new DateTime(2023, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2867) });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: new Guid("880e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "HireDate" },
                values: new object[] { new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2888), new DateTime(2024, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(2886) });

            migrationBuilder.UpdateData(
                table: "StaffShifts",
                keyColumn: "StaffShiftId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "ApprovedAt", "CreatedAt", "WorkDate" },
                values: new object[] { new DateTime(2025, 11, 17, 0, 36, 13, 409, DateTimeKind.Local).AddTicks(3769), new DateTime(2025, 11, 16, 23, 6, 13, 409, DateTimeKind.Local).AddTicks(3772), new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StaffShifts",
                keyColumn: "StaffShiftId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "WorkDate" },
                values: new object[] { new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3777), new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StaffShifts",
                keyColumn: "StaffShiftId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "CreatedAt", "WorkDate" },
                values: new object[] { new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3781), new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StockTransactions",
                keyColumn: "StockTransactionId",
                keyValue: new Guid("990e8400-e29b-41d4-a716-446655440007"),
                columns: new[] { "CreatedAt", "SupplierId" },
                values: new object[] { new DateTime(2025, 11, 17, 0, 6, 13, 409, DateTimeKind.Local).AddTicks(3478), null });

            migrationBuilder.UpdateData(
                table: "StorageLocations",
                keyColumn: "StorageLocationId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3046));

            migrationBuilder.UpdateData(
                table: "StorageLocations",
                keyColumn: "StorageLocationId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3050));

            migrationBuilder.UpdateData(
                table: "SupplierInvoices",
                keyColumn: "SupplierInvoiceId",
                keyValue: new Guid("ee0e8400-e29b-41d4-a716-446655440013"),
                columns: new[] { "CreatedAt", "DueDate", "InvoiceDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 12, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3706), new DateOnly(2025, 12, 12), new DateOnly(2025, 11, 12), new DateTime(2025, 11, 14, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3707) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: new Guid("bb0e8400-e29b-41d4-a716-446655440009"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3019));

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("ff0e8400-e29b-41d4-a716-446655440014"),
                columns: new[] { "CreatedAt", "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3737), new DateTime(2025, 11, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3735), new DateTime(2026, 5, 17, 1, 6, 13, 409, DateTimeKind.Local).AddTicks(3736) });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_Sku_Supplier",
                table: "InventoryItems",
                columns: new[] { "Sku", "SupplierId" },
                unique: true,
                filter: "[Sku] IS NOT NULL AND [SupplierId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_Sku_Supplier",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "StockTransactions");

            migrationBuilder.UpdateData(
                table: "Attendances",
                keyColumn: "AttendanceId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440008"),
                columns: new[] { "ClockInAt", "ClockOutAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 14, 21, 17, 479, DateTimeKind.Local).AddTicks(7233), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7234), new DateTime(2025, 11, 16, 14, 21, 17, 479, DateTimeKind.Local).AddTicks(7238), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7239) });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(5808));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(5824));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("990e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6407));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("990e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6410));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: new Guid("ee0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6837));

            migrationBuilder.UpdateData(
                table: "DishIngredients",
                keyColumn: "DishIngredientId",
                keyValue: new Guid("dd0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6795));

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "DishId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6604));

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "DishId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6610));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ItemId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6754));

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "ItemId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6761));

            migrationBuilder.UpdateData(
                table: "KitchenSections",
                keyColumn: "KitchenSectionId",
                keyValue: new Guid("110e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6901));

            migrationBuilder.UpdateData(
                table: "KitchenTickets",
                keyColumn: "KitchenTicketId",
                keyValue: new Guid("660e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "CreatedAt", "ReceivedAt", "SentAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7119), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7117), new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7116), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7120) });

            migrationBuilder.UpdateData(
                table: "LoyaltyAccounts",
                keyColumn: "LoyaltyAccountId",
                keyValue: new Guid("770e8400-e29b-41d4-a716-446655440005"),
                columns: new[] { "CreatedAt", "TierEffectiveFrom" },
                values: new object[] { new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7149), new DateTime(2025, 5, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7148) });

            migrationBuilder.UpdateData(
                table: "LoyaltyTransactions",
                keyColumn: "LoyaltyTransactionId",
                keyValue: new Guid("880e8400-e29b-41d4-a716-446655440006"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7173));

            migrationBuilder.UpdateData(
                table: "MenuPriceHistory",
                keyColumn: "MenuPriceId",
                keyValue: new Guid("bb0e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "EffectiveFrom" },
                values: new object[] { new DateTime(2025, 5, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6639), new DateTime(2025, 5, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6635) });

            migrationBuilder.UpdateData(
                table: "MenuPriceHistory",
                keyColumn: "MenuPriceId",
                keyValue: new Guid("bb0e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "EffectiveFrom" },
                values: new object[] { new DateTime(2025, 5, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6644), new DateTime(2025, 5, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6642) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: new Guid("440e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "RequestedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7043), new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7041), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7044) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: new Guid("440e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "RequestedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7050), new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7049), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7050) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("330e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "PlacedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(6975), new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(6968), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(6977) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "PlacedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7474), new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7469) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "PlacedAt", "ServedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7514), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7510), new DateTime(2025, 11, 16, 21, 51, 17, 479, DateTimeKind.Local).AddTicks(7510), new DateTime(2025, 11, 16, 21, 51, 17, 479, DateTimeKind.Local).AddTicks(7515) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "CreatedAt", "PlacedAt", "ServedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 16, 17, 21, 17, 479, DateTimeKind.Local).AddTicks(7524), new DateTime(2025, 11, 16, 17, 21, 17, 479, DateTimeKind.Local).AddTicks(7520), new DateTime(2025, 11, 16, 18, 21, 17, 479, DateTimeKind.Local).AddTicks(7520), new DateTime(2025, 11, 16, 18, 21, 17, 479, DateTimeKind.Local).AddTicks(7525) });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "CreatedAt", "PaymentDate" },
                values: new object[] { new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7086), new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7084) });

            migrationBuilder.UpdateData(
                table: "PurchaseOrderLines",
                keyColumn: "POLineId",
                keyValue: new Guid("dd0e8400-e29b-41d4-a716-446655440011"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 9, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7295), new DateTime(2025, 11, 11, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7295) });

            migrationBuilder.UpdateData(
                table: "PurchaseOrderLines",
                keyColumn: "POLineId",
                keyValue: new Guid("dd0e8400-e29b-41d4-a716-446655440012"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 9, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7299), new DateTime(2025, 11, 11, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7300) });

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "PurchaseOrderId",
                keyValue: new Guid("cc0e8400-e29b-41d4-a716-446655440010"),
                columns: new[] { "CreatedAt", "ExpectedDeliveryDate", "OrderedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 9, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7266), new DateTime(2025, 11, 11, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7264), new DateTime(2025, 11, 9, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7262), new DateTime(2025, 11, 11, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7267) });

            migrationBuilder.UpdateData(
                table: "RestaurantTables",
                keyColumn: "TableId",
                keyValue: new Guid("ff0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6864));

            migrationBuilder.UpdateData(
                table: "RestaurantTables",
                keyColumn: "TableId",
                keyValue: new Guid("ff0e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6867));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("b9bdce4c-65b4-44fa-ab05-bb3da1259175"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440010"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("43e21d90-ff41-48bc-a12e-ef7a1032db58"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440011"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e22d5807-22c4-4f12-95f6-16e42f95107f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440012"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("23af9752-db36-4875-bd8e-465a7fbf1fab"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440013"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("a2465e8a-13c9-4421-bc1f-8676d22fbbd8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("23a1499a-6be2-41ae-b482-ccc178253e02"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440021"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("1f97eb83-f9f5-4687-9f0c-5b9d1b1b24e7"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440022"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("964fe2e9-b51f-471a-be94-710b8e7ce6fc"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440023"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("df70223a-223f-4266-adfa-cf47a0fb52e2"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440030"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("13fe81c1-9609-4d79-8bc5-b105f595b64b"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440031"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("6b372734-c73b-4074-8cc9-5b32cd9de803"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440040"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("39100daa-59f9-48bc-a4cf-3ab951b69cbd"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440041"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("fff9cfd4-fa1d-4eda-b315-804bcdf224c0"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440042"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("be9381ae-d072-4abf-ad7b-952f420e171f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440050"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("3da02573-4994-4b84-83bc-0b0580b538d6"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440051"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("17d4679f-6482-4699-a58a-6ff4703e6f91"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440052"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("db43b65e-82fe-4839-a23f-b8006ad97c32"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440053"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("7807bcd4-2974-4853-b566-99fc04adac39"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440060"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("c3c0c768-4b8a-4677-8beb-a999efcf547d"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440061"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("70c9357b-a554-4c3f-883a-1008b7cfdca3"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440062"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("cf906d21-0376-4c42-ac24-a7bdfcedbdc5"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440063"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("f379a270-68b0-4ea1-8fcf-a7e93cb97ce1"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440070"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("24b783f7-6327-49db-ad1f-d66e63e3ee90"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440071"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e37e8112-b10c-4f5d-b527-cb6440f299c8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440080"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("029298e3-f790-4c49-937c-2efe6794f89c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440090"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("49548e0f-6dbf-4278-a458-3cad25c88c60"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440091"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("86679178-26e3-4d07-9703-c3bbbfdf3cb3"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440092"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("d8000f88-66ea-4b35-88da-af46f421e7ab"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440100"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("101add19-9b06-4480-bbe0-fb65561e25b8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440101"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("e81d03ac-d444-42ed-bf27-901ac7f5a4b1"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("bc03ae40-1ea4-43bd-ad48-2398837173f6"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440111"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("4dc23d2d-bf04-4ac3-ab13-fa9035441347"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440112"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("4969a86c-8d8a-4463-8ae6-f8268bac449b"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440113"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("9f1dadb3-f206-4750-ab88-18f62d7519b0"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440120"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("a05c9e0f-cc60-4454-9a52-126d0eed6da4"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440121"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("3dad4372-3cb6-4788-8f03-155be527aa83"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("f0e45747-4c53-462e-8272-92b35495384d"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440131"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("186ccb6b-9ccd-4dfe-8ebf-c26d2b746890"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("b1bb9bb6-edab-4a7a-bae0-65711800e92d"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440140"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("a34776aa-daae-4d65-9686-853916665c12"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("72aaa859-fe15-40cd-b3db-33ec2edbeafc"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440142"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("583635a6-9553-4610-982a-2356b1912f6d"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440143"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("c2c50874-6492-4b1b-80fb-69e6c12262d5"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440150"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("d7316af2-7363-47c9-a464-1c71f68cb6b5"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440160"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("b8295e42-01f6-4977-81d6-66ccfa662829"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440161"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("c060b07a-ece4-44bb-9cd9-b9cbc4266d62"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440162"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("729ac8a1-2b71-4f88-89eb-812b4fae5a22"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440163"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("1e92f928-a1f2-46b2-95ae-fd6dfd4832db"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440170"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("a3d05d4e-9903-454c-9606-563419d8d619"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440171"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("0cd291c4-5056-4231-b0ed-53657da14d12"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440180"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("32081fc0-0663-46c9-8a6c-acc072667f2b"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440181"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("cfccc809-3a49-480e-b477-ee8453345e43"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440182"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("a0f05004-2345-4927-bef1-d26849f26b28"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440190"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("180c8362-2442-4b99-af6c-300874b6d8ab"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440191"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("725ff06d-54e4-4c86-b872-ba50bd22995f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440192"), new Guid("660e8400-e29b-41d4-a716-446655440001") },
                column: "RolePermissionId",
                value: new Guid("a976e5a8-fe32-4a3c-8c44-8b1fb4046362"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("634dfe04-baca-4fd4-b716-da04bcb77e3c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440010"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("31ea83b8-23f6-4293-8940-ed5967c01338"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440011"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("0b101e4d-cd69-47f1-9e58-aa171dc39e1c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440012"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("a1077eaa-4b23-478f-9d2a-dfccbe183c39"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("c193757e-ecd0-4c7c-8303-73cfe81e9783"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440021"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("0c8bc019-beb0-40ff-ae44-75a5edbf48cf"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440022"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("2c5ecacb-55aa-4684-8c2a-d1d16be9b30d"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440030"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("2c5feb0b-c9e9-46c5-93b6-7b9fc8265017"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440031"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("9a58e7c8-dde1-482b-afbc-fc3dffbf364c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440040"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("13773bbb-e989-4c5e-a69c-cc18d451a131"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440041"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("f59e0900-8be0-4740-9b1d-0b301ef2afee"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440042"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("a730e806-3a10-4453-a360-f292420b1939"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440050"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("e0237a25-7d9e-4dd7-bdc8-caab15ff3a1f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440051"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("5d4791a1-53ed-434d-8c0a-8b1fdc0cd58f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440052"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("9572bc98-0482-48e2-a996-565752e5f353"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440053"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("020bda8d-ee3c-40fe-befd-09df5ed5d856"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440060"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("0a450002-f945-4c4e-9b08-568cd75bb5e5"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440061"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("ed8c6559-0591-44ed-b7d1-78a0fd92f657"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440062"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("d9ccdaa8-6364-4823-84db-20c4860c633c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440070"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("c29d3618-f51e-43dd-b0f2-9403307137ea"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440071"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("c8bb6236-9064-4a6f-a8ef-cc4005b4c7b9"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440090"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("ebd42aac-1c35-4096-8d4a-16677fb70abd"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440091"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("a91750ec-84ff-4e69-9513-8a5694962ea0"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440092"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("d371376d-f9a8-4dab-a708-0d13b5eb3dd4"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440100"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("644ba699-ac5a-4033-85b9-4ecf1483b8a3"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440101"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("7f2288f7-d9c1-4049-aea3-dbf29d974a97"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("09783382-3afd-40d5-92c9-3cfb9fb2519c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440111"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("124fb150-c3f2-4e61-91ef-c712bc557bbf"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440112"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("671a08f4-a832-47dd-9ca7-3402217c3e9a"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440120"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("fc11baf6-9364-4223-b86d-906d61010ced"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440121"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("4df4ca3f-447d-4974-9074-89aff2853b7f"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("d0807143-b54c-4c23-93d4-5b0e2b90909c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440131"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("e64bddfc-1525-4b46-b1a0-6390f6cb75fe"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("2f85ae9e-988a-4970-83e3-ee9d8db3239b"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("249f7045-4b77-402c-8aac-fad9bbabc1c7"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440150"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("dc8d369d-2957-4939-bdf4-a5fbce313fd8"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440160"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("4c1102ec-eb22-41a4-b421-cac2e0989579"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440161"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("f8f6d86f-eed6-43a0-9491-70f48523f0ab"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440162"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("d0556afd-eeeb-42fb-87ac-af5f09b5e76c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440190"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("e185b93b-d3af-478f-912a-b858f7b3692e"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440191"), new Guid("660e8400-e29b-41d4-a716-446655440002") },
                column: "RolePermissionId",
                value: new Guid("d7c4ed6a-e1fd-48d5-8c48-fc9e190eb13b"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440001"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("251af220-4093-4d38-a220-13cc78e40d0c"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440020"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("e7a275e6-6d69-42f3-8433-f326ebed9edc"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440110"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("668cc5cc-18f5-4ccb-bf0a-974a0df652dc"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440130"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("2ce19355-9994-41d2-b3b6-ecafd2619474"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440132"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("220d7db0-b7b3-4819-ad64-19f6716168bd"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440140"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("c2f6d2c2-0cdf-4c50-852e-49a7dc54574d"));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("770e8400-e29b-41d4-a716-446655440141"), new Guid("660e8400-e29b-41d4-a716-446655440003") },
                column: "RolePermissionId",
                value: new Guid("19d7f12c-e951-4198-9670-152ac73a9089"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("660e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6112));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("660e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6117));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("660e8400-e29b-41d4-a716-446655440003"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6120));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: new Guid("220e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6928));

            migrationBuilder.UpdateData(
                table: "Shifts",
                keyColumn: "ShiftId",
                keyValue: new Guid("220e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6933));

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: new Guid("880e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "CreatedAt", "HireDate" },
                values: new object[] { new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6361), new DateTime(2023, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6346) });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: new Guid("880e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "HireDate" },
                values: new object[] { new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6368), new DateTime(2024, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6366) });

            migrationBuilder.UpdateData(
                table: "StaffShifts",
                keyColumn: "StaffShiftId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "ApprovedAt", "CreatedAt", "WorkDate" },
                values: new object[] { new DateTime(2025, 11, 16, 21, 51, 17, 479, DateTimeKind.Local).AddTicks(7395), new DateTime(2025, 11, 16, 20, 21, 17, 479, DateTimeKind.Local).AddTicks(7397), new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StaffShifts",
                keyColumn: "StaffShiftId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "CreatedAt", "WorkDate" },
                values: new object[] { new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7402), new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StaffShifts",
                keyColumn: "StaffShiftId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "CreatedAt", "WorkDate" },
                values: new object[] { new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7439), new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StockTransactions",
                keyColumn: "StockTransactionId",
                keyValue: new Guid("990e8400-e29b-41d4-a716-446655440007"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 21, 21, 17, 479, DateTimeKind.Local).AddTicks(7204));

            migrationBuilder.UpdateData(
                table: "StorageLocations",
                keyColumn: "StorageLocationId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440001"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6717));

            migrationBuilder.UpdateData(
                table: "StorageLocations",
                keyColumn: "StorageLocationId",
                keyValue: new Guid("aa0e8400-e29b-41d4-a716-446655440002"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6722));

            migrationBuilder.UpdateData(
                table: "SupplierInvoices",
                keyColumn: "SupplierInvoiceId",
                keyValue: new Guid("ee0e8400-e29b-41d4-a716-446655440013"),
                columns: new[] { "CreatedAt", "DueDate", "InvoiceDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 11, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7331), new DateOnly(2025, 12, 11), new DateOnly(2025, 11, 11), new DateTime(2025, 11, 13, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7332) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: new Guid("bb0e8400-e29b-41d4-a716-446655440009"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(6685));

            migrationBuilder.UpdateData(
                table: "Vouchers",
                keyColumn: "VoucherId",
                keyValue: new Guid("ff0e8400-e29b-41d4-a716-446655440014"),
                columns: new[] { "CreatedAt", "ValidFrom", "ValidTo" },
                values: new object[] { new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7368), new DateTime(2025, 11, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7365), new DateTime(2026, 5, 16, 22, 21, 17, 479, DateTimeKind.Local).AddTicks(7366) });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_Sku",
                table: "InventoryItems",
                column: "Sku",
                unique: true,
                filter: "[Sku] IS NOT NULL");
        }
    }
}
