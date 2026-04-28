using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CustomerService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // F6 — Register customer with at least one vehicle
        public async Task<(bool Success, string Message, CustomerResponseDto? Data)> RegisterAsync(RegisterCustomerDto dto)
        {
            if (dto.Vehicles == null || dto.Vehicles.Count == 0)
                return (false, "At least one vehicle is required.", null);

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return (false, "A customer with this email already exists.", null);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                HomeAddress = dto.HomeAddress
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors, null);
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            var vehicles = dto.Vehicles.Select(v => new Vehicle
            {
                CustomerId = user.Id,
                Customer = user,
                PlateNumber = v.PlateNumber,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year
            }).ToList();

            await _context.Vehicles.AddRangeAsync(vehicles);
            await _context.SaveChangesAsync();

            return (true, "Customer registered successfully.", MapToDto(user));
        }

        private static CustomerResponseDto MapToDto(User u) => new()
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email ?? string.Empty,
            PhoneNumber = u.PhoneNumber ?? string.Empty
        };
    }
}