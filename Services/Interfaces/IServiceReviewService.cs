using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;

namespace VehiclePartsIMS_Backend.Services.Interfaces
{
    public interface IServiceReviewService
    {
        Task<ServiceReviewResponseDto> SubmitReviewAsync(int customerId, CreateServiceReviewDto dto);
    }
}