# 📋 Entity Specifications - QuanLiKhoHang Database Schema

## 🚀 Entity Framework Code First Setup

### Tạo Database từ Specifications này:

```bash
# 1. Cài đặt EF Core CLI tools (nếu chưa có)
dotnet tool install --global dotnet-ef

# 2. Tạo migration đầu tiên
dotnet ef migrations add InitialCreate

# 3. Áp dụng migration lên database
dotnet ef database update
```

### Quy tắc ánh xạ từ Spec sang C# Entity:

| Spec Type | C# Type | EF Attribute |
|-----------|---------|--------------|
| UUID | Guid | [Key] |
| BIGINT | long | - |
| INT | int | - |
| VARCHAR(n) | string | [MaxLength(n)] |
| TEXT | string | - |
| DECIMAL | decimal | [Column(TypeName = "decimal(18,2)")] |
| DATETIME | DateTime | - |
| BOOLEAN | bool | - |
| ENUM | enum | - |

### Ràng buộc Database:
- **PK**: Primary Key - tự động tạo clustered index
- **FK**: Foreign Key - tự động tạo index
- **NOT NULL**: Bắt buộc phải có giá trị
- **NULL**: Có thể null
- **UNIQUE**: Giá trị duy nhất
- **DEFAULT**: Giá trị mặc định

---

Order
●	OrderId (UUID / Guid) — PK, NOT NULL. Unique order identifier. [Key]

●	OrderNumber (VARCHAR(50)) — NOT NULL, unique per branch, human-friendly (e.g., BR01-2025-000123). [Index("IX_Order_Branch_OrderNumber", IsUnique = true)]

●	BranchId (UUID / Guid) — FK → Branch(BranchId), NOT NULL. [ForeignKey("Branch")], [Index]

●	TableId (UUID / Guid) — FK → RestaurantTable(TableId), NULL if take-away/delivery. [ForeignKey("Table")]

●	CustomerId (UUID / Guid) — FK → Customer(CustomerId), NULL for walk-in. [ForeignKey("Customer")]

●	CreatedByStaffId (UUID / Guid) — FK → Staff(StaffId), NOT NULL. [ForeignKey("CreatedByStaff")]

●	OrderType (ENUM / OrderType) — NOT NULL. Values: DineIn, TakeAway, Delivery, Counter.

●	OrderSource (ENUM / OrderSource) — NOT NULL. Values: POS, MobileApp, Web, Phone, ThirdParty.

●	PlacedAt (DATETIME / DateTime) — NOT NULL. Timestamp when order created.

●	ServedAt (DATETIME / DateTime) — NULL. Timestamp when order marked served/complete.

●	Status (ENUM / OrderStatus) — NOT NULL. E.g., Open, Confirmed, InPreparation, Ready, Served, Paid, Cancelled, Refunded.

●	SubTotal (DECIMAL(18,2) / decimal) — NOT NULL. Sum of items before discounts/tax. [Column(TypeName = "decimal(18,2)")]

●	DiscountAmount (DECIMAL(18,2) / decimal) — NOT NULL, default 0. [Column(TypeName = "decimal(18,2)")]

●	TaxAmount (DECIMAL(18,2) / decimal) — NOT NULL, default 0. [Column(TypeName = "decimal(18,2)")]

●	ServiceChargeAmount (DECIMAL(18,2) / decimal) — NOT NULL, default 0. [Column(TypeName = "decimal(18,2)")]

●	RoundingAdjustment (DECIMAL(18,2) / decimal) — NOT NULL. [Column(TypeName = "decimal(18,2)")]

●	TotalAmount (DECIMAL(18,2) / decimal) — NOT NULL. [Column(TypeName = "decimal(18,2)")]

●	PaidAmount (DECIMAL(18,2) / decimal) — NOT NULL, default 0. [Column(TypeName = "decimal(18,2)")]

●	BalanceDue (DECIMAL(18,2) / decimal) — NOT NULL. [Column(TypeName = "decimal(18,2)")]

