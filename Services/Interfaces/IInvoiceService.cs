using System.Threading.Tasks;
using VehiclePartsIMS_Backend.DTOs;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<PurchaseInvoiceResponseDto> CreatePurchaseInvoiceAsync(PurchaseInvoiceCreateDto dto);
        Task<List<PurchaseInvoiceResponseDto>> GetAllPurchaseInvoicesAsync();
        Task<PurchaseInvoiceResponseDto?> GetPurchaseInvoiceByIdAsync(int id);

        Task<SalesInvoiceResponseDto> CreateSalesInvoiceAsync(SalesInvoiceCreateDto dto);
    }
}