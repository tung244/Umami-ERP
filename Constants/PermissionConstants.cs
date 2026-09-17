namespace QuanLiKhoHang.Constants
{
    /// <summary>
    /// Danh sách các permission trong hệ thống
    /// </summary>
    public static class PermissionConstants
    {
        // Dashboard
        public const string VIEW_DASHBOARD = "Dashboard.View";

        // Menu Management
        public const string VIEW_CATEGORY = "Menu.Category.View";
        public const string CREATE_CATEGORY = "Menu.Category.Create";
        public const string EDIT_CATEGORY = "Menu.Category.Edit";
        public const string DELETE_CATEGORY = "Menu.Category.Delete";

        public const string VIEW_DISH = "Menu.Dish.View";
        public const string CREATE_DISH = "Menu.Dish.Create";
        public const string EDIT_DISH = "Menu.Dish.Edit";
        public const string DELETE_DISH = "Menu.Dish.Delete";

        public const string VIEW_MENU_PRICE_HISTORY = "Menu.PriceHistory.View";
        public const string CREATE_MENU_PRICE_HISTORY = "Menu.PriceHistory.Create";

        public const string VIEW_DISH_INGREDIENT = "Menu.DishIngredient.View";
        public const string CREATE_DISH_INGREDIENT = "Menu.DishIngredient.Create";
        public const string EDIT_DISH_INGREDIENT = "Menu.DishIngredient.Edit";

        // Inventory Management
        public const string VIEW_INVENTORY = "Inventory.View";
        public const string IMPORT_INVENTORY = "Inventory.Import";
        public const string EXPORT_INVENTORY = "Inventory.Export";
        public const string STOCK_TAKING = "Inventory.StockTaking";

        // Staff Management
        public const string VIEW_STAFF = "Staff.View";
        public const string CREATE_STAFF = "Staff.Create";
        public const string EDIT_STAFF = "Staff.Edit";
        public const string DELETE_STAFF = "Staff.Delete";

        public const string VIEW_PAYROLL = "Staff.Payroll.View";
        public const string MANAGE_PAYROLL = "Staff.Payroll.Manage";

        public const string MANAGE_ROLES_AND_PERMISSIONS = "Staff.RolesAndPermissions.Manage";

        public const string VIEW_SHIFT = "Staff.Shift.View";
        public const string CREATE_SHIFT = "Staff.Shift.Create";
        public const string EDIT_SHIFT = "Staff.Shift.Edit";

        public const string VIEW_ATTENDANCE = "Staff.Attendance.View";
        public const string MANAGE_ATTENDANCE = "Staff.Attendance.Manage";

        // Customer Management
        public const string VIEW_CUSTOMER = "Customer.View";
        public const string CREATE_CUSTOMER = "Customer.Create";
        public const string EDIT_CUSTOMER = "Customer.Edit";
        public const string DELETE_CUSTOMER = "Customer.Delete";

        public const string MANAGE_LOYALTY = "Customer.Loyalty.Manage";
        public const string MANAGE_VOUCHER = "Customer.Voucher.Manage";

        // POS - Sales
        public const string VIEW_TABLE = "POS.Table.View";
        public const string MANAGE_TABLE = "POS.Table.Manage";
        public const string VIEW_TABLE_MANAGEMENT = "POS.TableManagement.View";

        public const string CREATE_ORDER = "POS.Order.Create";
        public const string VIEW_ORDER = "POS.Order.View";
        public const string EDIT_ORDER = "POS.Order.Edit";
        public const string DELETE_ORDER = "POS.Order.Delete";

        public const string VIEW_SALES_REPORT = "POS.SalesReport.View";

        // Reservation & Delivery
        public const string VIEW_RESERVATION = "Reservation.View";
        public const string CREATE_RESERVATION = "Reservation.Create";
        public const string EDIT_RESERVATION = "Reservation.Edit";
        public const string DELETE_RESERVATION = "Reservation.Delete";

        public const string MANAGE_ONLINE_ORDER = "OnlineOrder.Manage";
        public const string MANAGE_DELIVERY = "Delivery.Manage";

        // Supplier Management
        public const string VIEW_SUPPLIER = "Supplier.View";
        public const string MANAGE_SUPPLIER = "Supplier.Manage";
        public const string MANAGE_SUPPLIER_DEBT = "Supplier.Debt.Manage";

        // Reports & Analytics
        public const string VIEW_REVENUE_REPORT = "Report.Revenue.View";
        public const string VIEW_FINANCE_REPORT = "Report.Finance.View";
        public const string VIEW_ANALYTICS = "Report.Analytics.View";
    }
}