●	PaymentStatus (ENUM / PaymentStatus) — NOT NULL. Unpaid, PartiallyPaid, Paid, Refunded.

●	IsVoided (BOOLEAN / bool) — NOT NULL, default false.

●	VoidReason (TEXT / string) — NULL.

●	Notes (TEXT / string) — NULL.

●	CreatedAt (DATETIME / DateTime) — NOT NULL. [Required]

●	UpdatedAt (DATETIME / DateTime) — NULL.

●	ClosedByStaffId (UUID / Guid) — FK → Staff(StaffId), NULL. [ForeignKey("ClosedByStaff")]

Relationships: Order → OrderItem (1:N), Order → Payment (1:N), Order → KitchenTicket (1:N).
Indexes: (BranchId, OrderNumber), (PlacedAt), (Status). [Index("IX_Order_PlacedAt")], [Index("IX_Order_Status")]
________________________________________
OrderItem
●	OrderItemId (UUID / Guid) — PK, NOT NULL. [Key]

●	OrderId (UUID / Guid) — FK → Order(OrderId), NOT NULL. [ForeignKey("Order")], [Index]

●	DishId (UUID / Guid) — FK → Dish(DishId), NOT NULL. [ForeignKey("Dish")], [Index]

●	MenuPriceId (UUID / Guid) — FK → MenuPriceHistory(MenuPriceId), NOT NULL. [ForeignKey("MenuPrice")]

●	ParentOrderItemId (UUID / Guid) — FK → OrderItem(OrderItemId), NULL (for modifiers/combo components). [ForeignKey("ParentOrderItem")]

●	Quantity (DECIMAL(18,3) / decimal) — NOT NULL. [Column(TypeName = "decimal(18,3)")]

●	UnitPrice (DECIMAL(18,2) / decimal) — NOT NULL. [Column(TypeName = "decimal(18,2)")]

●	LineTotal (DECIMAL(18,2) / decimal) — NOT NULL (Quantity * UnitPrice +/- adjustments). [Column(TypeName = "decimal(18,2)")]

●	Status (ENUM / OrderItemStatus) — NOT NULL. Pending, SentToKitchen, Preparing, Ready, Served, Cancelled.

●	KitchenSectionId (UUID / Guid) — FK → KitchenSection(KitchenSectionId), NULL. [ForeignKey("KitchenSection")]

●	RequestedAt (DATETIME / DateTime) — NOT NULL. [Required]

●	ServedAt (DATETIME / DateTime) — NULL.

●	IsDiscounted (BOOLEAN / bool) — NOT NULL, default false.

●	DiscountAmount (DECIMAL(18,2) / decimal) — NOT NULL, default 0. [Column(TypeName = "decimal(18,2)")]

●	SpecialInstructions (TEXT / string) — NULL.

●	CreatedAt (DATETIME / DateTime) — NOT NULL. [Required]

●	UpdatedAt (DATETIME / DateTime) — NULL.

Business notes: Track ParentOrderItemId for combos / modifiers so inventory deduction can be handled per ingredient.
________________________________________
Payment
●	PaymentId (UUID / Guid) — PK, NOT NULL. [Key]

●	OrderId (UUID / Guid) — FK → Order(OrderId), NULL if standalone. [ForeignKey("Order")], [Index]

●	TransactionReference (VARCHAR(100) / string) — NULL, gateway or bank reference. [MaxLength(100)]

●	PaymentMethod (ENUM / PaymentMethod) — NOT NULL. Cash, Card, QR, EWallet, Voucher, Split, Credit.

●	Amount (DECIMAL(18,2) / decimal) — NOT NULL. [Column(TypeName = "decimal(18,2)")]

●	PaymentDate (DATETIME / DateTime) — NOT NULL. [Required]

●	CardType (VARCHAR(50) / string) — NULL (if card). [MaxLength(50)]

●	CardLast4 (VARCHAR(4) / string) — NULL — store last 4 digits only. [MaxLength(4)]

