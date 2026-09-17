using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using QuanLiKhoHang.Models.ViewModels;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý kho hàng
    /// </summary>
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hiển thị danh sách vật phẩm kho (Quản lí vật phẩm)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? status, int page = 1)
        {
            var query = from i in _context.InventoryItems
                        .Include(i => i.StorageLocation)
                        .Include(i => i.StockTransactions)
                        .AsNoTracking()
                        select i;

            // Áp dụng bộ lọc tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = from i in query
                        where i.Name.Contains(search) ||
                              (i.Sku != null && i.Sku.Contains(search))
                        select i;
            }

            // Áp dụng bộ lọc trạng thái
            if (!string.IsNullOrEmpty(status))
            {
                query = status.ToLower() switch
                {
                    "active" => from i in query where i.IsActive select i,
                    "inactive" => from i in query where !i.IsActive select i,
                    "low" => from i in query where i.IsActive && i.CurrentQuantity > 0 && i.CurrentQuantity <= i.ReorderLevel select i,
                    "outofstock" => from i in query where i.IsActive && i.CurrentQuantity <= 0 select i,
                    _ => query
                };
            }

            // Sắp xếp: ưu tiên mặt hàng sắp hết trước
            var items = await query
                .OrderBy(i => i.CurrentQuantity <= i.ReorderLevel ? 0 : 1)
                .ThenBy(i => i.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var totalItems = await query.CountAsync();

            ViewBag.Items = items;                 // List<InventoryItem>
            ViewBag.CurrentPage = page;            // int
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.StatusFilter = status ?? "";
            ViewBag.Title = "Quản lí vật phẩm kho";
            return View();
        }

        /// <summary>
        /// Hiển thị danh sách vật phẩm để nhập kho
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> StockInList(string? search, int page = 1)
        {
            var query = from i in _context.InventoryItems
                        .Include(i => i.StorageLocation)
                        .AsNoTracking()
                        where i.IsActive
                        select i;

            if (!string.IsNullOrEmpty(search))
            {
                query = from i in query
                        where i.Name.Contains(search) ||
                              (i.Sku != null && i.Sku.Contains(search))
                        select i;
            }

            var items = await query
                .OrderBy(i => i.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var totalItems = await query.CountAsync();

            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.Title = "Nhập kho";
            return View();
        }

        /// <summary>
        /// Hiển thị danh sách vật phẩm để xuất kho
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> StockOutList(string? search, int page = 1)
        {
            var query = from i in _context.InventoryItems
                        .Include(i => i.StorageLocation)
                        .Include(i => i.StockTransactions)
                        .AsNoTracking()
                        where i.IsActive
                        select i;

            if (!string.IsNullOrEmpty(search))
            {
                query = from i in query
                        where i.Name.Contains(search) ||
                              (i.Sku != null && i.Sku.Contains(search))
                        select i;
            }

            var items = await query
                .OrderBy(i => i.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var totalItems = await query.CountAsync();

            // Sử dụng giá trị CurrentQuantity đã được lưu trong bảng InventoryItems
            // (không tính lại từ StockTransactions để tránh không đồng bộ và chi phí truy vấn)

            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.Title = "Xuất kho";
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết mặt hàng tồn kho
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _context.InventoryItems
                .Include(i => i.StorageLocation) // Include StorageLocation for display
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
                return NotFound();

            // Tải các giao dịch gần đây một cách riêng biệt và sắp xếp chúng
            var recentTransactions = await _context.StockTransactions
                .Where(st => st.ItemId == id)
                .OrderByDescending(st => st.CreatedAt)
                .Take(10) // Lấy 10 giao dịch gần nhất
                .ToListAsync();

            ViewBag.Item = item;                   // InventoryItem
            ViewBag.RecentTransactions = recentTransactions; // List<StockTransaction>
            ViewBag.Title = $"Chi tiết - {item.Name}";
            return View(item);
        }

        /// <summary>
        /// Hiển thị form tạo mặt hàng mới
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var storageLocations = await _context.StorageLocations
                .Where(sl => sl.IsActive)
                .OrderBy(sl => sl.Name)
                .ToListAsync();
            var suppliers = await _context.Set<QuanLiKhoHang.Models.Entities.Supplier>()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            ViewBag.StorageLocations = storageLocations;
            ViewBag.Suppliers = suppliers;
            ViewBag.Title = "Thêm vật phẩm mới";
            return View();
        }

        /// <summary>
        /// Tải file mẫu CSV để người dùng nhập/xuất hàng bằng Excel/CSV
        /// </summary>
        [HttpGet]
        public IActionResult DownloadImportTemplate()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "InventoryImportTemplate.xlsx");
            if (!System.IO.File.Exists(filePath))
            {
                // Tạo file mẫu nếu chưa có
                // EPPlusLicenseContext được thiết lập trong .csproj nên không cần thiết lập thủ công
                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Inventory Items");
                    worksheet.Cells[1, 1].Value = "SKU";
                    worksheet.Cells[1, 2].Value = "Tên mặt hàng";
                    worksheet.Cells[1, 3].Value = "Đơn vị";
                    worksheet.Cells[1, 4].Value = "Giá nhập";
                    worksheet.Cells[1, 5].Value = "Mã nhà cung cấp (SupplierCode)";
                    worksheet.Cells[1, 6].Value = "Tên vị trí lưu trữ";
                    worksheet.Cells[1, 7].Value = "Số lượng nhập";

                    // Định dạng header
                    using (var range = worksheet.Cells["A1:G1"])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // Auto fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Lưu file
                    var fileBytes = package.GetAsByteArray();
                    var directory = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    System.IO.File.WriteAllBytes(filePath, fileBytes);
                }
            }
            
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InventoryImportTemplate.xlsx");
        }

        /// <summary>
        /// Hiển thị form nhập file Excel
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Import()
        {
            ViewBag.Title = "Nhập vật phẩm từ Excel";
            return View();
        }

        /// <summary>
        /// Xử lý nhập file Excel - tạo PurchaseOrder, PurchaseOrderLine, SupplierInvoice, StockTransaction và cập nhật InventoryItem
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn một file Excel để nhập.";
                return RedirectToAction("Import");
            }

            if (!file.FileName.EndsWith(".xlsx"))
            {
                TempData["ErrorMessage"] = "File không đúng định dạng Excel (.xlsx).";
                return RedirectToAction("Import");
            }

            // Lấy BranchId từ claims của user
            var branchIdClaim = User.FindFirst("BranchId")?.Value;
            if (string.IsNullOrEmpty(branchIdClaim) || !Guid.TryParse(branchIdClaim, out var branchId))
            {
                TempData["ErrorMessage"] = "Không thể xác định chi nhánh của bạn.";
                return RedirectToAction("Import");
            }

            // Lấy StaffId từ claims (nếu cần)
            var staffIdClaim = User.FindFirst("StaffId")?.Value;
            Guid? createdByStaffId = null;
            if (!string.IsNullOrEmpty(staffIdClaim) && Guid.TryParse(staffIdClaim, out var staffId))
            {
                createdByStaffId = staffId;
            }

            var importedCount = 0;
            var updatedCount = 0;
            var errors = new List<string>();

            // Nhóm dữ liệu theo Supplier để tạo một PO mỗi nhà cung cấp
            var supplierGroups = new Dictionary<Guid, (Supplier supplier, List<(InventoryItem item, decimal quantity, decimal costPerUnit)> items)>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new OfficeOpenXml.ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            TempData["ErrorMessage"] = "File Excel không có worksheet nào.";
                            return RedirectToAction("Import");
                        }

                        // Bắt đầu từ hàng thứ 2 (bỏ qua header)
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            try
                            {
                                var sku = worksheet.Cells[row, 1].GetValue<string>();
                                var name = worksheet.Cells[row, 2].GetValue<string>();
                                var unit = worksheet.Cells[row, 3].GetValue<string>();
                                var costPerUnit = worksheet.Cells[row, 4].GetValue<decimal>();
                                var supplierCode = worksheet.Cells[row, 5].GetValue<string>();
                                var storageLocationName = worksheet.Cells[row, 6].GetValue<string>();
                                var importQuantity = worksheet.Cells[row, 7].GetValue<decimal>();

                                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(unit))
                                {
                                    errors.Add($"Hàng {row}: Tên mặt hàng hoặc Đơn vị không được để trống.");
                                    continue;
                                }

                                if (string.IsNullOrEmpty(sku))
                                {
                                    errors.Add($"Hàng {row}: SKU không được để trống.");
                                    continue;
                                }

                                if (importQuantity <= 0)
                                {
                                    errors.Add($"Hàng {row}: Số lượng nhập phải > 0.");
                                    continue;
                                }

                                if (costPerUnit <= 0)
                                {
                                    errors.Add($"Hàng {row}: Giá nhập phải > 0.");
                                    continue;
                                }

                                // YÊU CẦU: Mã nhà cung cấp bắt buộc
                                if (string.IsNullOrWhiteSpace(supplierCode))
                                {
                                    errors.Add($"Hàng {row}: Mã nhà cung cấp (SupplierCode) không được để trống.");
                                    continue;
                                }

                                // Tìm Supplier theo SupplierCode
                                var supplier = await _context.Set<QuanLiKhoHang.Models.Entities.Supplier>()
                                    .FirstOrDefaultAsync(s => s.SupplierCode == supplierCode);
                                if (supplier == null)
                                {
                                    errors.Add($"Hàng {row}: Không tìm thấy nhà cung cấp với mã '{supplierCode}'.");
                                    continue;
                                }

                                // Tìm StorageLocation
                                Guid? storageLocationId = null;
                                if (!string.IsNullOrEmpty(storageLocationName))
                                {
                                    var storageLocation = await _context.StorageLocations
                                        .FirstOrDefaultAsync(sl => sl.Name.Equals(storageLocationName) );
                                    if (storageLocation == null)
                                    {
                                        errors.Add($"Hàng {row}: Vị trí lưu trữ '{storageLocationName}' không tồn tại. Vui lòng tạo trước.");
                                        continue;
                                    }
                                    storageLocationId = storageLocation.StorageLocationId;
                                }

                                // Đối chiếu theo SKU + SupplierId để quyết định cập nhật hay tạo mới
                                var existingItem = await (
                                    from i in _context.InventoryItems
                                    where i.Sku == sku && i.SupplierId == supplier.SupplierId
                                    select i
                                ).FirstOrDefaultAsync();

                                InventoryItem inventoryItem;
                                if (existingItem != null)
                                {
                                    // Cập nhật mặt hàng hiện có
                                    existingItem.Name = name;
                                    existingItem.Unit = unit;
                                    // Tính giá vốn bình quân
                                    var oldQty = existingItem.CurrentQuantity;
                                    var oldCost = existingItem.CostPerUnit;
                                    var newQtyTotal = oldQty + importQuantity;
                                    existingItem.CurrentQuantity = newQtyTotal;
                                    existingItem.CostPerUnit = oldQty <= 0 ? costPerUnit : ((oldQty * oldCost) + (importQuantity * costPerUnit)) / newQtyTotal;
                                    existingItem.SupplierId = supplier.SupplierId;
                                    existingItem.StorageLocationId = storageLocationId;
                                    existingItem.UpdatedAt = DateTime.Now;
                                    inventoryItem = existingItem;
                                    updatedCount++;
                                }
                                else
                                {
                                    // Thêm mặt hàng mới
                                    inventoryItem = new InventoryItem
                                    {
                                        ItemId = Guid.NewGuid(),
                                        Sku = sku,
                                        Name = name,
                                        Unit = unit,
                                        CurrentQuantity = importQuantity,
                                        ReorderLevel = 0,
                                        ReorderQuantity = 0,
                                        CostPerUnit = costPerUnit,
                                        ShelfLifeDays = null,
                                        StorageCondition = StorageCondition.RoomTemperature,
                                        IsActive = true,
                                        SupplierId = supplier.SupplierId,
                                        StorageLocationId = storageLocationId,
                                        CreatedAt = DateTime.Now
                                    };
                                    _context.InventoryItems.Add(inventoryItem);
                                    importedCount++;
                                }

                                // Nhóm theo Supplier để tạo PO
                                if (!supplierGroups.ContainsKey(supplier.SupplierId))
                                {
                                    supplierGroups[supplier.SupplierId] = (supplier, new List<(InventoryItem, decimal, decimal)>());
                                }
                                supplierGroups[supplier.SupplierId].items.Add((inventoryItem, importQuantity, costPerUnit));
                            }
                            catch (Exception ex)
                            {
                                errors.Add($"Hàng {row}: Lỗi - {ex.Message}");
                            }
                        }
                    }
                }

                // Lưu InventoryItems trước (outside transaction để có sẵn ID)
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi lưu vật phẩm tồn kho: {ex.Message}. Chi tiết: {ex.InnerException?.Message}";
                    return RedirectToAction("Import");
                }

                // Bắt đầu transaction để đảm bảo atomic (tất cả hoặc không)
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Tạo PurchaseOrder, PurchaseOrderLine, SupplierInvoice, StockTransaction cho mỗi Supplier
                    foreach (var (supplierId, (supplier, items)) in supplierGroups)
                    {
                        try
                        {
                            // Tạo PurchaseOrder
                            var poId = Guid.NewGuid();
                            var poNumber = $"PO-{DateTime.Now:yyyyMMdd}-{poId.ToString().Substring(0, 8).ToUpper()}";
                            var totalPOAmount = items.Sum(x => x.quantity * x.costPerUnit);

                            var purchaseOrder = new PurchaseOrder
                            {
                                PurchaseOrderId = poId,
                                PONumber = poNumber,
                                SupplierId = supplierId,
                                BranchId = branchId,
                                CreatedByStaffId = createdByStaffId,
                                OrderedAt = DateTime.Now,
                                ExpectedDeliveryDate = DateTime.Now.AddDays(7),
                                Status = PurchaseOrderStatus.Completed,
                                TotalAmount = totalPOAmount,
                                Currency = "VND",
                                Notes = $"Nhập hàng từ file Excel ngày {DateTime.Now:dd/MM/yyyy}",
                                CreatedAt = DateTime.Now
                            };
                            _context.Set<PurchaseOrder>().Add(purchaseOrder);

                            // Tạo PurchaseOrderLine cho mỗi item
                            foreach (var (inventoryItem, quantity, cost) in items)
                            {
                                var poLine = new PurchaseOrderLine
                                {
                                    POLineId = Guid.NewGuid(),
                                    PurchaseOrderId = poId,
                                    ItemId = inventoryItem.ItemId,
                                    QuantityOrdered = quantity,
                                    QuantityReceived = quantity,
                                    UnitCost = cost,
                                    LineTotal = quantity * cost,
                                    CreatedAt = DateTime.Now
                                };
                                _context.Set<PurchaseOrderLine>().Add(poLine);
                            }

                            // Tạo StockTransaction cho mỗi item
                            foreach (var (inventoryItem, quantity, cost) in items)
                            {
                                _context.StockTransactions.Add(new StockTransaction
                                {
                                    StockTransactionId = Guid.NewGuid(),
                                    ItemId = inventoryItem.ItemId,
                                    Quantity = quantity,
                                    TransactionType = StockTransactionType.PurchaseIn,
                                    SupplierId = supplierId,
                                    UnitCost = cost,
                                    TotalCost = cost * quantity,
                                    CreatedAt = DateTime.Now,
                                    CreatedByStaffId = createdByStaffId
                                });
                            }

                            // Tạo SupplierInvoice liên kết với PurchaseOrder
                            var invoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{poId.ToString().Substring(0, 8).ToUpper()}";
                            var supplierInvoice = new SupplierInvoice
                            {
                                SupplierInvoiceId = Guid.NewGuid(),
                                SupplierId = supplierId,
                                InvoiceNumber = invoiceNumber,
                                InvoiceDate = DateOnly.FromDateTime(DateTime.Now),
                                DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                                TotalAmount = totalPOAmount,
                                BalanceDue = totalPOAmount,
                                Status = "Pending",
                                LinkedPOId = poId,
                                CreatedAt = DateTime.Now
                            };
                            _context.Set<SupplierInvoice>().Add(supplierInvoice);
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Nhà cung cấp {supplier.Name}: Lỗi tạo PO/Invoice - {ex.Message}");
                        }
                    }

                    // Lưu tất cả PO, POLine, SupplierInvoice, StockTransaction
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException dbEx)
                    {
                        // Log chi tiết lỗi DB
                        var errorDetails = string.Join("; ", _context.ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                            .Select(e => $"{e.Entity.GetType().Name} - {string.Join(", ", e.GetDatabaseValues()?.Properties.Select(p => p.Name) ?? new string[0])}"));
                        
                        errors.Add($"Lỗi ràng buộc DB: {dbEx.Message}. Chi tiết: {errorDetails}");
                        throw; // Re-throw để rollback transaction
                    }

                    // Commit transaction
                    await transaction.CommitAsync();

                    if (errors.Any())
                    {
                        TempData["ErrorMessage"] = $"Đã nhập thành công {importedCount} vật phẩm mới, cập nhật {updatedCount} vật phẩm. Có {errors.Count} lỗi: {string.Join("; ", errors)}";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = $"Đã nhập thành công {importedCount} vật phẩm mới, cập nhật {updatedCount} vật phẩm và tạo {supplierGroups.Count} đơn hàng từ file Excel.";
                    }
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    TempData["ErrorMessage"] = $"Lỗi khi lưu PO/Invoice: {ex.Message}. Chi tiết: {ex.InnerException?.Message}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi xử lý file: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Xử lý tạo mặt hàng mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryItem model)
        {
            ModelState.Remove("ItemId");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");

            if (ModelState.IsValid)
            {
                model.ItemId = Guid.NewGuid();
                model.CreatedAt = DateTime.Now;
                _context.InventoryItems.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm vật phẩm thành công!";
                return RedirectToAction("Index");
            }

            var storageLocations = await _context.StorageLocations
                .Where(sl => sl.IsActive)
                .OrderBy(sl => sl.Name)
                .ToListAsync();

            var suppliers = await _context.Set<QuanLiKhoHang.Models.Entities.Supplier>()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            ViewBag.StorageLocations = storageLocations;
            ViewBag.Suppliers = suppliers;
            ViewBag.Title = "Thêm vật phẩm mới";
            return View(model);
        }

        /// <summary>
        /// Hiển thị báo cáo giao dịch hàng ngày (nhập + xuất)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DailyReport(DateTime? date)
        {
            var reportDate = date?.Date ?? DateTime.Now.Date;

            // Lấy tất cả giao dịch nhập kho trong ngày
            var importTransactions = await _context.StockTransactions
                .Include(st => st.Item)
                .Where(st => st.TransactionType == StockTransactionType.PurchaseIn &&
                             st.CreatedAt.Date == reportDate)
                .OrderByDescending(st => st.CreatedAt)
                .ToListAsync();

            // Lấy tất cả giao dịch xuất kho trong ngày
            var exportTransactions = await _context.StockTransactions
                .Include(st => st.Item)
                .Where(st => st.TransactionType == StockTransactionType.SaleOut &&
                             st.CreatedAt.Date == reportDate)
                .OrderByDescending(st => st.CreatedAt)
                .ToListAsync();

            // Tính tổng
            var totalImport = importTransactions.Sum(st => st.Quantity);
            var totalExport = Math.Abs(exportTransactions.Sum(st => st.Quantity));

            ViewBag.ReportDate = reportDate;
            ViewBag.ImportTransactions = importTransactions;
            ViewBag.ExportTransactions = exportTransactions;
            ViewBag.TotalImport = totalImport;
            ViewBag.TotalExport = totalExport;
            ViewBag.Title = $"Báo cáo giao dịch hàng ngày - {reportDate:dd/MM/yyyy}";
            
            return View();
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa mặt hàng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _context.InventoryItems
                .Include(i => i.StorageLocation)
                .FirstOrDefaultAsync(i => i.ItemId == id);
            if (item == null)
                return NotFound();

            var storageLocations = await _context.StorageLocations
                .Where(sl => sl.IsActive)
                .OrderBy(sl => sl.Name)
                .ToListAsync();
            var suppliers = await _context.Set<QuanLiKhoHang.Models.Entities.Supplier>()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            ViewBag.StorageLocations = storageLocations;
            ViewBag.Suppliers = suppliers; // Thêm danh sách nhà cung cấp vào ViewBag
            ViewBag.Title = $"Chỉnh sửa - {item.Name}";
            return View(item);
        }

        /// <summary>
        /// Xử lý chỉnh sửa mặt hàng
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InventoryItem model)
        {
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                // Lấy entity đang được track để cập nhật từng trường hợp lệ (tránh ghi đè CurrentQuantity)
                var existingItem = await _context.InventoryItems
                    .FirstOrDefaultAsync(i => i.ItemId == model.ItemId);

                if (existingItem == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy vật phẩm!";
                    return RedirectToAction("Index");
                }

                // Chỉ cập nhật các trường cấu hình; KHÔNG đụng tới CurrentQuantity/CreatedAt
                existingItem.Sku = model.Sku;
                existingItem.Name = model.Name;
                existingItem.Unit = model.Unit;
                existingItem.ReorderLevel = model.ReorderLevel;
                existingItem.ReorderQuantity = model.ReorderQuantity;
                existingItem.CostPerUnit = model.CostPerUnit;
                existingItem.ShelfLifeDays = model.ShelfLifeDays;
                existingItem.StorageCondition = model.StorageCondition;
                existingItem.IsActive = model.IsActive; // Chỉ đổi trạng thái, không tự trừ tồn
                existingItem.SupplierId = model.SupplierId;
                existingItem.StorageLocationId = model.StorageLocationId;
                existingItem.ImageUrl = model.ImageUrl;

                existingItem.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật vật phẩm thành công!";
                return RedirectToAction("Index");
            }

            var storageLocations = await _context.StorageLocations
                .Where(sl => sl.IsActive)
                .OrderBy(sl => sl.Name)
                .ToListAsync();

            ViewBag.StorageLocations = storageLocations;
            ViewBag.Title = $"Chỉnh sửa - {model.Name}";
            return View(model);
        }

        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
                return NotFound();

            // Kiểm tra xem có giao dịch nào sử dụng mặt hàng này không
            var hasTransactions = await _context.StockTransactions
                .AnyAsync(st => st.ItemId == id);

            if (hasTransactions)
            {
                TempData["ErrorMessage"] = "Không thể xóa vật phẩm đã có giao dịch!";
                return RedirectToAction("Index");
            }

            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa vật phẩm thành công!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Hiển thị form nhập kho
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> StockIn(Guid id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
                return NotFound();

            ViewBag.Item = item;
            ViewBag.Title = $"Nhập kho - {item.Name}";
            return View();
        }

        /// <summary>
        /// Xử lý nhập kho
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StockIn(Guid id, int quantity, string? notes)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("quantity", "Số lượng phải lớn hơn 0");
                var itemForView = await _context.InventoryItems.FindAsync(id);
                ViewBag.Item = itemForView;
                ViewBag.Title = $"Nhập kho - {itemForView?.Name}";
                return View();
            }

            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
                return NotFound();

            // Tạo giao dịch nhập kho (gồm đơn giá và thành tiền)
            // Sử dụng CostPerUnit hiện tại của mặt hàng làm đơn giá giao dịch
            var currentUnitCost = inventoryItem.CostPerUnit;

            if (currentUnitCost <= 0)
            {
                ModelState.AddModelError("cost", "Giá vốn hiện tại chưa được thiết lập cho mặt hàng này. Vui lòng cập nhật giá (qua Import hoặc Edit) trước khi nhập kho.");
                ViewBag.Item = inventoryItem;
                ViewBag.Title = $"Nhập kho - {inventoryItem.Name}";
                return View();
            }

            var transaction = new StockTransaction
            {
                StockTransactionId = Guid.NewGuid(),
                ItemId = id,
                Quantity = quantity, // Số dương cho nhập kho
                TransactionType = StockTransactionType.PurchaseIn,
                Note = notes,
                UnitCost = currentUnitCost,
                TotalCost = currentUnitCost * quantity,
                SupplierId = inventoryItem.SupplierId,
                CreatedAt = DateTime.Now,
                CreatedByStaffId = null // Có thể lấy từ User.Identity sau này
            };

            _context.StockTransactions.Add(transaction);

            // Cập nhật CurrentQuantity; giá vốn giữ nguyên vì đơn giá dùng CostPerUnit hiện có
            inventoryItem.CurrentQuantity += quantity;
            inventoryItem.UpdatedAt = DateTime.Now;
            _context.InventoryItems.Update(inventoryItem);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Nhập kho thành công! Đã thêm {quantity} {inventoryItem.Unit} của {inventoryItem.Name}";
            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Hiển thị form xuất kho
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> StockOut(Guid id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
                return NotFound();

            // Dùng giá trị CurrentQuantity đã lưu trong DB (không tính lại từ StockTransactions)
            var currentQuantity = item.CurrentQuantity;

            if (currentQuantity <= 0)
            {
                TempData["ErrorMessage"] = "Không thể xuất kho vì mặt hàng đã hết!";
                return RedirectToAction("Details", new { id });
            }

            ViewBag.Item = item;
            ViewBag.CurrentQuantity = currentQuantity;
            ViewBag.Title = $"Xuất kho - {item.Name}";
            return View();
        }

        /// <summary>
        /// Kiểm kê hàng ngày - xem danh sách tất cả giao dịch trong ngày
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Stocktaking(DateTime? date, int page = 1)
        {
            // Nếu không chỉ định ngày, sử dụng ngày hôm nay
            var stocktakingDate = date?.Date ?? DateTime.Now.Date;

            // Lấy tất cả giao dịch trong ngày
            var query = from st in _context.StockTransactions
                        .Include(st => st.Item)
                        .AsNoTracking()
                        where st.CreatedAt.Date == stocktakingDate
                        select st;

            var transactionType = query
                .OrderByDescending(st => st.CreatedAt)
                .AsAsyncEnumerable();

            var totalItems = await query.CountAsync();
            var transactions = await query
                .OrderByDescending(st => st.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Tính tổng nhập/xuất trong ngày
            var totalImport = await _context.StockTransactions
                .Where(st => st.CreatedAt.Date == stocktakingDate && st.TransactionType == StockTransactionType.PurchaseIn)
                .SumAsync(st => st.Quantity);

            var totalExport = await _context.StockTransactions
                .Where(st => st.CreatedAt.Date == stocktakingDate && st.TransactionType == StockTransactionType.SaleOut)
                .SumAsync(st => Math.Abs(st.Quantity));

            ViewBag.Transactions = transactions;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.StocktakingDate = stocktakingDate.ToString("dd/MM/yyyy");
            ViewBag.TotalImport = totalImport;
            ViewBag.TotalExport = totalExport;
            ViewBag.Title = $"Kiểm kê hàng ngày - {stocktakingDate:dd/MM/yyyy}";
            return View();
        }

        /// <summary>
        /// Xử lý xuất kho
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StockOut(Guid id, int quantity, string? notes)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("quantity", "Số lượng phải lớn hơn 0");
                var item = await _context.InventoryItems.FindAsync(id);
                ViewBag.Item = item;
                ViewBag.CurrentQuantity = item?.CurrentQuantity ?? 0;
                ViewBag.Title = $"Xuất kho - {item?.Name}";
                return View();
            }

            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
                return NotFound();

            // Dùng giá trị CurrentQuantity đã lưu trong DB (không tính lại từ StockTransactions)
            var currentQuantity = inventoryItem.CurrentQuantity;

            if (quantity > currentQuantity)
            {
                ModelState.AddModelError("quantity", $"Không thể xuất {quantity} {inventoryItem.Unit}. Chỉ còn {currentQuantity} {inventoryItem.Unit} trong kho!");
                ViewBag.Item = inventoryItem;
                ViewBag.CurrentQuantity = currentQuantity;
                ViewBag.Title = $"Xuất kho - {inventoryItem.Name}";
                return View();
            }

            // Tạo giao dịch xuất kho
            var transaction = new StockTransaction
            {
                StockTransactionId = Guid.NewGuid(),
                ItemId = id,
                Quantity = -quantity, // Số âm cho xuất kho
                TransactionType = StockTransactionType.SaleOut,
                Note = notes,
                CreatedAt = DateTime.Now,
                CreatedByStaffId = null // Có thể lấy từ User.Identity sau này
            };

            _context.StockTransactions.Add(transaction);

            // Cập nhật CurrentQuantity trong InventoryItems
            inventoryItem.CurrentQuantity -= quantity;
            inventoryItem.UpdatedAt = DateTime.Now;
            _context.InventoryItems.Update(inventoryItem);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Xuất kho thành công! Đã xuất {quantity} {inventoryItem.Unit} của {inventoryItem.Name}";
            return RedirectToAction("Details", new { id });
        }
    }
}
