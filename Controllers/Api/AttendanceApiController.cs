using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Hubs;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using QuanLiKhoHang.Models.Requests;
using QuanLiKhoHang.Models.ViewModels;

namespace QuanLiKhoHang.Controllers.Api;

[ApiController]
[Route("api/attendance")]
public class AttendanceApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<AttendanceHub> _hub;

    public AttendanceApiController(ApplicationDbContext context, IHubContext<AttendanceHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpPost("face")]
    public async Task<IActionResult> FaceAttendance([FromBody] FaceAttendanceRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Dữ liệu đầu vào bị thiếu." });
        }

        if (request.Confidence < 0 || request.Confidence > 1)
        {
            return BadRequest(new { message = "Độ tin cậy phải trong khoảng 0 đến 1." });
        }

        var staff = await _context.Staff
            .Include(s => s.Branch)
            .FirstOrDefaultAsync(s => s.StaffId == request.StaffId && s.IsActive);

        if (staff == null)
        {
            return NotFound(new { message = "Không tìm thấy nhân viên đang hoạt động." });
        }

        var direction = request.Direction?.Trim().ToLowerInvariant();
        if (direction != "checkin" && direction != "checkout")
        {
            return BadRequest(new { message = "Direction phải là 'checkin' hoặc 'checkout'." });
        }

        var timestamp = request.Timestamp;
        var workDate = timestamp.Date;
        Attendance? attendance;

        if (direction == "checkin")
        {
            attendance = await _context.Attendances
                .Where(a => a.StaffId == staff.StaffId && a.ClockInAt.Date == workDate && !a.ClockOutAt.HasValue)
                .OrderByDescending(a => a.ClockInAt)
                .FirstOrDefaultAsync();

            if (attendance != null)
            {
                attendance.ClockInAt = attendance.ClockInAt > timestamp ? timestamp : attendance.ClockInAt;
                attendance.UpdatedAt = DateTime.Now;
                attendance.Notes = BuildNotes(direction, request.Confidence);
                _context.Update(attendance);
            }
            else
            {
                attendance = new Attendance
                {
                    AttendanceId = Guid.NewGuid(),
                    StaffId = staff.StaffId,
                    ClockInAt = timestamp,
                    ClockType = ClockType.Biometric,
                    Notes = BuildNotes(direction, request.Confidence),
                    Status = AttendanceStatus.Approved,
                    CreatedAt = DateTime.Now
                };
                _context.Add(attendance);
            }
        }
        else
        {
            attendance = await _context.Attendances
                .Where(a => a.StaffId == staff.StaffId && a.ClockInAt.Date == workDate && !a.ClockOutAt.HasValue)
                .OrderBy(a => a.ClockInAt)
                .FirstOrDefaultAsync();

            if (attendance == null)
            {
                return BadRequest(new { message = "Không có bản ghi check-in để check-out." });
            }

            attendance.ClockOutAt = timestamp >= attendance.ClockInAt ? timestamp : attendance.ClockInAt;
            attendance.ClockType = ClockType.Biometric;
            attendance.Status = AttendanceStatus.Approved;
            attendance.UpdatedAt = DateTime.Now;
            attendance.Notes = BuildNotes(direction, request.Confidence);
            _context.Update(attendance);
        }

        await _context.SaveChangesAsync();

        var payload = new AttendanceEvent
        {
            StaffId = staff.StaffId,
            StaffName = staff.FullName,
            EmployeeNumber = staff.EmployeeNumber,
            BranchName = staff.Branch?.Name ?? string.Empty,
            Direction = direction,
            Timestamp = timestamp,
            Confidence = request.Confidence,
            ClockInAt = attendance.ClockInAt,
            ClockOutAt = attendance.ClockOutAt,
            Status = attendance.Status.ToString()
        };

        await _hub.Clients.All.SendAsync("AttendanceUpdated", payload);

        var message = direction == "checkin"
            ? "Đã ghi nhận check-in bằng khuôn mặt."
            : "Đã ghi nhận check-out bằng khuôn mặt.";

        return Ok(new { message, data = payload });
    }

    private static string BuildNotes(string direction, decimal confidence)
    {
        return $"Chấm công bằng khuôn mặt ({direction}) - độ tin cậy {(confidence * 100):0.##}%";
    }
}