●	AuthCode (VARCHAR(50) / string) — NULL. [MaxLength(50)]

●	Status (ENUM / PaymentStatus) — NOT NULL. Pending, Approved, Failed, Refunded.

●	ProcessedByStaffId (UUID / Guid) — FK → Staff(StaffId), NULL. [ForeignKey("ProcessedByStaff")]

●	Notes (TEXT / string) — NULL.

●	CreatedAt (DATETIME / DateTime) — NOT NULL. [Required]

●	UpdatedAt (DATETIME / DateTime) — NULL.

Business notes: Support split payments by having multiple Payment rows for single Order.
________________________________________
Refund
●	RefundId (UUID) — PK.

●	OriginalPaymentId (UUID) — FK → Payment(PaymentId).

●	OrderId (UUID) — FK → Order(OrderId).

●	Amount (DECIMAL) — NOT NULL.

●	RefundMethod (ENUM) — OriginalMethod, Cash, StoreCredit.

●	Reason (TEXT) — NOT NULL.

●	ProcessedByStaffId (UUID) — FK → Staff(StaffId).

●	RefundDate (DATETIME).

●	Status (ENUM) — Requested, Processed, Rejected.

●	CreatedAt, UpdatedAt.

________________________________________
Menu & Recipe Entities
Dish (Menu Item)
●	DishId (UUID) — PK.

●	SKU (VARCHAR) — OPTIONAL internal code.

●	Name (VARCHAR) — NOT NULL.

●	Description (TEXT) — NULL.

●	CategoryId (UUID) — FK → Category(CategoryId).

●	DefaultServingPrice (DECIMAL) — NOT NULL.

●	Active (BOOLEAN) — NOT NULL.

●	IsAvailableOnline (BOOLEAN) — show on web/app.

●	PreparationTimeMinutes (INT) — NULL.

●	KitchenSectionId (UUID) — FK → KitchenSection.

●	IsComposite (BOOLEAN) — if it maps to recipe/ingredients.

●	PortionSize (VARCHAR) — e.g., Large/Regular.

●	ImageUrl (VARCHAR).

●	Taxable (BOOLEAN).

●	CreatedAt, UpdatedAt.

________________________________________
Category
●	CategoryId (UUID) — PK.

●	Name (VARCHAR) — NOT NULL.

●	Description (TEXT) — NULL.

●	ParentCategoryId (UUID) — FK → Category(CategoryId), NULL.

●	DisplayOrder (INT) — for menu ordering.

●	Active (BOOLEAN).

●	CreatedAt, UpdatedAt.

________________________________________
Recipe / DishIngredient (mapping Dish → Ingredient)
●	DishIngredientId (UUID) — PK.

●	DishId (UUID) — FK → Dish(DishId), NOT NULL.

●	InventoryItemId (UUID) — FK → InventoryItem(ItemId), NOT NULL.

●	QuantityPerPortion (DECIMAL) — NOT NULL (units match InventoryItem.Unit).

●	UnitMultiplier (DECIMAL) — conversion factor if needed.

●	IsOptional (BOOLEAN) — modifier/optional ingredient.

●	CreatedAt, UpdatedAt.

Business notes: Use this table to deduct inventory when OrderItem is served/sold.
________________________________________
MenuPriceHistory
●	MenuPriceId (UUID) — PK.

●	DishId (UUID) — FK → Dish(DishId), NOT NULL.

●	Price (DECIMAL) — NOT NULL.

●	EffectiveFrom (DATETIME) — NOT NULL.

●	EffectiveTo (DATETIME) — NULL for current.

●	ChangedByStaffId (UUID) — FK → Staff(StaffId).

●	Reason (VARCHAR) — e.g., promotion, price update.

●	CreatedAt (DATETIME).

Business notes: Snapshot price at time of order (OrderItem.MenuPriceId) to preserve historical revenue.
________________________________________
Inventory & Procurement Entities
InventoryItem
●	ItemId (UUID) — PK.

●	SKU (VARCHAR) — supplier SKU / internal SKU.

