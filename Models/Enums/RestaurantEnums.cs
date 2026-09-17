namespace QuanLiKhoHang.Models.Enums;

public enum OrderType
{
    DineIn,
    TakeAway,
    Delivery,
    Counter
}

public enum OrderSource
{
    Pos,
    MobileApp,
    Web,
    Phone,
    ThirdParty
}

public enum OrderStatus
{
    Open,
    Confirmed,
    InPreparation,
    Ready,
    Served,
    Paid,
    Cancelled,
    Refunded
}

public enum OrderPaymentStatus
{
    Unpaid,
    PartiallyPaid,
    Paid,
    Refunded
}

public enum OrderItemStatus
{
    Pending,
    SentToKitchen,
    Preparing,
    Ready,
    Served,
    Cancelled
}

public enum PaymentMethod
{
    Cash,
    Card,
    Qr,
    EWallet,
    Voucher,
    Split,
    Credit
}

public enum PaymentStatus
{
    Pending,
    Received,
    Approved,
    Failed,
    Refunded
}

public enum RefundMethod
{
    OriginalMethod,
    Cash,
    StoreCredit
}

public enum RefundStatus
{
    Requested,
    Processed,
    Rejected
}

public enum StockTransactionType
{
    PurchaseIn,
    SaleOut,
    AdjustmentIn,
    AdjustmentOut,
    TransferIn,
    TransferOut,
    Waste
}

public enum PurchaseOrderStatus
{
    Draft,
    Placed,
    PartiallyReceived,
    Completed,
    Cancelled
}

public enum EmploymentType
{
    FullTime,
    PartTime,
    Contract
}

public enum ClockType
{
    Qr,
    Biometric,
    Manual
}

public enum AttendanceStatus
{
    PendingApproval,
    Approved,
    Rejected
}

public enum PayrollPaymentMethod
{
    Cash,
    BankTransfer,
    Check,
    DigitalWallet
}

public enum LoyaltyTier
{
    None,
    Bronze,     // Đồng
    Silver,     // Bạc
    Gold,       // Vàng
    Platinum    // Bạch kim
}

public enum LoyaltyTransactionType
{
    Earn,
    Redeem,
    Expire,
    Adjust
}

public enum VoucherDiscountType
{
    Percentage,
    FixedAmount,
    FreeItem
}

public enum TableStatus
{
    Available,
    Occupied,
    Reserved,
    OutOfService
}

public enum KitchenTicketStatus
{
    Sent,
    Acknowledged,
    Completed,
    Cancelled
}

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled,
    NoShow
}

public enum ReservationSource
{
    Web,
    Phone,
    WalkIn,
    App
}

public enum DeliveryMethod
{
    InHouse,
    ThirdParty
}

public enum DeliveryStatus
{
    Assigned,
    PickedUp,
    OnRoute,
    Delivered,
    Failed
}

public enum FinancialTransactionType
{
    Sale,
    Expense,
    PurchasePayment,
    Receipt,
    Refund,
    Salary
}

public enum AuditAction
{
    Create,
    Update,
    Delete,
    Login,
    PermissionChange
}

public enum DeviceType
{
    Pos,
    KitchenPrinter,
    ReceiptPrinter,
    Kiosk
}

public enum StockCountStatus
{
    Draft,
    Completed
}

public enum ShiftStatus
{
    Scheduled,
    Approved,
    Rejected,
    Completed
}

public enum StorageLocationType
{
    Warehouse,
    Refrigerator,
    Freezer,
    DryStorage,
    ChemicalStorage,
    OutdoorStorage
}

public enum StorageCondition
{
    RoomTemperature,
    Cool,
    Cold,
    Frozen,
    Dry,
    Dark,
    Ventilated
}
