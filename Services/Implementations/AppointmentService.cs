using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Data.Enums;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentResponseDto> BookAppointmentAsync(int customerId, CreateAppointmentDto dto)
        {
            var customer = await _context.Users.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Customer not found.");

            var appointment = new Appointment
            {
                CustomerId = customerId,
                Customer = customer,
                PreferredDate = DateOnly.Parse(dto.PreferredDate),
                PreferredTime = TimeOnly.Parse(dto.PreferredTime),
                CreatedAt = DateTime.UtcNow,
                Status = AppointmentStatus.Pending
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return MapToDto(appointment);
        }

        public async Task<List<AppointmentResponseDto>> GetMyAppointmentsAsync(int customerId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return appointments.Select(MapToDto).ToList();
        }

        private static AppointmentResponseDto MapToDto(Appointment a) => new()
        {
            Id = a.Id,
            CustomerId = a.CustomerId,
            CustomerName = a.Customer.FullName,
            PreferredDate = a.PreferredDate.ToString("yyyy-MM-dd"),
            PreferredTime = a.PreferredTime.ToString("HH:mm"),
            Status = a.Status.ToString(),
            CreatedAt = a.CreatedAt
        };
    }
}