using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IVendorService
    {
        Task<List<VendorResponseDto>> GetAllAsync(string? search);
        Task<VendorResponseDto?> GetByIdAsync(int id);
        Task<VendorResponseDto> CreateAsync(CreateVendorDto dto);
        Task<VendorResponseDto?> UpdateAsync(int id, UpdateVendorDto dto);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
