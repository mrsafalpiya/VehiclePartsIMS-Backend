using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<List<PurchaseHistoryItemDto>> GetPurchaseHistoryAsync(int customerId);
        Task<List<AppointmentResponseDto>> GetAppointmentHistoryAsync(int customerId);
    }
}