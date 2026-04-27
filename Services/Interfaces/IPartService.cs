using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IPartService
    {
        Task<List<PartResponseDto>> GetAllAsync();
        Task<PartResponseDto?> GetByIdAsync(int id);
        Task<(bool Success, string Message, PartResponseDto? Data)> CreateAsync(CreatePartDto dto);
        Task<(bool Success, string Message, PartResponseDto? Data)> UpdateAsync(int id, UpdatePartDto dto);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
