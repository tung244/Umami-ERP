using Microsoft.AspNetCore.SignalR;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Hubs
{
    public class DishHub : Hub
    {
        public async Task SendDishUpdate(Dish dish, string action)
        {
            await Clients.All.SendAsync("ReceiveDishUpdate", dish, action);
        }

        public async Task SendDishStatusUpdate(Guid dishId, bool isActive)
        {
            await Clients.All.SendAsync("ReceiveDishStatusUpdate", dishId, isActive);
        }

        public async Task SendDishPreparationUpdate(Guid dishId, decimal pendingQuantity)
        {
            await Clients.All.SendAsync("ReceiveDishPreparationUpdate", dishId, pendingQuantity);
        }

        public async Task SendKitchenTicketAdded(object data)
        {
            await Clients.All.SendAsync("KitchenTicketAdded", data);
        }

        public async Task SendTableStatusUpdated(object data)
        {
            await Clients.All.SendAsync("TableStatusUpdated", data);
        }
    }
}