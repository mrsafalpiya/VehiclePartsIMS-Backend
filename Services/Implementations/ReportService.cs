using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Enums;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FinancialReportDto> GetDailyReportAsync(DateOnly date)
        {
            var sales = await _context.SalesInvoices
                .Where(i => i.InvoiceDate == date)
                .ToListAsync();

            var purchases = await _context.PurchaseInvoices
                .Include(i => i.Items)
                .Where(i => i.InvoiceDate == date)
                .ToListAsync();

            var transactions = new List<TransactionItemDto>();

            foreach (var s in sales)
            {
                transactions.Add(new TransactionItemDto
                {
                    Type = "Sale",
                    InvoiceNumber = s.InvoiceNumber,
                    Date = s.InvoiceDate.ToString("yyyy-MM-dd"),
                    Description = $"Sales Invoice - {s.InvoiceNumber}",
                    Amount = s.FinalTotal
                });
            }

            foreach (var p in purchases)
            {
                int total = p.Items.Sum(i => i.Quantity * i.UnitCostPrice);
                transactions.Add(new TransactionItemDto
                {
                    Type = "Purchase",
                    InvoiceNumber = p.InvoiceNumber,
                    Date = p.InvoiceDate.ToString("yyyy-MM-dd"),
                    Description = $"Purchase Invoice - {p.InvoiceNumber}",
                    Amount = total
                });
            }

            int revenue     = sales.Sum(s => s.FinalTotal);
            int expenditure = purchases.Sum(p => p.Items.Sum(i => i.Quantity * i.UnitCostPrice));

            return new FinancialReportDto
            {
                ReportType      = "Daily",
                Period          = date.ToString("dd MMM yyyy"),
                TotalRevenue    = revenue,
                TotalExpenditure = expenditure,
                NetProfit       = revenue - expenditure,
                Transactions    = transactions.OrderByDescending(t => t.Date).ToList()
            };
        }

        public async Task<FinancialReportDto> GetMonthlyReportAsync(int month, int year)
        {
            var sales = await _context.SalesInvoices
                .Where(i => i.InvoiceDate.Month == month && i.InvoiceDate.Year == year)
                .ToListAsync();

            var purchases = await _context.PurchaseInvoices
                .Include(i => i.Items)
                .Where(i => i.InvoiceDate.Month == month && i.InvoiceDate.Year == year)
                .ToListAsync();

            var transactions = new List<TransactionItemDto>();

            foreach (var s in sales)
            {
                transactions.Add(new TransactionItemDto
                {
                    Type = "Sale",
                    InvoiceNumber = s.InvoiceNumber,
                    Date = s.InvoiceDate.ToString("yyyy-MM-dd"),
                    Description = $"Sales Invoice - {s.InvoiceNumber}",
                    Amount = s.FinalTotal
                });
            }

            foreach (var p in purchases)
            {
                int total = p.Items.Sum(i => i.Quantity * i.UnitCostPrice);
                transactions.Add(new TransactionItemDto
                {
                    Type = "Purchase",
                    InvoiceNumber = p.InvoiceNumber,
                    Date = p.InvoiceDate.ToString("yyyy-MM-dd"),
                    Description = $"Purchase Invoice - {p.InvoiceNumber}",
                    Amount = total
                });
            }

            int revenue     = sales.Sum(s => s.FinalTotal);
            int expenditure = purchases.Sum(p => p.Items.Sum(i => i.Quantity * i.UnitCostPrice));

            string monthName = new DateTime(year, month, 1).ToString("MMMM yyyy");

            return new FinancialReportDto
            {
                ReportType       = "Monthly",
                Period           = monthName,
                TotalRevenue     = revenue,
                TotalExpenditure = expenditure,
                NetProfit        = revenue - expenditure,
                Transactions     = transactions.OrderByDescending(t => t.Date).ToList()
            };
        }

        public async Task<FinancialReportDto> GetYearlyReportAsync(int year)
        {
            var sales = await _context.SalesInvoices
                .Where(i => i.InvoiceDate.Year == year)
                .ToListAsync();

            var purchases = await _context.PurchaseInvoices
                .Include(i => i.Items)
                .Where(i => i.InvoiceDate.Year == year)
                .ToListAsync();

            var transactions = new List<TransactionItemDto>();

            foreach (var s in sales)
            {
                transactions.Add(new TransactionItemDto
                {
                    Type = "Sale",
                    InvoiceNumber = s.InvoiceNumber,
                    Date = s.InvoiceDate.ToString("yyyy-MM-dd"),
                    Description = $"Sales Invoice - {s.InvoiceNumber}",
                    Amount = s.FinalTotal
                });
            }

            foreach (var p in purchases)
            {
                int total = p.Items.Sum(i => i.Quantity * i.UnitCostPrice);
                transactions.Add(new TransactionItemDto
                {
                    Type = "Purchase",
                    InvoiceNumber = p.InvoiceNumber,
                    Date = p.InvoiceDate.ToString("yyyy-MM-dd"),
                    Description = $"Purchase Invoice - {p.InvoiceNumber}",
                    Amount = total
                });
            }

            int revenue     = sales.Sum(s => s.FinalTotal);
            int expenditure = purchases.Sum(p => p.Items.Sum(i => i.Quantity * i.UnitCostPrice));

            return new FinancialReportDto
            {
                ReportType       = "Yearly",
                Period           = year.ToString(),
                TotalRevenue     = revenue,
                TotalExpenditure = expenditure,
                NetProfit        = revenue - expenditure,
                Transactions     = transactions.OrderByDescending(t => t.Date).ToList()
            };
        }

        public async Task<List<RegularCustomerReportItemDto>> GetRegularCustomersReportAsync()
        {
            return await _context.SalesInvoices
                .Include(i => i.Customer)
                .GroupBy(i => new { i.CustomerId, i.Customer.FullName, i.Customer.PhoneNumber, i.Customer.Email })
                .Select(g => new RegularCustomerReportItemDto
                {
                    CustomerName   = g.Key.FullName,
                    PhoneNumber    = g.Key.PhoneNumber ?? string.Empty,
                    Email          = g.Key.Email ?? string.Empty,
                    TotalPurchases = g.Count()
                })
                .OrderByDescending(x => x.TotalPurchases)
                .ToListAsync();
        }

        public async Task<List<HighSpenderReportItemDto>> GetHighSpendersReportAsync()
        {
            return await _context.SalesInvoices
                .Include(i => i.Customer)
                .GroupBy(i => new { i.CustomerId, i.Customer.FullName, i.Customer.PhoneNumber, i.Customer.Email })
                .Select(g => new HighSpenderReportItemDto
                {
                    CustomerName     = g.Key.FullName,
                    PhoneNumber      = g.Key.PhoneNumber ?? string.Empty,
                    Email            = g.Key.Email ?? string.Empty,
                    TotalAmountSpent = g.Sum(i => i.FinalTotal)
                })
                .OrderByDescending(x => x.TotalAmountSpent)
                .ToListAsync();
        }

        public async Task<List<PendingCreditReportItemDto>> GetPendingCreditsReportAsync()
        {
            return await _context.SalesInvoices
                .Include(i => i.Customer)
                .Where(i => i.PaymentStatus == SalesInvoicePaymentStatus.Unpaid)
                .GroupBy(i => new { i.CustomerId, i.Customer.FullName, i.Customer.PhoneNumber, i.Customer.Email })
                .Select(g => new PendingCreditReportItemDto
                {
                    CustomerName             = g.Key.FullName,
                    PhoneNumber              = g.Key.PhoneNumber ?? string.Empty,
                    Email                    = g.Key.Email ?? string.Empty,
                    TotalOutstandingBalance  = g.Sum(i => i.FinalTotal)
                })
                .OrderByDescending(x => x.TotalOutstandingBalance)
                .ToListAsync();
        }
    }
}