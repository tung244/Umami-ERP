using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class PaymentHub : Hub
{
    // Server có thể gọi hàm này để gửi thông báo đến các client
    public async Task SendPaymentStatusUpdate(string orderId, string status)
    {
        // Gửi thông báo đến TẤT CẢ các client đang kết nối
        await Clients.All.SendAsync("ReceivePaymentStatusUpdate", orderId, status);
    }
}