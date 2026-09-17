using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using QuanLiKhoHang.Models;
using Microsoft.AspNetCore.SignalR;

namespace QuanLiKhoHang.Controllers{
    [Route("api/webhook")]
    [ApiController]
    public class SepayController : Controller{
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<PaymentHub> _hubContext;
        
        public SepayController(ApplicationDbContext context, IHubContext<PaymentHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public class SepayPayload
        {
            public string content { get; set; } // Nội dung chuyển khoản
            public decimal transferAmount  { get; set; }        // Số tiền
            // Thêm các trường khác nếu cần
        }

        [HttpPost ("sepay")]
        public async Task<IActionResult> HandleSepay([FromBody] SepayPayload payload)
        {
            Console.WriteLine($"[Sepay Webhook] Received: Content='{payload.content}', Amount={payload.transferAmount}");

            var descriptionPrefix = "Thanh toan don ";
            if (string.IsNullOrEmpty(payload.content) || !payload.content.StartsWith(descriptionPrefix))
            {
                return Ok("Invalid content format.");
            }

            // Tách chuỗi để lấy OrderNumber và các phần khác
            var contentWithoutPrefix = payload.content.Substring(descriptionPrefix.Length);
            var contentParts = contentWithoutPrefix.Split(' ');
            var orderNumberFromWebhook = contentParts[0]; // Lấy phần tử đầu tiên sau "Thanh toan don "

            Console.WriteLine($"[Sepay Webhook] Extracted OrderNumber: {orderNumberFromWebhook}");

            // --- PHẦN SỬA LỖI QUAN TRỌNG NHẤT ---
            // Tìm đơn hàng bằng cách so sánh phiên bản "đã được làm sạch" (loại bỏ dấu '-') của OrderNumber
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber.Replace("-", "") == orderNumberFromWebhook);

            if (order == null)
            {
                Console.WriteLine($"[Sepay Webhook] Order not found for cleaned number: {orderNumberFromWebhook}");
                return Ok("Order not found.");
            }

            // Phần còn lại của logic giữ nguyên
            if (order != null && order.PaymentStatus == PaymentStatus.Pending && payload.transferAmount == 10000)
            {
                order.PaymentStatus = PaymentStatus.Received;
                order.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                Console.WriteLine($"[Sepay Webhook] Order {order.OrderNumber} status updated to Received.");

                // --- PHẦN QUAN TRỌNG NHẤT ---
                // Gửi thông báo real-time đến tất cả client
                await _hubContext.Clients.All.SendAsync(
                    "ReceivePaymentStatusUpdate", // Tên sự kiện mà client sẽ lắng nghe
                    order.OrderId.ToString(),     // Dữ liệu gửi đi (OrderId)
                    "Received"                    // Dữ liệu gửi đi (Trạng thái mới)
                );
            }
            else
            {
                // ... (ghi log lỗi)
            }
            
            return Ok("Webhook processed.");
        }
    }
}
