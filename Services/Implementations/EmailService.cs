using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Settings;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly AppDbContext _context;
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(AppDbContext context, IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendInvoiceEmailAsync(int invoiceId)
        {
            var invoice = await _context.SalesInvoices
                .Include(i => i.Customer)
                .Include(i => i.CreatedByStaff)
                .Include(i => i.Items)
                    .ThenInclude(item => item.Part)
                .FirstOrDefaultAsync(i => i.Id == invoiceId)
                ?? throw new KeyNotFoundException($"Invoice with ID {invoiceId} not found.");

            var customerEmail = invoice.Customer.Email
                ?? throw new InvalidOperationException("Customer has no email address.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(invoice.Customer.FullName, customerEmail));
            message.Subject = $"Invoice {invoice.InvoiceNumber} - Vehicle Parts IMS";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = BuildEmailHtml(invoice);
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private static string BuildEmailHtml(Data.Entities.SalesInvoice invoice)
        {
            var lineItems = string.Join("", invoice.Items.Select(item => $"""
                <tr>
                    <td style="padding:8px;border-bottom:1px solid #eee;">{item.Part.PartName}</td>
                    <td style="padding:8px;border-bottom:1px solid #eee;text-align:center;">{item.Quantity}</td>
                    <td style="padding:8px;border-bottom:1px solid #eee;text-align:right;">£{item.UnitSellingPrice:N0}</td>
                    <td style="padding:8px;border-bottom:1px solid #eee;text-align:right;">£{item.Quantity * item.UnitSellingPrice:N0}</td>
                </tr>
            """));

            string discountRow = invoice.LoyaltyDiscount.HasValue
                ? $"""
                    <tr>
                        <td colspan="3" style="padding:8px;text-align:right;color:green;"><strong>Loyalty Discount (10%)</strong></td>
                        <td style="padding:8px;text-align:right;color:green;">-£{invoice.LoyaltyDiscount.Value:N0}</td>
                    </tr>
                  """
                : "";

            return $"""
                <div style="font-family:Arial,sans-serif;max-width:600px;margin:0 auto;padding:20px;">
                    <div style="background:#2E75B6;color:white;padding:20px;border-radius:8px 8px 0 0;">
                        <h1 style="margin:0;font-size:24px;">Vehicle Parts IMS</h1>
                        <p style="margin:5px 0 0;">Invoice Receipt</p>
                    </div>
                    <div style="background:#f9f9f9;padding:20px;border:1px solid #eee;">
                        <h2 style="color:#2E75B6;">Invoice #{invoice.InvoiceNumber}</h2>
                        <p><strong>Date:</strong> {invoice.InvoiceDate:dd MMM yyyy}</p>
                        <p><strong>Customer:</strong> {invoice.Customer.FullName}</p>
                        <p><strong>Email:</strong> {invoice.Customer.Email}</p>
                        <p><strong>Payment Status:</strong>
                            <span style="color:{(invoice.PaymentStatus.ToString() == "Paid" ? "green" : "red")};">
                                {invoice.PaymentStatus}
                            </span>
                        </p>
                    </div>
                    <table style="width:100%;border-collapse:collapse;margin-top:20px;">
                        <thead>
                            <tr style="background:#2E75B6;color:white;">
                                <th style="padding:10px;text-align:left;">Part</th>
                                <th style="padding:10px;text-align:center;">Qty</th>
                                <th style="padding:10px;text-align:right;">Unit Price</th>
                                <th style="padding:10px;text-align:right;">Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            {lineItems}
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="3" style="padding:8px;text-align:right;"><strong>Subtotal</strong></td>
                                <td style="padding:8px;text-align:right;">£{invoice.Subtotal:N0}</td>
                            </tr>
                            {discountRow}
                            <tr style="background:#f0f0f0;">
                                <td colspan="3" style="padding:10px;text-align:right;"><strong>Final Total</strong></td>
                                <td style="padding:10px;text-align:right;"><strong>£{invoice.FinalTotal:N0}</strong></td>
                            </tr>
                        </tfoot>
                    </table>
                    <div style="margin-top:20px;padding:15px;background:#f9f9f9;border-radius:0 0 8px 8px;text-align:center;color:#666;">
                        <p>Thank you for your business!</p>
                        <p style="font-size:12px;">This is an automated email from Vehicle Parts IMS. Please do not reply.</p>
                    </div>
                </div>
            """;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                // Build the email message
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                message.To.Add(new MailboxAddress(string.Empty, toEmail));
                message.Subject = subject;

                // Plain text body
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                // Connect and send
                using var smtpClient = new SmtpClient();

                await smtpClient.ConnectAsync(
                    _emailSettings.SmtpHost,
                    _emailSettings.SmtpPort,
                    SecureSocketOptions.StartTls
                );

                await smtpClient.AuthenticateAsync(
                    _emailSettings.SenderEmail,
                    _emailSettings.Password
                );

                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {Email}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
                throw;
            }
        }
    }
}