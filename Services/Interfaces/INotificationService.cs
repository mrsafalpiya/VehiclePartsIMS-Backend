using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateLowStockNotificationAsync(string partName, int currentStock);
        Task<List<NotificationResponseDto>> GetAllNotificationsAsync();
        Task MarkAsReadAsync(int notificationId);
    }
}
