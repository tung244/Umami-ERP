using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý phân tích và báo cáo (Analytics & Reporting)
    /// Triển khai theo Analytics_Guide.md - Version 2.0
    /// </summary>
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region View Actions

        /// <summary>
        /// Hiển thị trang báo cáo Doanh thu & Lợi nhuận
        /// </summary>
        [HttpGet]
        public IActionResult Revenue()
        {
            ViewBag.Branches = _context.Branches.AsNoTracking().Where(b => b.IsActive).ToList();
            ViewBag.Categories = _context.Categories.AsNoTracking().Where(c => c.Active).ToList();
            return View();
        }

        /// <summary>
        /// Hiển thị trang báo cáo Khách hàng
        /// </summary>
        [HttpGet]
        public IActionResult Customer()
        {
            return View();
        }

        /// <summary>
        /// Hiển thị trang báo cáo Kho & Công nợ
        /// </summary>
        [HttpGet]
        public IActionResult Inventory()
        {
            return View();
        }

        /// <summary>
        /// Hiển thị trang báo cáo Phân tích (gộp Xu hướng + Hiệu suất)
        /// </summary>
        [HttpGet]
        public IActionResult Trends()
        {
            ViewBag.Branches = _context.Branches.AsNoTracking().Where(b => b.IsActive).ToList();
            return View();
        }

        #endregion

        #region A. Doanh thu & Lợi nhuận

        /// <summary>
        /// A1. REP-01: Tổng quan doanh thu theo thời gian
        /// Line chart với KPI cards, MoM/YoY comparison
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RevenueTimeSeries(DateTime? startDate, DateTime? endDate, Guid? branchId, string compareWith = "none")
        {
            var start = (startDate ?? DateTime.Today.AddDays(-30)).Date;
            var end = (endDate ?? DateTime.Today).Date;
            var endExclusive = end.AddDays(1);

            // Query orders đã thanh toán
            var ordersQuery = from o in _context.Orders.AsNoTracking()
                              where o.PlacedAt >= start &&
                                    o.PlacedAt < endExclusive &&
                                    o.Status == OrderStatus.Paid
                              select o;

            if (branchId.HasValue)
            {
                ordersQuery = from o in ordersQuery where o.BranchId == branchId.Value select o;
            }

            var orders = await ordersQuery.ToListAsync();

            // Tính refunds trong kỳ
            var refundsQuery = from r in _context.Refunds.AsNoTracking()
                               join o in ordersQuery on r.OrderId equals o.OrderId
                               where r.RefundDate >= start && r.RefundDate < endExclusive
                               select r;
            var totalRefunds = await refundsQuery.SumAsync(r => (decimal?)r.Amount) ?? 0m;

            // Group theo ngày
            var dailyRevenue = orders
                .GroupBy(o => o.PlacedAt.Date)
                .Select(g => new
                {
                    date = g.Key,
                    revenue = g.Sum(x => x.TotalAmount),
                    orders = g.Count(),
                    avgOrderValue = g.Average(x => x.TotalAmount)
                })
                .OrderBy(x => x.date)
                .ToList();

            // Điền đầy đủ các ngày (kể cả ngày 0 doanh thu)
            var allDays = Enumerable.Range(0, (end - start).Days + 1)
                .Select(i => start.AddDays(i))
                .Select(d =>
                {
                    var found = dailyRevenue.FirstOrDefault(r => r.date == d);
                    return new
                    {
                        date = d.ToString("yyyy-MM-dd"),
                        revenue = found?.revenue ?? 0m,
                        orders = found?.orders ?? 0,
                        avgOrderValue = found?.avgOrderValue ?? 0m
                    };
                })
                .ToList();

            var totalRevenue = allDays.Sum(x => x.revenue);
            var totalOrders = allDays.Sum(x => x.orders);
            var netRevenue = totalRevenue - totalRefunds;
            var aov = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            // Tính so sánh với kỳ trước (MoM hoặc YoY)
            object? comparison = null;
            if (compareWith == "mom")
            {
                var prevStart = start.AddMonths(-1);
                var prevEnd = end.AddMonths(-1);
                var prevRevenue = await (
                    from o in _context.Orders.AsNoTracking()
                    where o.PlacedAt >= prevStart && o.PlacedAt < prevEnd.AddDays(1) && o.Status == OrderStatus.Paid
                    select o.TotalAmount
                ).SumAsync();

                var changePercent = prevRevenue > 0 ? ((totalRevenue - prevRevenue) / prevRevenue) * 100 : 0;
                comparison = new { period = "MoM", previousRevenue = prevRevenue, changePercent = Math.Round(changePercent, 2) };
            }
            else if (compareWith == "yoy")
            {
                var prevStart = start.AddYears(-1);
                var prevEnd = end.AddYears(-1);
                var prevRevenue = await (
                    from o in _context.Orders.AsNoTracking()
                    where o.PlacedAt >= prevStart && o.PlacedAt < prevEnd.AddDays(1) && o.Status == OrderStatus.Paid
                    select o.TotalAmount
                ).SumAsync();

                var changePercent = prevRevenue > 0 ? ((totalRevenue - prevRevenue) / prevRevenue) * 100 : 0;
                comparison = new { period = "YoY", previousRevenue = prevRevenue, changePercent = Math.Round(changePercent, 2) };
            }

            return Json(new
            {
                success = true,
                period = new { start = start.ToString("yyyy-MM-dd"), end = end.ToString("yyyy-MM-dd") },
                kpi = new
                {
                    totalRevenue,
                    netRevenue,
                    totalRefunds,
                    totalOrders,
                    aov = Math.Round(aov, 2)
                },
                comparison,
                data = allDays
            });
        }

        /// <summary>
        /// A2. REP-02: Top món theo doanh thu & lợi nhuận
        /// Horizontal bar chart với breakdown volume + profit
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TopDishesAnalysis(DateTime? startDate, DateTime? endDate, Guid? branchId, Guid? categoryId, string sortBy = "revenue", int limit = 10)
        {
            var start = (startDate ?? DateTime.Today.AddDays(-30)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            // Query OrderItems
            var query = from oi in _context.OrderItems.AsNoTracking()
                        join o in _context.Orders.AsNoTracking() on oi.OrderId equals o.OrderId
                        join d in _context.Dishes.AsNoTracking() on oi.DishId equals d.DishId
                        where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1) && o.Status != OrderStatus.Cancelled
                        select new { oi, d, o };

            if (branchId.HasValue)
            {
                query = from x in query where x.o.BranchId == branchId.Value select x;
            }

            if (categoryId.HasValue)
            {
                query = from x in query where x.d.CategoryId == categoryId.Value select x;
            }

            var orderItems = await query.ToListAsync();

            // Group và tính toán
            var dishStats = orderItems
                .GroupBy(x => new { x.d.DishId, x.d.Name })
                .Select(g =>
                {
                    var revenue = g.Sum(x => x.oi.LineTotal);
                    var quantity = g.Sum(x => x.oi.Quantity);

                    return new
                    {
                        dishId = g.Key.DishId,
                        dishName = g.Key.Name,
                        quantitySold = quantity,
                        revenue,
                        avgPrice = g.Average(x => x.oi.UnitPrice)
                    };
                })
                .ToList();

            // Sắp xếp theo tiêu chí
            var sortedDishes = sortBy switch
            {
                "quantity" => dishStats.OrderByDescending(d => d.quantitySold),
                _ => dishStats.OrderByDescending(d => d.revenue)
            };

            var topDishes = sortedDishes.Take(limit).ToList();
            var totalRevenue = dishStats.Sum(d => d.revenue);

            return Json(new
            {
                success = true,
                summary = new
                {
                    totalRevenue,
                    totalDishes = dishStats.Count,
                    sortBy
                },
                data = topDishes
            });
        }

        /// <summary>
        /// Phân tích doanh thu theo phương thức thanh toán
        /// Donut chart: Cash, Card, Transfer, E-wallet
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> PaymentMethodAnalysis(DateTime? startDate, DateTime? endDate, Guid? branchId)
        {
            var start = (startDate ?? DateTime.Today.AddDays(-30)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            // Query payments từ paid orders
            var query = from p in _context.Payments.AsNoTracking()
                        join o in _context.Orders.AsNoTracking() on p.OrderId equals o.OrderId
                        where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1)
                              && p.Status == PaymentStatus.Approved
                        select new { p, o };

            if (branchId.HasValue)
            {
                query = from x in query where x.o.BranchId == branchId.Value select x;
            }

            var payments = await query.ToListAsync();

            // Group by payment method
            var paymentStats = from x in payments
                               group x by x.p.PaymentMethod into g
                               select new
                               {
                                   method = g.Key.ToString(),
                                   amount = g.Sum(x => x.p.Amount)
                               };

            var statsList = paymentStats.ToList();
            var totalAmount = statsList.Sum(x => x.amount);

            var result = statsList.Select(x => new
            {
                method = x.method,
                amount = x.amount,
                percentage = totalAmount > 0 ? Math.Round((x.amount / totalAmount) * 100, 2) : 0
            }).ToList();

            return Json(new
            {
                success = true,
                data = result
            });
        }

        /// <summary>
        /// Phân tích doanh thu theo danh mục món ăn theo thời gian
        /// Stacked bar chart: Revenue by category over time
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CategoryRevenueAnalysis(DateTime? startDate, DateTime? endDate, Guid? branchId, string groupBy = "month")
        {
            var start = (startDate ?? DateTime.Today.AddMonths(-15)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            // Query OrderItems với join
            var query = from oi in _context.OrderItems.AsNoTracking()
                        join o in _context.Orders.AsNoTracking() on oi.OrderId equals o.OrderId
                        join d in _context.Dishes.AsNoTracking() on oi.DishId equals d.DishId
                        where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1) && o.Status != OrderStatus.Cancelled
                        select new { oi, o, d };

            if (branchId.HasValue)
            {
                query = from x in query where x.o.BranchId == branchId.Value select x;
            }

            var orderItems = await query.ToListAsync();

            // Group theo period và category
            var grouped = orderItems
                .GroupBy(x => new
                {
                    Period = groupBy == "week"
                        ? CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.o.PlacedAt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                        : x.o.PlacedAt.Month,
                    Year = x.o.PlacedAt.Year,
                    CategoryId = x.d.CategoryId
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Period,
                    g.Key.CategoryId,
                    Revenue = g.Sum(x => x.oi.LineTotal)
                })
                .ToList();

            // Lấy danh sách categories
            var categoryIds = grouped.Select(x => x.CategoryId).Where(x => x.HasValue).Distinct().ToList();
            var categories = await (from c in _context.Categories.AsNoTracking()
                                    where categoryIds.Contains(c.CategoryId)
                                    select c).ToListAsync();

            // Tạo periods
            var periods = grouped
                .Select(x => groupBy == "week"
                    ? $"{x.Year}-W{x.Period:D2}"
                    : $"{x.Year}-{x.Period:D2}")
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            // Tạo series cho từng category
            var seriesList = new List<object>();

            foreach (var cat in categories)
            {
                var categoryData = periods.Select(period =>
                {
                    var periodParts = period.Split('-');
                    var year = int.Parse(periodParts[0]);
                    var periodNum = groupBy == "week"
                        ? int.Parse(periodParts[1].Replace("W", ""))
                        : int.Parse(periodParts[1]);

                    var revenue = grouped
                        .Where(x => x.Year == year && x.Period == periodNum && x.CategoryId == cat.CategoryId)
                        .Sum(x => x.Revenue);

                    return revenue;
                }).ToList();

                seriesList.Add(new
                {
                    name = cat.Name,
                    data = categoryData
                });
            }

            // Thêm uncategorized nếu có
            var uncategorizedRevenue = grouped.Where(x => !x.CategoryId.HasValue).ToList();
            if (uncategorizedRevenue.Any())
            {
                var uncategorizedData = periods.Select(period =>
                {
                    var periodParts = period.Split('-');
                    var year = int.Parse(periodParts[0]);
                    var periodNum = groupBy == "week"
                        ? int.Parse(periodParts[1].Replace("W", ""))
                        : int.Parse(periodParts[1]);

                    var revenue = uncategorizedRevenue
                        .Where(x => x.Year == year && x.Period == periodNum)
                        .Sum(x => x.Revenue);

                    return revenue;
                }).ToList();

                seriesList.Add(new
                {
                    name = "Chưa phân loại",
                    data = uncategorizedData
                });
            }

            return Json(new
            {
                success = true,
                periods,
                series = seriesList
            });
        }

        /// <summary>
        /// A3. REP-04: Lợi nhuận (Gross & Net) với breakdown COGS/Opex
        /// Combo chart: bars (revenue/COGS/Opex) + line (profit margin)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ProfitAnalysis(DateTime? startDate, DateTime? endDate, Guid? branchId, string groupBy = "month")
        {
            var start = (startDate ?? DateTime.Today.AddMonths(-3)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            // Doanh thu
            var revenueQuery = from o in _context.Orders.AsNoTracking()
                               where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1) && o.Status == OrderStatus.Paid
                               select o;

            if (branchId.HasValue)
            {
                revenueQuery = from o in revenueQuery where o.BranchId == branchId.Value select o;
            }

            var orders = await revenueQuery.ToListAsync();

            // Chi phí vận hành
            var expensesQuery = from e in _context.Expenses.AsNoTracking()
                                where e.PaidAt >= start && e.PaidAt < end.AddDays(1)
                                select e;

            if (branchId.HasValue)
            {
                expensesQuery = from e in expensesQuery where e.BranchId == branchId.Value select e;
            }

            var expenses = await expensesQuery.ToListAsync();

            // Group theo tháng hoặc ngày
            var profitData = groupBy == "day"
                ? orders.GroupBy(o => o.PlacedAt.Date).Select(g => new
                {
                    period = g.Key.ToString("yyyy-MM-dd"),
                    revenue = g.Sum(x => x.TotalAmount),
                    cogs = g.Sum(x => x.TotalAmount) * 0.40m,
                    opex = expenses.Where(e => e.PaidAt.Date == g.Key).Sum(e => e.Amount)
                }).ToList()
                : orders.GroupBy(o => new { o.PlacedAt.Year, o.PlacedAt.Month }).Select(g => new
                {
                    period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    revenue = g.Sum(x => x.TotalAmount),
                    cogs = g.Sum(x => x.TotalAmount) * 0.40m,
                    opex = expenses.Where(e => e.PaidAt.Year == g.Key.Year && e.PaidAt.Month == g.Key.Month).Sum(e => e.Amount)
                }).ToList();

            var enrichedData = profitData.Select(p => new
            {
                p.period,
                p.revenue,
                p.cogs,
                p.opex,
                grossProfit = p.revenue - p.cogs,
                netProfit = p.revenue - p.cogs - p.opex,
                grossMargin = p.revenue > 0 ? Math.Round(((p.revenue - p.cogs) / p.revenue) * 100, 2) : 0,
                netMargin = p.revenue > 0 ? Math.Round(((p.revenue - p.cogs - p.opex) / p.revenue) * 100, 2) : 0
            }).ToList();

            // Breakdown chi phí theo category
            var expensesByCategory = expenses
                .GroupBy(e => e.Category)
                .Select(g => new { category = g.Key, amount = g.Sum(x => x.Amount) })
                .OrderByDescending(x => x.amount)
                .Take(5)
                .ToList();

            return Json(new
            {
                success = true,
                summary = new
                {
                    totalRevenue = enrichedData.Sum(x => x.revenue),
                    totalCOGS = enrichedData.Sum(x => x.cogs),
                    totalOpex = enrichedData.Sum(x => x.opex),
                    totalGrossProfit = enrichedData.Sum(x => x.grossProfit),
                    totalNetProfit = enrichedData.Sum(x => x.netProfit)
                },
                expensesByCategory,
                data = enrichedData
            });
        }

        #endregion

        #region B. Báo cáo Khách hàng

        /// <summary>
        /// B1. Customer metrics: Active customers, New registrations, Retention
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CustomerMetrics(DateTime? startDate, DateTime? endDate)
        {
            var start = (startDate ?? DateTime.Today.AddMonths(-1)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            // Tổng khách hàng
            var totalCustomers = await _context.Customers.CountAsync(c => c.IsActive);

            // Khách hàng mới trong kỳ
            var newCustomers = await _context.Customers
                .Where(c => c.CreatedAt >= start && c.CreatedAt < end.AddDays(1))
                .CountAsync();

            // Khách hàng active (có order trong kỳ)
            var activeCustomers = await (
                from o in _context.Orders.AsNoTracking()
                where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1) && o.CustomerId != null
                select o.CustomerId
            ).Distinct().CountAsync();

            // Khách hàng quay lại (có >= 2 đơn trong kỳ)
            var returningCustomers = await (
                from o in _context.Orders.AsNoTracking()
                where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1) && o.CustomerId != null
                group o by o.CustomerId into g
                where g.Count() >= 2
                select g.Key
            ).CountAsync();

            var retentionRate = activeCustomers > 0 ? (decimal)returningCustomers / activeCustomers * 100 : 0;

            return Json(new
            {
                success = true,
                metrics = new
                {
                    totalCustomers,
                    newCustomers,
                    activeCustomers,
                    returningCustomers,
                    retentionRate = Math.Round(retentionRate, 2)
                }
            });
        }

        /// <summary>
        /// B2. AOV, CLV & Top customers
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CustomerValueAnalysis(DateTime? startDate, DateTime? endDate, LoyaltyTier? tier, int limit = 10)
        {
            var start = (startDate ?? DateTime.Today.AddMonths(-1)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            var query = from o in _context.Orders.AsNoTracking()
                        where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1) &&
                              o.Status == OrderStatus.Paid &&
                              o.CustomerId != null
                        join c in _context.Customers.AsNoTracking() on o.CustomerId equals c.CustomerId
                        select new { o, c };

            // Lọc theo tier
            if (tier.HasValue)
            {
                var tierCustomers = from la in _context.LoyaltyAccounts.AsNoTracking()
                                    where la.Tier == tier.Value
                                    select la.CustomerId;
                query = from x in query where tierCustomers.Contains(x.c.CustomerId) select x;
            }

            var customerOrders = await query.ToListAsync();

            var customerStats = customerOrders
                .GroupBy(x => new { x.c.CustomerId, x.c.FirstName, x.c.LastName, x.c.Email })
                .Select(g => new
                {
                    customerId = g.Key.CustomerId,
                    customerName = $"{g.Key.FirstName} {g.Key.LastName}".Trim(),
                    email = g.Key.Email,
                    totalSpent = g.Sum(x => x.o.TotalAmount),
                    orderCount = g.Count(),
                    aov = g.Average(x => x.o.TotalAmount),
                    firstOrderDate = g.Min(x => x.o.PlacedAt),
                    lastOrderDate = g.Max(x => x.o.PlacedAt)
                })
                .OrderByDescending(x => x.totalSpent)
                .Take(limit)
                .ToList();

            var overallAOV = customerOrders.Any() ? customerOrders.Average(x => x.o.TotalAmount) : 0;
            var totalRevenue = customerOrders.Sum(x => x.o.TotalAmount);

            return Json(new
            {
                success = true,
                summary = new
                {
                    overallAOV = Math.Round(overallAOV, 2),
                    totalRevenue,
                    uniqueCustomers = customerStats.Count
                },
                data = customerStats
            });
        }

        /// <summary>
        /// Phân tích phân bố khách hàng theo tier
        /// Pie chart: Bronze, Silver, Gold, Platinum
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TierDistributionAnalysis()
        {
            // Lấy tất cả loyalty accounts
            var loyaltyAccounts = await (from la in _context.LoyaltyAccounts.AsNoTracking()
                                         join c in _context.Customers.AsNoTracking() on la.CustomerId equals c.CustomerId
                                         select new { la.Tier, c.CustomerId })
                                        .ToListAsync();

            // Group by tier
            var tierStats = from la in loyaltyAccounts
                            group la by la.Tier into g
                            select new
                            {
                                tier = g.Key.ToString(),
                                count = g.Count()
                            };

            var statsList = tierStats.ToList();
            var totalCustomers = statsList.Sum(x => x.count);

            var result = statsList.Select(x => new
            {
                tier = x.tier,
                count = x.count,
                percentage = totalCustomers > 0 ? Math.Round((x.count / (double)totalCustomers) * 100, 2) : 0
            }).ToList();

            return Json(new
            {
                success = true,
                data = result
            });
        }

        #endregion

        #region C. Kho & Nguyên liệu

        /// <summary>
        /// C1. Top nguyên liệu được nhập nhiều nhất
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TopPurchasedItems(DateTime? startDate, DateTime? endDate, int limit = 10)
        {
            var start = (startDate ?? DateTime.Today.AddMonths(-6)).Date;
            var end = (endDate ?? DateTime.Today).Date;
            var purchaseOrderItems = await (
                from pol in _context.PurchaseOrderLines.AsNoTracking()
                join po in _context.PurchaseOrders.AsNoTracking() on pol.PurchaseOrderId equals po.PurchaseOrderId
                join item in _context.InventoryItems.AsNoTracking() on pol.ItemId equals item.ItemId
                where po.OrderedAt >= start && po.OrderedAt < end.AddDays(1)
                select new { pol, item, po }
            ).ToListAsync();

            var topItemsFromPO = purchaseOrderItems
                .GroupBy(x => new { x.item.ItemId, x.item.Name, x.item.Unit })
                .Select(g => new
                {
                    itemId = g.Key.ItemId,
                    itemName = g.Key.Name,
                    unit = g.Key.Unit,
                    totalQuantity = g.Sum(x => x.pol.QuantityOrdered),
                    totalValue = g.Sum(x => x.pol.LineTotal),
                    transactionCount = g.Count(),
                    avgUnitCost = g.Any() ? g.Average(x => x.pol.UnitCost) : 0
                })
                .OrderByDescending(x => x.totalQuantity)
                .Take(limit)
                .ToList();

            var totalPurchaseValuePO = topItemsFromPO.Sum(x => x.totalValue);
            var totalQuantityPO = topItemsFromPO.Sum(x => x.totalQuantity);

            return Json(new
            {
                success = true,
                summary = new
                {
                    totalItems = topItemsFromPO.Count,
                    totalPurchaseValue = totalPurchaseValuePO,
                    totalQuantity = totalQuantityPO,
                    source = "PurchaseOrders"
                },
                data = topItemsFromPO
            });
        }

        /// <summary>
        /// C2. Nhập kho & chi phí nhập theo nhà cung cấp
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> PurchaseAnalysis(DateTime? startDate, DateTime? endDate)
        {
            var start = (startDate ?? DateTime.Today.AddMonths(-1)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            var purchaseOrders = await (
                from po in _context.PurchaseOrders.AsNoTracking()
                where po.OrderedAt >= start && po.OrderedAt < end.AddDays(1)
                join s in _context.Suppliers.AsNoTracking() on po.SupplierId equals s.SupplierId
                select new { po, s }
            ).ToListAsync();

            var bySupplier = purchaseOrders
                .GroupBy(x => new { x.s.SupplierId, x.s.Name })
                .Select(g => new
                {
                    supplierId = g.Key.SupplierId,
                    supplierName = g.Key.Name,
                    totalAmount = g.Sum(x => x.po.TotalAmount),
                    orderCount = g.Count(),
                    avgOrderValue = g.Average(x => x.po.TotalAmount)
                })
                .OrderByDescending(x => x.totalAmount)
                .ToList();

            var byMonth = purchaseOrders
                .GroupBy(x => new { x.po.OrderedAt.Year, x.po.OrderedAt.Month })
                .Select(g => new
                {
                    period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    totalAmount = g.Sum(x => x.po.TotalAmount),
                    orderCount = g.Count()
                })
                .OrderBy(x => x.period)
                .ToList();

            return Json(new
            {
                success = true,
                summary = new
                {
                    totalPurchaseValue = purchaseOrders.Sum(x => x.po.TotalAmount),
                    totalOrders = purchaseOrders.Count
                },
                bySupplier,
                byMonth
            });
        }

        #endregion

        #region E. Hiệu suất & Vận hành

        /// <summary>
        /// E1. Peak hours analysis - Orders theo giờ (heatmap)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> PeakHoursAnalysis(DateTime? startDate, DateTime? endDate, Guid? branchId)
        {
            var start = (startDate ?? DateTime.Today.AddDays(-7)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            var ordersQuery = from o in _context.Orders.AsNoTracking()
                              where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1)
                              select o;

            if (branchId.HasValue)
            {
                ordersQuery = from o in ordersQuery where o.BranchId == branchId.Value select o;
            }

            var orders = await ordersQuery.ToListAsync();

            // Group by hour + day of week
            var heatmapData = orders
                .GroupBy(o => new { Hour = o.PlacedAt.Hour, DayOfWeek = (int)o.PlacedAt.DayOfWeek })
                .Select(g => new
                {
                    hour = g.Key.Hour,
                    dayOfWeek = g.Key.DayOfWeek,
                    orderCount = g.Count(),
                    revenue = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.dayOfWeek)
                .ThenBy(x => x.hour)
                .ToList();

            // Tìm peak hours
            var peakHours = heatmapData
                .OrderByDescending(x => x.orderCount)
                .Take(5)
                .Select(x => new
                {
                    x.hour,
                    dayOfWeekName = ((DayOfWeek)x.dayOfWeek).ToString(),
                    x.orderCount,
                    x.revenue
                })
                .ToList();

            return Json(new
            {
                success = true,
                peakHours,
                heatmap = heatmapData
            });
        }

        /// <summary>
        /// E2. Thời gian phục vụ & Lượt quay bàn
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> OperationalMetrics(DateTime? startDate, DateTime? endDate, Guid? branchId)
        {
            var start = (startDate ?? DateTime.Today.AddDays(-7)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            var ordersQuery = from o in _context.Orders.AsNoTracking()
                              where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1)
                              select o;

            if (branchId.HasValue)
            {
                ordersQuery = from o in ordersQuery where o.BranchId == branchId.Value select o;
            }

            var orders = await ordersQuery.ToListAsync();

            // Thời gian phục vụ trung bình
            var completedOrders = orders.Where(o => o.ServedAt.HasValue).ToList();
            var fulfillmentTimes = completedOrders
                .Select(o => (o.ServedAt!.Value - o.PlacedAt).TotalMinutes)
                .ToList();

            var avgFulfillment = fulfillmentTimes.Any() ? fulfillmentTimes.Average() : 0;

            // Lượt quay bàn
            var tableOrders = from o in orders
                              where o.TableId.HasValue
                              join t in _context.RestaurantTables.AsNoTracking() on o.TableId equals t.TableId
                              select new { o.TableId, t.Name, o.PlacedAt, o.ServedAt };

            var tableTurnover = (from to in tableOrders
                                 group to by new { to.TableId, to.Name } into g
                                 let avgDuration = g.Where(x => x.ServedAt.HasValue).Any() 
                                                   ? g.Where(x => x.ServedAt.HasValue).Average(x => (x.ServedAt!.Value - x.PlacedAt).TotalMinutes)
                                                   : 0
                                 select new
                                 {
                                     tenBan = g.Key.Name,
                                     soLuotDat = g.Count(),
                                     thoiGianTrungBinh = avgDuration
                                 })
                                .OrderByDescending(x => x.soLuotDat)
                                .Take(10)
                                .ToList();

            return Json(new
            {
                success = true,
                thoiGianPhucVu = new
                {
                    phutTrungBinh = Math.Round(avgFulfillment, 2),
                    phanPho = fulfillmentTimes.Take(100)
                },
                luotQuayBan = tableTurnover
            });
        }

        #endregion

        #region F. Xu hướng & Seasonality

        /// <summary>
        /// F. Seasonality & Campaign impact
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SeasonalityAnalysis(DateTime? startDate, DateTime? endDate)
        {
            var start = (startDate ?? DateTime.Today.AddMonths(-12)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            var orders = await (
                from o in _context.Orders.AsNoTracking()
                where o.PlacedAt >= start && o.PlacedAt < end.AddDays(1) && o.Status == OrderStatus.Paid
                select o
            ).ToListAsync();

            // Weekly pattern
            var byDayOfWeek = orders
                .GroupBy(o => o.PlacedAt.DayOfWeek)
                .Select(g => new
                {
                    dayOfWeek = g.Key.ToString(),
                    avgRevenue = g.Average(x => x.TotalAmount),
                    avgOrders = g.Count() / Math.Max(1, (end - start).Days / 7)
                })
                .OrderBy(x => x.dayOfWeek)
                .ToList();

            // Monthly trend
            var byMonth = orders
                .GroupBy(o => new { o.PlacedAt.Year, o.PlacedAt.Month })
                .Select(g => new
                {
                    period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    revenue = g.Sum(x => x.TotalAmount),
                    orders = g.Count()
                })
                .OrderBy(x => x.period)
                .ToList();

            return Json(new
            {
                success = true,
                weeklyPattern = byDayOfWeek,
                monthlyTrend = byMonth
            });
        }

        #endregion

        #region Export Functions

        [HttpGet]
        public IActionResult ExportCSV(string reportType, DateTime? startDate, DateTime? endDate)
        {
            return Content("CSV Export - Coming Soon", "text/csv");
        }

        [HttpGet]
        public IActionResult ExportPDF(string reportType, DateTime? startDate, DateTime? endDate)
        {
            return Content("PDF Export - Coming Soon", "application/pdf");
        }

        #endregion
    }
}
