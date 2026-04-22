using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IPartRequestService
    {
        Task<PartRequestResponseDto> SubmitPartRequestAsync(int customerId, CreatePartRequestDto dto);
    }
}