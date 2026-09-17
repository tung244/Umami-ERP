using Microsoft.AspNetCore.SignalR;
using QuanLiKhoHang.Models.ViewModels;

namespace QuanLiKhoHang.Hubs;

public class AttendanceHub : Hub
{
    public async Task BroadcastAttendance(AttendanceEvent data)
    {
        await Clients.All.SendAsync("AttendanceUpdated", data);
    }
}
