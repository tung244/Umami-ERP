using Microsoft.AspNetCore.SignalR;
using QuanLiKhoHang.Hubs;

namespace QuanLiKhoHang.Extensions
{
    public static class DishHubExtensions
    {
        public static async Task SendDishUpdate(this IHubContext<DishHub> hubContext, Models.Entities.Dish dish, string action)
        {
            await hubContext.Clients.All.SendAsync("ReceiveDishUpdate", dish, action);
        }

        public static async Task SendDishStatusUpdate(this IHubContext<DishHub> hubContext, Guid dishId, bool isActive)
        {
            await hubContext.Clients.All.SendAsync("ReceiveDishStatusUpdate", dishId, isActive);
        }

        public static async Task SendDishPreparationUpdate(this IHubContext<DishHub> hubContext, Guid dishId, decimal pendingQuantity)
        {
            await hubContext.Clients.All.SendAsync("ReceiveDishPreparationUpdate", dishId, pendingQuantity);
        }

        public static async Task SendKitchenTicketAdded(this IHubContext<DishHub> hubContext, object data)
        {
            await hubContext.Clients.All.SendAsync("KitchenTicketAdded", data);
        }

        public static async Task SendKitchenTicketUpdated(this IHubContext<DishHub> hubContext, object data)
        {
            await hubContext.Clients.All.SendAsync("KitchenTicketUpdated", data);
        }

        public static async Task SendTableStatusUpdated(this IHubContext<DishHub> hubContext, object data)
        {
            await hubContext.Clients.All.SendAsync("TableStatusUpdated", data);
        }

        public static async Task SendTableStatusChanged(this IHubContext<DishHub> hubContext, Guid tableId, string status)
        {
            await hubContext.Clients.All.SendAsync("TableStatusChanged", new { tableId, status });
        }
    }
}