using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Controllers
{
    public class TableController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 12;

        public TableController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Table
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? status, int page = 1)
        {
            var query = from t in _context.RestaurantTables.AsNoTracking()
                       .Include(t => t.Branch)
                       select t;

            // Tìm kiếm theo tên hoặc mã bàn
            if (!string.IsNullOrEmpty(search))
            {
                query = from t in query
                       where t.Name.Contains(search) || t.Code.Contains(search)
                       select t;
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<TableStatus>(status, out var tableStatus))
            {
                query = from t in query
                       where t.Status == tableStatus
                       select t;
            }

            // Sắp xếp
            query = from t in query
                   orderby t.Name
                   select t;

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.StatusFilter = status ?? "";
            ViewBag.Title = "Quản lý bàn ăn";

            return View();
        }

        // GET: Table/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var table = await _context.RestaurantTables.AsNoTracking()
                .Include(t => t.Branch)
                .Include(t => t.Orders.Where(o => o.Status != OrderStatus.Paid))
                .FirstOrDefaultAsync(t => t.TableId == id);

            if (table == null)
            {
                return NotFound();
            }

            ViewBag.Table = table;
            ViewBag.Title = $"Chi tiết bàn - {table.Name}";

            return View();
        }

        /// <summary>
        /// Tạo QR code cho bàn để khách hàng tự gọi món
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GenerateQR(Guid id)
        {
            var table = await _context.RestaurantTables.AsNoTracking()
                .FirstOrDefaultAsync(t => t.TableId == id && t.Status != TableStatus.OutOfService);

            if (table == null)
            {
                return NotFound("Bàn không tồn tại hoặc không hoạt động");
            }

            // Lấy IP address của server
            var host = Request.Host.Host;
            var scheme = Request.Scheme;
            var port = Request.Host.Port.HasValue ? $":{Request.Host.Port}" : "";
            
            // Nếu là localhost, thay bằng IP của server
            if (host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || host.Equals("127.0.0.1"))
            {
                var hostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                var ipv4 = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                if (ipv4 != null)
                {
                    host = ipv4.ToString();
                }
            }

            // Tạo URL cho public order
            var baseUrl = $"{scheme}://{host}{port}";
            var orderUrl = $"{baseUrl}/PublicOrder/Table/{table.TableId}";

            ViewBag.Table = table;
            ViewBag.OrderUrl = orderUrl;
            ViewBag.Title = $"QR Code gọi món - Bàn {table.Name}";

            return View();
        }

        // GET: Table/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
            ViewBag.Title = "Thêm bàn mới";
            return View();
        }

        // POST: Table/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestaurantTable table)
        {
            if (ModelState.IsValid)
            {
                table.TableId = Guid.NewGuid();
                table.CreatedAt = DateTime.Now;
                table.UpdatedAt = DateTime.Now;

                // Tự động tạo mã bàn nếu không có
                if (string.IsNullOrEmpty(table.Code))
                {
                    var branchCode = _context.Branches.Find(table.BranchId)?.Code ?? "BR";
                    var sequence = (_context.RestaurantTables.Count(t => t.BranchId == table.BranchId) + 1).ToString("D3");
                    table.Code = $"{branchCode}-T{sequence}";
                }

                _context.RestaurantTables.Add(table);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã thêm bàn {table.Name} thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
            ViewBag.Title = "Thêm bàn mới";
            return View(table);
        }

        // GET: Table/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var table = await _context.RestaurantTables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
            ViewBag.Title = $"Sửa bàn - {table.Name}";
            return View(table);
        }

        // POST: Table/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RestaurantTable table)
        {
            if (id != table.TableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    table.UpdatedAt = DateTime.Now;
                    _context.Update(table);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Đã cập nhật bàn {table.Name} thành công!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.TableId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
            ViewBag.Title = $"Sửa bàn - {table.Name}";
            return View(table);
        }

        // POST: Table/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var table = await _context.RestaurantTables
                .Include(t => t.Orders)
                .FirstOrDefaultAsync(t => t.TableId == id);

            if (table == null)
            {
                return NotFound();
            }

            // Kiểm tra xem bàn có đơn hàng đang hoạt động không
            if (table.Orders.Any(o => o.Status != OrderStatus.Paid))
            {
                TempData["ErrorMessage"] = "Không thể xóa bàn đang có đơn hàng hoạt động!";
                return RedirectToAction("Details", new { id });
            }

            _context.RestaurantTables.Remove(table);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã xóa bàn {table.Name} thành công!";
            return RedirectToAction("Index");
        }

        // POST: Table/ChangeStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(Guid id, TableStatus newStatus)
        {
            var table = await _context.RestaurantTables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            // Validation logic
            if (newStatus == TableStatus.Occupied && table.Status != TableStatus.Available)
            {
                TempData["ErrorMessage"] = "Chỉ có thể đặt bàn Available thành Occupied!";
                return RedirectToAction("Details", new { id });
            }

            if (newStatus == TableStatus.Available && table.Status != TableStatus.Occupied)
            {
                TempData["ErrorMessage"] = "Chỉ có thể thu bàn Occupied thành Available!";
                return RedirectToAction("Details", new { id });
            }

            table.Status = newStatus;
            table.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            var statusName = GetStatusDisplayName(newStatus);
            TempData["SuccessMessage"] = $"Đã cập nhật trạng thái bàn thành {statusName}!";
            return RedirectToAction("Details", new { id });
        }

        // GET: Table/GetAvailableTables
        [HttpGet]
        public async Task<IActionResult> GetAvailableTables()
        {
            var availableTables = await _context.RestaurantTables.AsNoTracking()
                .Where(t => t.Status == TableStatus.Available)
                .OrderBy(t => t.Name)
                .Select(t => new
                {
                    t.TableId,
                    t.Name,
                    t.Code,
                    t.Seats,
                    Area = t.Area ?? "Chung"
                })
                .ToListAsync();

            return Json(availableTables);
        }

        private bool TableExists(Guid id)
        {
            return _context.RestaurantTables.Any(e => e.TableId == id);
        }

        private string GetStatusDisplayName(TableStatus status)
        {
            return status switch
            {
                TableStatus.Available => "Sẵn sàng",
                TableStatus.Occupied => "Đang sử dụng",
                TableStatus.Reserved => "Đã đặt",
                TableStatus.OutOfService => "Bảo trì",
                _ => status.ToString()
            };
        }
    }
}
