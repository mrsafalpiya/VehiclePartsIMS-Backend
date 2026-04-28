using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<(bool Success, string Message, CustomerProfileResponseDto? Data)> RegisterAsync(RegisterCustomerDto dto);
        Task<CustomerProfileResponseDto?> GetProfileAsync(int userId);
        Task<(bool Success, string Message, CustomerProfileResponseDto? Data)> UpdateProfileAsync(int userId, UpdateCustomerProfileDto dto);
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task<List<VehicleResponseDto>> GetVehiclesAsync(int userId);
        Task<(bool Success, string Message, VehicleResponseDto? Data)> AddVehicleAsync(int userId, CreateVehicleDto dto);
        Task<(bool Success, string Message, VehicleResponseDto? Data)> UpdateVehicleAsync(int vehicleId, int userId, UpdateVehicleDto dto);
        Task<(bool Success, string Message)> DeleteVehicleAsync(int vehicleId, int userId);
    }
}
