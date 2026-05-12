namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendInvoiceEmailAsync(int invoiceId);
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}