●	Name (VARCHAR) — NOT NULL.

●	Unit (VARCHAR) — e.g., kg, g, L, pcs.

●	CurrentQuantity (DECIMAL) — NOT NULL.

●	ReorderLevel (DECIMAL) — threshold to trigger reorder.

●	ReorderQuantity (DECIMAL) — suggested reorder amount.

●	CostPerUnit (DECIMAL) — latest landed cost.

●	BatchTracking (BOOLEAN) — if true, track batch/lot and expiry.

●	ShelfLifeDays (INT) — NULL.

●	IsActive (BOOLEAN).

●	SupplierId (UUID) — FK → Supplier(SupplierId), NULL.

●	StorageLocation (VARCHAR) — e.g., ColdRoom A.

●	CreatedAt, UpdatedAt.

________________________________________
StockTransaction (ledger of all inventory moves)
●	StockTransactionId (UUID) — PK.

●	ItemId (UUID) — FK → InventoryItem(ItemId).

●	TransactionType (ENUM) — PurchaseIn, SaleOut, AdjustmentIn, AdjustmentOut, TransferIn, TransferOut, Waste.

●	ReferenceId (UUID) — FK to relevant doc (e.g., PurchaseOrderId, OrderId).

●	Quantity (DECIMAL) — positive for in, negative for out (or explicit sign via TransactionType).

●	UnitCost (DECIMAL) — cost used for in-transactions.

●	TotalCost (DECIMAL) — Quantity * UnitCost.

●	LotNumber (VARCHAR) — NULL.

●	ExpiryDate (DATE) — NULL.

●	CreatedByStaffId (UUID).

●	CreatedAt (DATETIME).

●	Note (TEXT).

Business notes: Do NOT update InventoryItem.CurrentQuantity directly without creating StockTransaction; keep ledger authoritative.
________________________________________
PurchaseOrder
●	PurchaseOrderId (UUID) — PK.

●	PONumber (VARCHAR) — unique per branch.

●	SupplierId (UUID) — FK → Supplier(SupplierId).

●	BranchId (UUID) — FK → Branch.

●	CreatedByStaffId (UUID).

●	OrderedAt (DATETIME).

●	ExpectedDeliveryDate (DATETIME).

●	Status (ENUM) — Draft, Placed, PartiallyReceived, Completed, Cancelled.

●	TotalAmount (DECIMAL).

●	Currency (VARCHAR).

●	Notes (TEXT).

●	CreatedAt, UpdatedAt.

________________________________________
PurchaseOrderLine
●	POLineId (UUID) — PK.

●	PurchaseOrderId (UUID) — FK → PurchaseOrder.

●	ItemId (UUID) — FK → InventoryItem.

●	QuantityOrdered (DECIMAL).

●	QuantityReceived (DECIMAL).

●	UnitCost (DECIMAL).

●	LineTotal (DECIMAL).

________________________________________
People & HR Entities
Staff
●	StaffId (UUID) — PK.

●	EmployeeNumber (VARCHAR) — unique.

●	FirstName (VARCHAR), LastName (VARCHAR).

●	FullName (computed).

●	Email (VARCHAR).

●	PhoneNumber (VARCHAR).

●	RoleId (UUID) — FK → Role(RoleId).

●	BranchId (UUID) — FK → Branch(BranchId).

●	HireDate (DATE).

●	EmploymentType (ENUM) — FullTime, PartTime, Contract.

●	BaseSalary (DECIMAL) — monthly or hourly based on EmploymentType.

●	HourlyRate (DECIMAL) — NULL if salaried.

●	IsActive (BOOLEAN).

●	Address (TEXT).

●	TaxId (VARCHAR) — for payroll.

●	AvatarUrl (VARCHAR).

●	CreatedAt, UpdatedAt.

________________________________________
Role
●	RoleId (UUID) — PK.

●	Name (VARCHAR) — e.g., Manager, Cashier, Waiter, Cook, Chef, Storekeeper.

