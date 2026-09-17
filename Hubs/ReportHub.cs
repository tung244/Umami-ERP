using Microsoft.AspNetCore.SignalR;

namespace QuanLiKhoHang.Hubs
{
    /// <summary>
    /// SignalR Hub cho báo cáo và phân tích realtime
    /// </summary>
    public class ReportHub : Hub
    {
        /// <summary>
        /// Thông báo cập nhật báo cáo doanh thu (khi có order mới, payment mới)
        /// </summary>
        public async Task NotifyRevenueUpdate(string message = "Revenue data updated")
        {
            await Clients.All.SendAsync("RevenueUpdated", new { message, timestamp = DateTime.Now });
        }

        /// <summary>
        /// Thông báo cập nhật báo cáo khách hàng (khi có customer mới, tier change)
        /// </summary>
        public async Task NotifyCustomerUpdate(string message = "Customer data updated")
        {
            await Clients.All.SendAsync("CustomerUpdated", new { message, timestamp = DateTime.Now });
        }

        /// <summary>
        /// Thông báo cập nhật báo cáo kho (khi có purchase order mới, stock transaction)
        /// </summary>
        public async Task NotifyInventoryUpdate(string message = "Inventory data updated")
        {
            await Clients.All.SendAsync("InventoryUpdated", new { message, timestamp = DateTime.Now });
        }

        /// <summary>
        /// Thông báo cập nhật phân tích xu hướng (khi có order mới ảnh hưởng peak hours)
        /// </summary>
        public async Task NotifyTrendsUpdate(string message = "Trends data updated")
        {
            await Clients.All.SendAsync("TrendsUpdated", new { message, timestamp = DateTime.Now });
        }

        /// <summary>
        /// Thông báo cập nhật tất cả báo cáo
        /// </summary>
        public async Task NotifyAllReportsUpdate(string message = "All reports updated")
        {
            await Clients.All.SendAsync("AllReportsUpdated", new { message, timestamp = DateTime.Now });
        }
    }
}
