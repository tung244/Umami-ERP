using Microsoft.AspNetCore.SignalR;
using QuanLiKhoHang.Hubs;

namespace QuanLiKhoHang.Services
{
    /// <summary>
    /// Service để gửi thông báo cập nhật báo cáo qua SignalR
    /// </summary>
    public interface IReportUpdateService
    {
        Task NotifyRevenueUpdate(string message = "Revenue data updated");
        Task NotifyCustomerUpdate(string message = "Customer data updated");
        Task NotifyInventoryUpdate(string message = "Inventory data updated");
        Task NotifyTrendsUpdate(string message = "Trends data updated");
        Task NotifyAllReportsUpdate(string message = "All reports updated");
    }

    public class ReportUpdateService : IReportUpdateService
    {
        private readonly IHubContext<ReportHub> _hubContext;

        public ReportUpdateService(IHubContext<ReportHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Thông báo cập nhật báo cáo doanh thu
        /// Gọi khi: Order mới, Payment mới, Order status change
        /// </summary>
        public async Task NotifyRevenueUpdate(string message = "Revenue data updated")
        {
            await _hubContext.Clients.All.SendAsync("RevenueUpdated", new 
            { 
                message, 
                timestamp = DateTime.Now 
            });
        }

        /// <summary>
        /// Thông báo cập nhật báo cáo khách hàng
        /// Gọi khi: Customer mới, Tier change, Loyalty points change
        /// </summary>
        public async Task NotifyCustomerUpdate(string message = "Customer data updated")
        {
            await _hubContext.Clients.All.SendAsync("CustomerUpdated", new 
            { 
                message, 
                timestamp = DateTime.Now 
            });
        }

        /// <summary>
        /// Thông báo cập nhật báo cáo kho
        /// Gọi khi: Purchase Order mới, Stock Transaction mới
        /// </summary>
        public async Task NotifyInventoryUpdate(string message = "Inventory data updated")
        {
            await _hubContext.Clients.All.SendAsync("InventoryUpdated", new 
            { 
                message, 
                timestamp = DateTime.Now 
            });
        }

        /// <summary>
        /// Thông báo cập nhật phân tích xu hướng
        /// Gọi khi: Order mới (ảnh hưởng peak hours), Table reservation
        /// </summary>
        public async Task NotifyTrendsUpdate(string message = "Trends data updated")
        {
            await _hubContext.Clients.All.SendAsync("TrendsUpdated", new 
            { 
                message, 
                timestamp = DateTime.Now 
            });
        }

        /// <summary>
        /// Thông báo cập nhật tất cả báo cáo
        /// Gọi khi có thay đổi ảnh hưởng nhiều báo cáo
        /// </summary>
        public async Task NotifyAllReportsUpdate(string message = "All reports updated")
        {
            await _hubContext.Clients.All.SendAsync("AllReportsUpdated", new 
            { 
                message, 
                timestamp = DateTime.Now 
            });
        }
    }
}