●	Description (TEXT).

●	DefaultPermissions (JSON) — optional defaults.

●	CreatedAt, UpdatedAt.

________________________________________
StaffPermission / RolePermission
●	PermissionId (UUID) — PK.

●	Name (VARCHAR) — orders.create, orders.refund, inventory.adjust, etc.

●	Description (TEXT).

●	RolePermission table: RoleId + PermissionId.

________________________________________
Shift
●	ShiftId (UUID) — PK.

●	BranchId (UUID).

●	Name (VARCHAR) — e.g., Morning, Evening.

●	StartTime (TIME).

●	EndTime (TIME).

●	CreatedAt, UpdatedAt.

________________________________________
Attendance (TimeClock)
●	AttendanceId (UUID) — PK.

●	StaffId (UUID) — FK → Staff.

●	ClockInAt (DATETIME).

●	ClockOutAt (DATETIME).

●	ClockType (ENUM) — QR, Biometric, Manual.

●	Location (VARCHAR) — optional.

●	ShiftId (UUID) — FK → Shift, NULL.

●	ApprovedByStaffId (UUID) — for corrections.

●	Status (ENUM) — PendingApproval, Approved, Rejected.

●	Notes, CreatedAt, UpdatedAt.

________________________________________
PayrollRecord
●	PayrollId (UUID) — PK.

●	StaffId (UUID).

●	PayrollPeriodStart, PayrollPeriodEnd (DATE).

●	GrossPay (DECIMAL).

●	NetPay (DECIMAL).

●	Deductions (DECIMAL).

●	Taxes (DECIMAL).

●	PaidAt (DATETIME).

●	PaymentMethod (ENUM).

●	CreatedAt, UpdatedAt.

________________________________________
Customer & Loyalty Entities
Customer
●	CustomerId (UUID) — PK.

●	FirstName, LastName, FullName.

●	Email, PhoneNumber.

●	DateOfBirth (DATE) — optional for promotions.

●	Address (TEXT).

●	CreatedAt, UpdatedAt.

●	IsActive (BOOLEAN).

●	PreferredBranchId (UUID).

●	Notes (TEXT).

________________________________________
LoyaltyAccount
●	LoyaltyAccountId (UUID) — PK.

●	CustomerId (UUID) — FK.

●	PointsBalance (DECIMAL).

●	Tier (ENUM) — None, Silver, Gold, Platinum.

●	TierEffectiveFrom (DATETIME).

●	TierEffectiveTo (DATETIME).

●	TotalEarnedPoints, TotalRedeemedPoints.

●	CreatedAt, UpdatedAt.

________________________________________
LoyaltyTransaction
●	LoyaltyTransactionId (UUID) — PK.

●	LoyaltyAccountId (UUID).

●	OrderId (UUID) — FK, NULL.

●	Type (ENUM) — Earn, Redeem, Expire, Adjust.

●	Points (DECIMAL) — + or -.

●	Reason (TEXT).

●	CreatedAt.

________________________________________
Voucher / Promotion
●	VoucherId (UUID) — PK.

●	Code (VARCHAR) — unique coupon code.

●	Description (TEXT).

●	DiscountType (ENUM) — Percentage, FixedAmount, FreeItem.

●	DiscountValue (DECIMAL).

●	ValidFrom, ValidTo (DATETIME).

●	UsageLimitPerCustomer (INT).

●	TotalUsageLimit (INT).

●	AppliedTo (ENUM/JSON) — AllItems, Category, SpecificDish.

●	MinimumOrderAmount (DECIMAL).

●	IsActive (BOOLEAN).

●	CreatedAt, UpdatedAt.

________________________________________
Branching & Physical Layout Entities
Branch
●	BranchId (UUID) — PK.

●	Name (VARCHAR).

●	Code (VARCHAR).

●	Address (TEXT).

●	Phone (VARCHAR).

●	TimeZone (VARCHAR) — e.g., Asia/Bangkok.

●	DefaultCurrency (VARCHAR).

