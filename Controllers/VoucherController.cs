using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý voucher
    /// </summary>
    public class VoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public VoucherController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hiển thị danh sách voucher
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? sort, int page = 1)
        {
            var query = from v in _context.Vouchers.AsNoTracking()
                        select v;

            // Áp dụng bộ lọc
            if (!string.IsNullOrEmpty(search))
            {
                query = from v in query
                        where v.Code.Contains(search) ||
                              (v.Description != null && v.Description.Contains(search))
                        select v;
            }

            // Sắp xếp
            query = sort switch
            {
                "code" => from v in query orderby v.Code select v,
                "type" => from v in query orderby v.DiscountType select v,
                "value" => from v in query orderby v.DiscountValue select v,
                "validfrom" => from v in query orderby v.ValidFrom select v,
                "validto" => from v in query orderby v.ValidTo select v,
                _ => from v in query orderby v.CreatedAt descending select v
            };

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            ViewBag.Items = items;                 // List<Voucher>
            ViewBag.CurrentPage = page;            // int
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.SortOrder = sort ?? "default";
            ViewBag.Title = "Quản lý voucher";
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết voucher
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(v => v.VoucherId == id);

            if (voucher == null)
                return NotFound();

            ViewBag.Voucher = voucher;           // Voucher
            ViewBag.Title = $"Chi tiết voucher - {voucher.Code}";
            return View();
        }

        /// <summary>
        /// Hiển thị form tạo voucher mới
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm voucher mới";
            return View();
        }

        /// <summary>
        /// Xử lý tạo voucher mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Voucher model)
        {
            ModelState.Remove("VoucherId");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");

            if (ModelState.IsValid)
            {
                model.VoucherId = Guid.NewGuid();
                model.CreatedAt = DateTime.Now;

                _context.Vouchers.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm voucher thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Thêm voucher mới";
            return View(model);
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa voucher
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
                return NotFound();

            ViewBag.Title = $"Chỉnh sửa voucher - {voucher.Code}";
            return View(voucher);
        }

        /// <summary>
        /// Xử lý chỉnh sửa voucher
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Voucher model)
        {
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;

                _context.Vouchers.Update(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật voucher thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Title = $"Chỉnh sửa voucher - {model.Code}";
            return View(model);
        }

        /// <summary>
        /// Xóa voucher
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
                return NotFound();

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa voucher thành công!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// API: Lấy danh sách vouchers đang active
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveVouchers()
        {
            var now = DateTime.Now;
            var activeVouchers = await _context.Vouchers
                .Where(v => v.IsActive && 
                           v.ValidFrom <= now && 
                           v.ValidTo > now)
                .OrderBy(v => v.Code)
                .Select(v => new
                {
                    code = v.Code,
                    description = v.Description,
                    discountType = v.DiscountType.ToString(),
                    discountValue = v.DiscountValue,
                    minimumOrderAmount = v.MinimumOrderAmount
                })
                .ToListAsync();

            return Json(activeVouchers);
        }

        /// <summary>
        /// API: Validate và tính toán giảm giá từ voucher
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ValidateVoucher(string code, decimal orderAmount)
        {
            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(v => v.Code == code && v.IsActive);

            if (voucher == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Mã voucher không tồn tại hoặc đã bị vô hiệu hóa!"
                });
            }

            // Check validity period
            var now = DateTime.Now;
            if (now < voucher.ValidFrom || now > voucher.ValidTo)
            {
                return Json(new
                {
                    success = false,
                    message = $"Voucher chỉ có hiệu lực từ {voucher.ValidFrom:dd/MM/yyyy} đến {voucher.ValidTo:dd/MM/yyyy}!"
                });
            }

            // Check minimum order amount
            if (voucher.MinimumOrderAmount.HasValue && orderAmount < voucher.MinimumOrderAmount.Value)
            {
                return Json(new
                {
                    success = false,
                    message = $"Đơn hàng tối thiểu phải từ {voucher.MinimumOrderAmount.Value:N0} VND!"
                });
            }

            // Calculate discount
            decimal discount = 0;
            string discountInfo = "";

            switch (voucher.DiscountType)
            {
                case VoucherDiscountType.Percentage:
                    discount = orderAmount * (voucher.DiscountValue / 100);
                    discountInfo = $"Giảm {voucher.DiscountValue}%";
                    break;

                case VoucherDiscountType.FixedAmount:
                    discount = voucher.DiscountValue;
                    discountInfo = $"Giảm {voucher.DiscountValue:N0} VND";
                    break;

                case VoucherDiscountType.FreeItem:
                    // FreeItem logic can be handled separately
                    discountInfo = "Tặng món miễn phí";
                    break;
            }

            // Ensure discount doesn't exceed order amount
            if (discount > orderAmount)
                discount = orderAmount;

            return Json(new
            {
                success = true,
                message = "Áp dụng voucher thành công!",
                voucherCode = voucher.Code,
                discountType = voucher.DiscountType.ToString(),
                discountValue = voucher.DiscountValue,
                discountAmount = discount,
                discountInfo = discountInfo,
                finalAmount = orderAmount - discount
            });
        }
    }
}