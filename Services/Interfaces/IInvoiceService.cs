using System.Threading.Tasks;
using VehiclePartsIMS_Backend.DTOs;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<PurchaseInvoiceResponseDto> CreatePurchaseInvoiceAsync(PurchaseInvoiceCreateDto dto);
        
        Task<SalesInvoiceResponseDto> CreateSalesInvoiceAsync(SalesInvoiceCreateDto dto);
    }
}