●	IsActive (BOOLEAN).

●	CreatedAt, UpdatedAt.

________________________________________
Table (Dining Table)
●	TableId (UUID) — PK.

●	BranchId (UUID).

●	Code (VARCHAR) — e.g., A01, T12.

●	Name (VARCHAR).

●	Area (VARCHAR) — e.g., MainHall, Terrace.

●	Seats (INT).

●	Status (ENUM) — Available, Occupied, Reserved, OutOfService.

●	IsOutdoor (BOOLEAN).

●	MapX, MapY (INT) — optional for UI floorplan.

●	CreatedAt, UpdatedAt.

________________________________________
Kitchen & Food Prep Entities
KitchenSection
●	KitchenSectionId (UUID) — PK.

●	Name (VARCHAR) — e.g., HotKitchen, ColdPass, Bar.

●	Description (TEXT).

●	PrinterDeviceId (UUID) — optional printer to send tickets.

●	CreatedAt, UpdatedAt.

________________________________________
KitchenTicket
●	KitchenTicketId (UUID) — PK.

●	OrderId (UUID).

●	OrderItemId (UUID) — FK.

●	TicketNumber (VARCHAR).

●	KitchenSectionId (UUID).

●	SentAt (DATETIME).

●	ReceivedAt (DATETIME).

●	Status (ENUM) — Sent, Acknowledged, Completed, Cancelled.

●	PrintedByStaffId (UUID).

●	Notes (TEXT).

________________________________________
Delivery & Reservation Entities
Reservation
●	ReservationId (UUID) — PK.

●	BranchId (UUID).

●	CustomerId (UUID).

●	ReservedTableId (UUID) — FK → Table, NULL for generic.

●	PartySize (INT).

●	ReserveStartAt (DATETIME).

●	ReserveEndAt (DATETIME).

●	Status (ENUM) — Pending, Confirmed, Completed, Cancelled, NoShow.

●	Source (ENUM) — Web, Phone, WalkIn, App.

●	Notes (TEXT).

●	CreatedAt, UpdatedAt.

________________________________________
DeliveryOrder (extends Order logically but separate for delivery specifics)
●	DeliveryOrderId (UUID) — PK.

●	OrderId (UUID) — FK → Order.

●	DeliveryAddress (TEXT).

●	RecipientName (VARCHAR).

●	RecipientPhone (VARCHAR).

●	DeliveryMethod (ENUM) — InHouse, ThirdParty.

●	DriverId (UUID) — FK → Driver(DriverId), NULL.

●	EstimatedDeliveryTime (DATETIME).

●	ActualDeliveryTime (DATETIME).

●	DeliveryFee (DECIMAL).

●	DeliveryStatus (ENUM) — Assigned, PickedUp, OnRoute, Delivered, Failed.

●	CreatedAt, UpdatedAt.

________________________________________
Driver
●	DriverId (UUID) — PK.

●	Name, PhoneNumber.

●	VehicleInfo (VARCHAR).

●	IsActive (BOOLEAN).

●	AssignedOrdersCount (INT).

●	CreatedAt, UpdatedAt.

________________________________________
Supplier & Accounts Payable Entities
Supplier
●	SupplierId (UUID) — PK.

●	Name (VARCHAR).

●	PrimaryContactName (VARCHAR).

●	Phone, Email.

●	Address (TEXT).

●	PaymentTerms (VARCHAR) — e.g., Net30.

●	Currency (VARCHAR).

●	IsActive (BOOLEAN).

●	CreatedAt, UpdatedAt.

________________________________________
SupplierInvoice
●	SupplierInvoiceId (UUID) — PK.

●	SupplierId (UUID).

●	InvoiceNumber (VARCHAR).

●	InvoiceDate (DATE).

●	DueDate (DATE).

●	TotalAmount (DECIMAL).

●	BalanceDue (DECIMAL).

●	Status (ENUM) — Open, PartiallyPaid, Paid, Overdue.

●	LinkedPOId (UUID) — FK → PurchaseOrder.

