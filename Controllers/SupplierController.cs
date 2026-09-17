
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Controllers
{
	/// <summary>
	/// Controller quản lý nhà cung cấp (Supplier)
	/// </summary>
	public class SupplierController : Controller
	{
	private readonly ApplicationDbContext _context;
		private const int PageSize = 10;

		public SupplierController(ApplicationDbContext context) => _context = context;

		[HttpGet]
		public async Task<IActionResult> Index(string? search, int page = 1)
		{
			var query =
				from s in _context.Suppliers.AsNoTracking()
				select s;

			if (!string.IsNullOrEmpty(search))
				query = from s in query where s.Name.Contains(search) || (s.PrimaryContactName != null && s.PrimaryContactName.Contains(search)) select s;

			
			var totalItems = await query.CountAsync();
			var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

			ViewBag.Items = items;                 // List<Supplier>
			ViewBag.CurrentPage = page;            // int
			ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
			ViewBag.SearchTerm = search ?? "";
			
			ViewBag.Title = "Quản lý nhà cung cấp";
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Details(Guid id)
		{
			var supplier = await _context.Suppliers
				.AsNoTracking()
				.FirstOrDefaultAsync(s => s.SupplierId == id);

			if (supplier == null)
				return NotFound();

			ViewBag.Item = supplier; // Supplier
			ViewBag.Title = "Chi tiết nhà cung cấp";
			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Title = "Tạo nhà cung cấp mới";
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Supplier model)
		{
			var existingSupplier = await _context.Suppliers
				.AsNoTracking()
				.FirstOrDefaultAsync(s => s.Name == model.Name || s.Email == model.Email);
			if (ModelState.IsValid)
			{
				if(existingSupplier != null)
				{
					if (existingSupplier.Name == model.Name)
						ModelState.AddModelError("Name", "Tên nhà cung cấp đã tồn tại.");
					if (existingSupplier.Email == model.Email)
						ModelState.AddModelError("Email", "Email nhà cung cấp đã tồn tại.");
					TempData["ErrorMessage"] = "Tên hoặc Email nhà cung cấp đã tồn tại.";
					ViewBag.Title = "Tạo nhà cung cấp mới";
					return View(model);
				}

				   // Nếu chưa nhập mã, tự động sinh mã duy nhất dạng NCCyyyyMMddHHmmss + 3 ký tự random
				   if (string.IsNullOrWhiteSpace(model.SupplierCode))
				   {
					   string baseCode = $"NCC{DateTime.Now:yyyyMMddHHmmss}";
					   string random = Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();
					   string code = baseCode + random;
					   // Đảm bảo không trùng
					   while (await _context.Suppliers.AnyAsync(s => s.SupplierCode == code))
					   {
						   random = Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();
						   code = baseCode + random;
					   }
					   model.SupplierCode = code;
				   }
				   model.CreatedAt = DateTime.Now;
				_context.Suppliers.Add(model);
				await _context.SaveChangesAsync();
				TempData["SuccessMessage"] = $"Đã tạo nhà cung cấp: {model.Name} (Mã: {model.SupplierCode}).";
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Title = "Tạo nhà cung cấp mới";
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var supplier = await _context.Suppliers.FindAsync(id);
			if (supplier == null)
				return NotFound();

			ViewBag.Title = "Chỉnh sửa nhà cung cấp";
			return View(supplier);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, Supplier model)
		{
			var existingSupplier = await _context.Suppliers
				.AsNoTracking()
				.FirstOrDefaultAsync(s => (s.Name == model.Name || s.Email == model.Email) && s.SupplierId != model.SupplierId);
			if (id != model.SupplierId)
				return NotFound();

			if (ModelState.IsValid)
			{
				if(existingSupplier != null)
				{
					if (existingSupplier.Name == model.Name)
						ModelState.AddModelError("Name", "Tên nhà cung cấp đã tồn tại.");
					if (existingSupplier.Email == model.Email)
						ModelState.AddModelError("Email", "Email nhà cung cấp đã tồn tại.");
					TempData["ErrorMessage"] = "Tên hoặc Email nhà cung cấp đã tồn tại.";
					ViewBag.Title = "Chỉnh sửa nhà cung cấp";
					return View(model);
				}
				try
				{
					model.UpdatedAt = DateTime.Now;
					_context.Update(model);
					await _context.SaveChangesAsync();
					TempData["SuccessMessage"] = $"Đã cập nhật nhà cung cấp: {model.Name} (Mã: {model.SupplierCode}).";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SupplierExists(model.SupplierId))
						return NotFound();
					else
					{
						TempData["ErrorMessage"] = "Có lỗi đồng bộ dữ liệu khi cập nhật nhà cung cấp.";
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Title = "Chỉnh sửa nhà cung cấp";
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid id)
		{
			var supplier = await _context.Suppliers.FindAsync(id);
			if (supplier != null)
			{
				_context.Suppliers.Remove(supplier);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> TransactionHistory(Guid id, int poPage = 1, int invoicePage = 1, int stockPage = 1)
		{
			var supplier = await _context.Suppliers
				.AsNoTracking()
				.FirstOrDefaultAsync(s => s.SupplierId == id);

			if (supplier == null)
				return NotFound();

			// Lấy danh sách Purchase Orders
			var poQuery = _context.PurchaseOrders
				.AsNoTracking()
				.Include(po => po.Branch)
				.Include(po => po.CreatedByStaff)
				.Where(po => po.SupplierId == id)
				.OrderByDescending(po => po.OrderedAt);

			var poTotalItems = await poQuery.CountAsync();
			var purchaseOrders = await poQuery
				.Skip((poPage - 1) * PageSize)
				.Take(PageSize)
				.ToListAsync();

			// Lấy danh sách Supplier Invoices
			var invoiceQuery = _context.SupplierInvoices
				.AsNoTracking()
				.Include(inv => inv.LinkedPurchaseOrder)
				.Where(inv => inv.SupplierId == id)
				.OrderByDescending(inv => inv.InvoiceDate);

			var invoiceTotalItems = await invoiceQuery.CountAsync();
			var supplierInvoices = await invoiceQuery
				.Skip((invoicePage - 1) * PageSize)
				.Take(PageSize)
				.ToListAsync();

				// Lấy danh sách Stock Transactions (Giao dịch nhập/xuất kho)
			var stockQuery = _context.StockTransactions
				.AsNoTracking()
				.Include(st => st.Item)
				.Where(st => st.SupplierId == id)
				.OrderByDescending(st => st.CreatedAt);

			var stockTotalItems = await stockQuery.CountAsync();
			var stockTransactions = await stockQuery
				.Skip((stockPage - 1) * PageSize)
				.Take(PageSize)
				.ToListAsync();
	

			ViewBag.Supplier = supplier;
			ViewBag.PurchaseOrders = purchaseOrders;
			ViewBag.SupplierInvoices = supplierInvoices;
				ViewBag.StockTransactions = stockTransactions;
			ViewBag.POCurrentPage = poPage;
			ViewBag.POTotalPages = (int)Math.Ceiling(poTotalItems / (double)PageSize);
			ViewBag.InvoiceCurrentPage = invoicePage;
			ViewBag.InvoiceTotalPages = (int)Math.Ceiling(invoiceTotalItems / (double)PageSize);
				ViewBag.StockCurrentPage = stockPage;
			ViewBag.StockTotalPages = (int)Math.Ceiling(stockTotalItems / (double)PageSize);
			ViewBag.Title = $"Lịch sử giao dịch - {supplier.Name}";
			
			return View();
		}

		private bool SupplierExists(Guid id)
		{
			return _context.Suppliers.Any(e => e.SupplierId == id);
		}
	}
}
