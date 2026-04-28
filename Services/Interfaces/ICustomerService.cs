using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface ICustomerService
    {
        // F6
        Task<(bool Success, string Message, CustomerResponseDto? Data)> RegisterAsync(RegisterCustomerDto dto);

        // F8
        Task<CustomerProfileResponseDto?> GetProfileAsync(int customerId);
        Task<(bool Success, string Message)> UpdateCustomerAsync(int customerId, UpdateCustomerDto dto);
        Task<(bool Success, string Message)> AddVehicleAsync(int customerId, AddVehicleDto dto);
        Task<(bool Success, string Message)> UpdateVehicleAsync(int customerId, int vehicleId, UpdateVehicleDto dto);
        Task<(bool Success, string Message)> DeleteVehicleAsync(int customerId, int vehicleId);
    }
}