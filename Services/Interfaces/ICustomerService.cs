using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface ICustomerService
    {
        // F6
        Task<(bool Success, string Message, CustomerResponseDto? Data)> RegisterAsync(RegisterCustomerDto dto);
    }
}