●	CreatedAt, UpdatedAt.

________________________________________
Accounting & Finance Entities
FinancialTransaction
●	TransactionId (UUID) — PK.

●	BranchId (UUID).

●	Type (ENUM) — Sale, Expense, PurchasePayment, Receipt, Refund, Salary.

●	RelatedId (UUID) — FK to related doc (OrderId, SupplierInvoiceId, PayrollId).

●	Account (VARCHAR) — ledger account code.

●	Amount (DECIMAL).

●	Currency (VARCHAR).

●	Date (DATETIME).

●	Description (TEXT).

●	CreatedByStaffId (UUID).

●	CreatedAt, UpdatedAt.

________________________________________
AccountReceivable / AccountPayable (summary ledgers)
●	ARId / APId (UUID) — PK.

●	ReferenceId (UUID).

●	PartyId (CustomerId or SupplierId).

●	AmountDue, AmountPaid, BalanceDue.

●	DueDate.

●	Status.

●	CreatedAt, UpdatedAt.

________________________________________
Expense
●	ExpenseId (UUID) — PK.

●	BranchId (UUID).

●	Category (VARCHAR).

●	Amount (DECIMAL).

●	PaidTo (VARCHAR).

●	PaidAt (DATETIME).

●	ReceiptImageUrl (VARCHAR).

●	Notes, CreatedBy, CreatedAt.

________________________________________
Audit, System & Config Entities
AuditLog
●	AuditId (UUID) — PK.

●	EntityName (VARCHAR).

●	EntityId (UUID / VARCHAR).

●	Action (ENUM) — Create, Update, Delete, Login, PermissionChange.

●	ChangedByStaffId (UUID).

●	ChangedAt (DATETIME).

●	ChangeSummary (JSON) — old/new values.

●	IpAddress, UserAgent.

________________________________________
Device (POS devices, printers)
●	DeviceId (UUID) — PK.

●	BranchId.

●	Name (VARCHAR).

●	Type (ENUM) — POS, KitchenPrinter, ReceiptPrinter, Kiosk.

●	Identifier (VARCHAR) — MAC, serial.

●	IsActive.

●	LastSeenAt (DATETIME).

●	CreatedAt, UpdatedAt.

________________________________________
ApplicationSetting
●	SettingKey (VARCHAR) — PK.

●	SettingValue (TEXT / JSON).

●	Description.

●	BranchScoped (BOOLEAN).

●	BranchId (UUID) — NULL if global.

●	UpdatedAt.

________________________________________
Additional Practical Entities
StockCount (Inventory Audit)
●	StockCountId (UUID).

●	BranchId.

●	PerformedByStaffId.

●	CountDate.

●	Status — Draft, Completed.

●	Notes.

●	CreatedAt.

StockCountLine
●	StockCountLineId.

●	StockCountId.

●	ItemId.

●	CountedQuantity.

●	SystemQuantity.

●	Variance.

●	Remark.

________________________________________
Relationship & Index Recommendations (summary)
●	FKs: Use strict foreign keys for data integrity (Order → Branch, OrderItem → Order/Dish, StockTransaction → InventoryItem, etc.).

●	Indexes:

○	Orders: (BranchId, PlacedAt), (OrderNumber), (Status).

○	InventoryItem: (Name), (CurrentQuantity).

○	PurchaseOrder: (SupplierId, Status).

○	Customer: (PhoneNumber), (Email).

○	Staff: (EmployeeNumber).

●	Audit fields: keep CreatedAt, CreatedBy, UpdatedAt, UpdatedBy on major tables.

●	Soft deletes: prefer IsActive / IsDeleted over physical deletes for important entities (Customer, Dish, InventoryItem, Supplier).

●	Event sourcing / ledger: Important financial & inventory changes should be recorded in a append-only ledger (StockTransaction, FinancialTransaction) for traceability.

●	Price snapshots: Always store price snapshot (MenuPriceHistory) to avoid revenue skew when price changes later.

