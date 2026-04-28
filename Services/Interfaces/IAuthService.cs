using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
