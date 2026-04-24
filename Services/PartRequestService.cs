using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Data.Enums;
using VehiclePartsIMS_Backend.DTOs;

namespace VehiclePartsIMS_Backend.Services
{
    public class PartRequestService : IPartRequestService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PartRequestService> _logger;

        public PartRequestService(AppDbContext context, ILogger<PartRequestService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PartRequestResponseDto> CreatePartRequestAsync(PartRequestCreateDto dto)
        {
            try
            {
                var customer = await _context.Users.FindAsync(dto.CustomerId);
                if (customer == null)
                    throw new KeyNotFoundException($"Customer with ID {dto.CustomerId} not found");

                var request = new PartRequest
                {
                    CustomerId = dto.CustomerId,
                    Customer = customer,
                    PartName = dto.PartName,
                    Notes = dto.Notes,
                    Status = PartRequestStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.PartRequests.Add(request);
                await _context.SaveChangesAsync();

                return new PartRequestResponseDto
                {
                    Id = request.Id,
                    CustomerId = request.CustomerId,
                    CustomerName = customer.FullName,
                    PartName = request.PartName,
                    Notes = request.Notes,
                    Status = request.Status.ToString(),
                    CreatedAt = request.CreatedAt,
                    UpdatedAt = request.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating part request");
                throw;
            }
        }

        public async Task<List<PartRequestResponseDto>> GetAllPartRequestsAsync()
        {
            var requests = await _context.PartRequests
                .Include(r => r.Customer)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return requests.Select(r => new PartRequestResponseDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.FullName,
                PartName = r.PartName,
                Notes = r.Notes,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();
        }

        public async Task<List<PartRequestResponseDto>> GetPendingRequestsAsync()
        {
            var requests = await _context.PartRequests
                .Include(r => r.Customer)
                .Where(r => r.Status == PartRequestStatus.Pending)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return requests.Select(r => new PartRequestResponseDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.FullName,
                PartName = r.PartName,
                Notes = r.Notes,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();
        }

        public async Task<List<PartRequestResponseDto>> GetRequestsByCustomerAsync(int customerId)
        {
            var customer = await _context.Users.FindAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found");

            var requests = await _context.PartRequests
                .Include(r => r.Customer)
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return requests.Select(r => new PartRequestResponseDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = customer.FullName,
                PartName = r.PartName,
                Notes = r.Notes,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();
        }

        public async Task<PartRequestResponseDto> UpdateRequestStatusAsync(PartRequestUpdateDto dto)
        {
            try
            {
                var request = await _context.PartRequests
                    .Include(r => r.Customer)
                    .FirstOrDefaultAsync(r => r.Id == dto.Id);

                if (request == null)
                    throw new KeyNotFoundException($"Part request with ID {dto.Id} not found");

                request.Status = dto.Status;
                request.UpdatedAt = DateTime.UtcNow;

                _context.PartRequests.Update(request);
                await _context.SaveChangesAsync();

                return new PartRequestResponseDto
                {
                    Id = request.Id,
                    CustomerId = request.CustomerId,
                    CustomerName = request.Customer.FullName,
                    PartName = request.PartName,
                    Notes = request.Notes,
                    Status = request.Status.ToString(),
                    CreatedAt = request.CreatedAt,
                    UpdatedAt = request.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating part request status");
                throw;
            }
        }
    }
}