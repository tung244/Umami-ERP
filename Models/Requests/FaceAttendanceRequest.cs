using System.ComponentModel.DataAnnotations;

namespace QuanLiKhoHang.Models.Requests;

public class FaceAttendanceRequest
{
    [Required]
    public Guid StaffId { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    [MaxLength(16)]
    public string Direction { get; set; } = string.Empty;

    [Range(0, 1)]
    public decimal Confidence { get; set; }
}
