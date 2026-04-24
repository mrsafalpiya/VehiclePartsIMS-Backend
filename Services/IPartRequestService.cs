using VehiclePartsIMS_Backend.DTOs;

namespace VehiclePartsIMS_Backend.Services
{
    public interface IPartRequestService
    {
        // Customer creates a part request
        Task<PartRequestResponseDto> CreatePartRequestAsync(PartRequestCreateDto dto);
        
        // Admin/Staff get all requests
        Task<List<PartRequestResponseDto>> GetAllPartRequestsAsync();
        
        // Admin/Staff get pending requests only
        Task<List<PartRequestResponseDto>> GetPendingRequestsAsync();
        
        // Admin/Staff get requests by customer
        Task<List<PartRequestResponseDto>> GetRequestsByCustomerAsync(int customerId);
        
        // Admin/Staff update request status
        Task<PartRequestResponseDto> UpdateRequestStatusAsync(PartRequestUpdateDto dto);
    }
}