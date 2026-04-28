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

        // F8 — Get full customer profile with vehicles and sales history
        public async Task<CustomerProfileResponseDto?> GetProfileAsync(int customerId)
        {
            var user = await _userManager.FindByIdAsync(customerId.ToString());
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Customer")) return null;

            var vehicles = await _context.Vehicles
                .Where(v => v.CustomerId == customerId)
                .Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    PlateNumber = v.PlateNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year
                }).ToListAsync();

            var salesHistory = await _context.SalesInvoices
                .Where(s => s.CustomerId == customerId)
                .Include(s => s.SalesInvoiceItems)
                    .ThenInclude(i => i.Part)
                .OrderByDescending(s => s.InvoiceDate)
                .Select(s => new SalesInvoiceSummaryDto
                {
                    Id = s.Id,
                    InvoiceNumber = s.InvoiceNumber,
                    InvoiceDate = s.InvoiceDate,
                    PaymentStatus = s.PaymentStatus.ToString(),
                    Subtotal = s.Subtotal,
                    LoyaltyDiscount = s.LoyaltyDiscount,
                    FinalTotal = s.FinalTotal,
                    Items = s.SalesInvoiceItems.Select(i => new InvoiceItemSummaryDto
                    {
                        PartName = i.Part.PartName,
                        Quantity = i.Quantity,
                        UnitSellingPrice = i.UnitSellingPrice
                    }).ToList()
                }).ToListAsync();

            return new CustomerProfileResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                HomeAddress = user.HomeAddress ?? string.Empty,
                Vehicles = vehicles,
                SalesHistory = salesHistory
            };
        }

        // F8 — Update customer personal details
        public async Task<(bool Success, string Message)> UpdateCustomerAsync(int customerId, UpdateCustomerDto dto)
        {
            var user = await _userManager.FindByIdAsync(customerId.ToString());
            if (user == null) return (false, "Customer not found.");

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.HomeAddress = dto.HomeAddress;

            await _userManager.UpdateAsync(user);
            return (true, "Customer updated successfully.");
        }

        // F8 — Add a new vehicle to a customer
        public async Task<(bool Success, string Message)> AddVehicleAsync(int customerId, AddVehicleDto dto)
        {
            var user = await _userManager.FindByIdAsync(customerId.ToString());
            if (user == null) return (false, "Customer not found.");

            bool plateExists = await _context.Vehicles
                .AnyAsync(v => v.PlateNumber == dto.PlateNumber);
            if (plateExists) return (false, "A vehicle with this plate number already exists.");

            var vehicle = new Vehicle
            {
                CustomerId = customerId,
                Customer = user,
                PlateNumber = dto.PlateNumber,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year
            };

            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return (true, "Vehicle added successfully.");
        }

        // F8 — Update an existing vehicle
        public async Task<(bool Success, string Message)> UpdateVehicleAsync(int customerId, int vehicleId, UpdateVehicleDto dto)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.CustomerId == customerId);
            if (vehicle == null) return (false, "Vehicle not found.");

            bool plateExists = await _context.Vehicles
                .AnyAsync(v => v.PlateNumber == dto.PlateNumber && v.Id != vehicleId);
            if (plateExists) return (false, "A vehicle with this plate number already exists.");

            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;

            await _context.SaveChangesAsync();
            return (true, "Vehicle updated successfully.");
        }

        // F8 — Delete a vehicle from a customer
        public async Task<(bool Success, string Message)> DeleteVehicleAsync(int customerId, int vehicleId)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.CustomerId == customerId);
            if (vehicle == null) return (false, "Vehicle not found.");

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return (true, "Vehicle deleted successfully.");
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