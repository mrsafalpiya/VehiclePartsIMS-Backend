using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IStaffService
    {
        Task<List<StaffResponseDto>> GetAllAsync();
        Task<StaffResponseDto?> GetByIdAsync(int id);
        Task<(bool Success, string Message, StaffResponseDto? Data)> CreateAsync(CreateStaffDto dto);
        Task<(bool Success, string Message, StaffResponseDto? Data)> UpdateAsync(int id, UpdateStaffDto dto);
        Task<(bool Success, string Message)> DeleteAsync(int id, int requestingUserId);
    }
}
