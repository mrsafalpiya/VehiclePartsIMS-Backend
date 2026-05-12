using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Enums;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations;

public class OverdueCreditReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OverdueCreditReminderService> _logger;
    private readonly IEmailService emailService;
    private readonly AppDbContext context;

    public OverdueCreditReminderService(
        IServiceScopeFactory scopeFactory,
        ILogger<OverdueCreditReminderService> logger,
        IEmailService emailService,
        AppDbContext context)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        this.emailService = emailService;
        this.context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Overdue Credit Reminder Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await SendOverdueRemindersAsync();
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task SendOverdueRemindersAsync()
    {
        _logger.LogInformation("Checking for overdue invoices at {Time}", DateTime.UtcNow);

        // DateOnly comparison — one month ago
        var oneMonthAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1));

        // Fetch all unpaid invoices older than 1 month, grouped by customer
        var overdueInvoices = await context.SalesInvoices
            .Where(inv =>
                inv.PaymentStatus == SalesInvoicePaymentStatus.Unpaid &&
                inv.InvoiceDate < oneMonthAgo)
            .Include(inv => inv.Customer)
            .ToListAsync();

        if (!overdueInvoices.Any())
        {
            _logger.LogInformation("No overdue invoices found.");
            return;
        }

        // Group by customer in memory after fetching
        var groupedByCustomer = overdueInvoices
            .GroupBy(inv => inv.Customer);

        foreach (var group in groupedByCustomer)
        {
            var customer = group.Key;
            var customerInvoices = group.ToList();
            var totalOutstanding = customerInvoices.Sum(inv => inv.FinalTotal);

            var invoiceLines = customerInvoices.Select(inv =>
                $"- Invoice #{inv.InvoiceNumber} " +
                $"dated {inv.InvoiceDate:yyyy-MM-dd}: " +
                $"{inv.FinalTotal:N0}");

            var invoiceDetails = string.Join("\n", invoiceLines);

            var emailBody = $"""
                Dear {customer.FullName},

                This is a reminder that you have overdue unpaid invoice(s) on your account.

                Outstanding Invoices:
                {invoiceDetails}

                Total Outstanding Balance: {totalOutstanding:N0}

                Please settle your balance at your earliest convenience.

                Thank you.
                """;

            await emailService.SendEmailAsync(
                customer.Email,
                "Overdue Payment Reminder",
                emailBody
            );

            _logger.LogInformation(
                "Sent overdue reminder to {Email} for {Count} invoice(s).",
                customer.Email,
                customerInvoices.Count);
        }
    }
}