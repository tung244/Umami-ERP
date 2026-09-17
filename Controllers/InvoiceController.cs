using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using System.Text;

namespace QuanLiKhoHang.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoice/Print/{orderId}
        [HttpGet]
        public async Task<IActionResult> Print(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Include(o => o.Payments)
                .Include(o => o.CreatedByStaff)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Lấy thông tin chi nhánh
            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.BranchId == order.BranchId);

            ViewBag.Order = order;
            ViewBag.Branch = branch;
            ViewBag.Title = $"Hóa đơn - {order.OrderNumber}";

            return View();
        }

        // GET: Invoice/DownloadPDF/{orderId}
        [HttpGet]
        public async Task<IActionResult> DownloadPDF(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Include(o => o.Payments)
                .Include(o => o.CreatedByStaff)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.BranchId == order.BranchId);

            // Generate PDF (sẽ implement với QuestPDF sau)
            var pdfBytes = GenerateInvoicePDF(order, branch);

            return File(pdfBytes, "application/pdf", $"Invoice_{order.OrderNumber}.pdf");
        }

        // GET: Invoice/Receipt/{orderId} - Format in nhiệt siêu thị
        [HttpGet]
        public async Task<IActionResult> Receipt(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.BranchId == order.BranchId);

            ViewBag.Order = order;
            ViewBag.Branch = branch;
            ViewBag.Title = $"Phiếu thanh toán - {order.OrderNumber}";

            return View();
        }

        private byte[] GenerateInvoicePDF(Order order, Branch? branch)
        {
            // Tạm thời return empty array, sẽ implement với QuestPDF
            // TODO: Implement PDF generation với QuestPDF
            return Array.Empty<byte>();
        }

        // Các action cũ giữ lại để không bị lỗi
        public IActionResult AddNew()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Preview()
        {
            return View();
        }
    }
}
