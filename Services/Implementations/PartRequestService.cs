using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Data.Enums;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class PartRequestService : IPartRequestService
    {
        private readonly AppDbContext _context;

        public PartRequestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PartRequestResponseDto> SubmitPartRequestAsync(int customerId, CreatePartRequestDto dto)
        {
            var customer = await _context.Users.FindAsync(customerId)
                           ?? throw new KeyNotFoundException("Customer not found.");

            var partRequest = new PartRequest
            {
                CustomerId = customerId,
                Customer = customer,
                PartName = dto.PartName,
                Notes = dto.Notes,
                Status = PartRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PartRequests.Add(partRequest);
            await _context.SaveChangesAsync();

            return new PartRequestResponseDto
            {
                Id = partRequest.Id,
                CustomerId = partRequest.CustomerId,
                CustomerName = customer.FullName,
                PartName = partRequest.PartName,
                Notes = partRequest.Notes,
                Status = partRequest.Status.ToString(),
                CreatedAt = partRequest.CreatedAt
            };
        }
    }
}