using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 12;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservation
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? status, DateTime? dateFrom, DateTime? dateTo, int page = 1)
        {
            var query = from r in _context.Reservations.AsNoTracking()
                       .Include(r => r.Customer)
                       .Include(r => r.ReservedTable)
                       .Include(r => r.Branch)
                       select r;

            // Tìm kiếm theo ghi chú
            if (!string.IsNullOrEmpty(search))
            {
                query = from r in query
                       where !string.IsNullOrEmpty(r.Notes) && r.Notes.Contains(search)
                       select r;
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<ReservationStatus>(status, out var reservationStatus))
            {
                query = from r in query
                       where r.Status == reservationStatus
                       select r;
            }

            // Lọc theo khoảng thời gian
            if (dateFrom.HasValue)
            {
                query = from r in query
                       where r.ReserveStartAt >= dateFrom.Value
                       select r;
            }

            if (dateTo.HasValue)
            {
                query = from r in query
                       where r.ReserveStartAt <= dateTo.Value
                       select r;
            }

            // Sắp xếp theo thời gian đặt
            query = from r in query
                   orderby r.ReserveStartAt descending
                   select r;

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.StatusFilter = status ?? "";
            ViewBag.DateFrom = dateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = dateTo?.ToString("yyyy-MM-dd");
            ViewBag.Title = "Quản lý đặt bàn";

            return View();
        }

        // GET: Reservation/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var reservation = await _context.Reservations.AsNoTracking()
                .Include(r => r.Customer)
                .Include(r => r.ReservedTable)
                .Include(r => r.Branch)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null)
            {
                return NotFound();
            }

            ViewBag.Reservation = reservation;
            ViewBag.Title = $"Chi tiết đặt bàn - {(reservation.Customer != null ? $"{reservation.Customer.FirstName} {reservation.Customer.LastName}".Trim() : "Khách vãng lai")}";

            return View();
        }

        // GET: Reservation/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
            ViewBag.Customers = _context.Customers.AsNoTracking().ToList();
            ViewBag.Title = "Tạo đặt bàn mới";
            return View();
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // Validation: Kiểm tra thời gian hợp lệ
                if (reservation.ReserveEndAt <= reservation.ReserveStartAt)
                {
                    ModelState.AddModelError("ReserveEndAt", "Thời gian kết thúc phải sau thời gian bắt đầu.");
                    ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
                    ViewBag.Customers = _context.Customers.AsNoTracking().ToList();
                    return View(reservation);
                }

                // Validation: Kiểm tra bàn có sẵn không
                if (reservation.ReservedTableId.HasValue)
                {
                    var conflictingReservation = await _context.Reservations
                        .Where(r => r.ReservedTableId == reservation.ReservedTableId &&
                                   r.Status != ReservationStatus.Cancelled &&
                                   r.Status != ReservationStatus.Completed &&
                                   r.Status != ReservationStatus.NoShow &&
                                   ((r.ReserveStartAt <= reservation.ReserveStartAt && r.ReserveEndAt > reservation.ReserveStartAt) ||
                                    (r.ReserveStartAt < reservation.ReserveEndAt && r.ReserveEndAt >= reservation.ReserveEndAt)))
                        .FirstOrDefaultAsync();

                    if (conflictingReservation != null)
                    {
                        ModelState.AddModelError("ReservedTableId", "Bàn đã được đặt trong khoảng thời gian này.");
                        ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
                        ViewBag.Customers = _context.Customers.AsNoTracking().ToList();
                        return View(reservation);
                    }
                }

                reservation.ReservationId = Guid.NewGuid();
                reservation.CreatedAt = DateTime.Now;
                reservation.UpdatedAt = DateTime.Now;
                reservation.Status = ReservationStatus.Pending;

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã tạo đặt bàn cho {reservation.PartySize} người thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
            ViewBag.Customers = _context.Customers.AsNoTracking().ToList();
            ViewBag.Title = "Tạo đặt bàn mới";
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Chỉ cho phép sửa đặt bàn chưa hoàn thành
            if (reservation.Status == ReservationStatus.Completed ||
                reservation.Status == ReservationStatus.Cancelled ||
                reservation.Status == ReservationStatus.NoShow)
            {
                TempData["ErrorMessage"] = "Không thể chỉnh sửa đặt bàn đã hoàn thành hoặc đã hủy.";
                return RedirectToAction("Details", new { id });
            }

            ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
            ViewBag.Customers = _context.Customers.AsNoTracking().ToList();
            ViewBag.Title = $"Sửa đặt bàn - {(reservation.Customer != null ? $"{reservation.Customer.FirstName} {reservation.Customer.LastName}".Trim() : "Khách vãng lai")}";
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Validation: Kiểm tra thời gian hợp lệ
                    if (reservation.ReserveEndAt <= reservation.ReserveStartAt)
                    {
                        ModelState.AddModelError("ReserveEndAt", "Thời gian kết thúc phải sau thời gian bắt đầu.");
                        ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
                        ViewBag.Customers = _context.Customers.AsNoTracking().ToList();
                        return View(reservation);
                    }

                    // Validation: Kiểm tra bàn có sẵn không (trừ đặt bàn hiện tại)
                    if (reservation.ReservedTableId.HasValue)
                    {
                        var conflictingReservation = await _context.Reservations
                            .Where(r => r.ReservationId != reservation.ReservationId &&
                                       r.ReservedTableId == reservation.ReservedTableId &&
                                       r.Status != ReservationStatus.Cancelled &&
                                       r.Status != ReservationStatus.Completed &&
                                       r.Status != ReservationStatus.NoShow &&
                                       ((r.ReserveStartAt <= reservation.ReserveStartAt && r.ReserveEndAt > reservation.ReserveStartAt) ||
                                        (r.ReserveStartAt < reservation.ReserveEndAt && r.ReserveEndAt >= reservation.ReserveEndAt)))
                            .FirstOrDefaultAsync();

                        if (conflictingReservation != null)
                        {
                            ModelState.AddModelError("ReservedTableId", "Bàn đã được đặt trong khoảng thời gian này.");
                            ViewBag.Branches = _context.Branches.AsNoTracking().ToList();
                            ViewBag.Customers = _context.Customers.AsNoTracking().ToList();
                            return View(reservation);
                        }
                    }

                    reservation.UpdatedAt = DateTime.Now;
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Đã cập nhật đặt bàn thành công!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
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
            ViewBag.Customers = _context.Customers.AsNoTracking().ToList();
            ViewBag.Title = $"Sửa đặt bàn - {(reservation.Customer != null ? $"{reservation.Customer.FirstName} {reservation.Customer.LastName}".Trim() : "Khách vãng lai")}";
            return View(reservation);
        }

        // POST: Reservation/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id, string? reason)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Chỉ cho phép hủy đặt bàn chưa hoàn thành
            if (reservation.Status == ReservationStatus.Completed ||
                reservation.Status == ReservationStatus.Cancelled ||
                reservation.Status == ReservationStatus.NoShow)
            {
                TempData["ErrorMessage"] = "Không thể hủy đặt bàn đã hoàn thành hoặc đã hủy.";
                return RedirectToAction("Details", new { id });
            }

            reservation.Status = ReservationStatus.Cancelled;
            reservation.UpdatedAt = DateTime.Now;
            reservation.Notes = string.IsNullOrEmpty(reservation.Notes) ? $"Đã hủy: {reason}" : $"{reservation.Notes}\nĐã hủy: {reason}";

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã hủy đặt bàn thành công!";
            return RedirectToAction("Details", new { id });
        }

        // POST: Reservation/Confirm/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Chỉ cho phép xác nhận đặt bàn đang pending
            if (reservation.Status != ReservationStatus.Pending)
            {
                TempData["ErrorMessage"] = "Chỉ có thể xác nhận đặt bàn đang chờ xử lý.";
                return RedirectToAction("Details", new { id });
            }

            reservation.Status = ReservationStatus.Confirmed;
            reservation.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã xác nhận đặt bàn thành công!";
            return RedirectToAction("Details", new { id });
        }

        // GET: Reservation/GetAvailableTables
        [HttpGet]
        public async Task<IActionResult> GetAvailableTables(Guid branchId, DateTime startTime, DateTime endTime, int partySize)
        {
            var availableTables = await _context.RestaurantTables.AsNoTracking()
                .Where(t => t.BranchId == branchId &&
                           t.Status == TableStatus.Available &&
                           t.Seats >= partySize)
                .Where(t => !_context.Reservations.Any(r =>
                    r.ReservedTableId == t.TableId &&
                    r.Status != ReservationStatus.Cancelled &&
                    r.Status != ReservationStatus.Completed &&
                    r.Status != ReservationStatus.NoShow &&
                    ((r.ReserveStartAt <= startTime && r.ReserveEndAt > startTime) ||
                     (r.ReserveStartAt < endTime && r.ReserveEndAt >= endTime))))
                .OrderBy(t => t.Seats)
                .ThenBy(t => t.Name)
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

        private bool ReservationExists(Guid id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }

        private string GetStatusDisplayName(ReservationStatus status)
        {
            return status switch
            {
                ReservationStatus.Pending => "Chờ xử lý",
                ReservationStatus.Confirmed => "Đã xác nhận",
                ReservationStatus.Completed => "Đã hoàn thành",
                ReservationStatus.Cancelled => "Đã hủy",
                ReservationStatus.NoShow => "Không đến",
                _ => status.ToString()
            };
        }
    }
}