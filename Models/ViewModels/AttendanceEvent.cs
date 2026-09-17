namespace QuanLiKhoHang.Models.ViewModels;

public class AttendanceEvent
{
    public Guid StaffId { get; set; }
    public string StaffName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public decimal Confidence { get; set; }
    public DateTime? ClockInAt { get; set; }
    public DateTime? ClockOutAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
