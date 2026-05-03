namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendInvoiceEmailAsync(int invoiceId);
    }
}