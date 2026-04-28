using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class StaffCustomerService(UserManager<User> userManager, AppDbContext context) : IStaffCustomerService
    {
        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            var customerRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
            if (customerRole == null) return [];

            var customerIds = await context.UserRoles
                .Where(ur => ur.RoleId == customerRole.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var customers = await context.Users
                .Where(u => customerIds.Contains(u.Id))
                .OrderBy(u => u.FullName)
                .Select(u => new CustomerResponseDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email ?? string.Empty,
                    PhoneNumber = u.PhoneNumber ?? string.Empty
                })
                .ToListAsync();

            return customers;
        }

        public async Task<(bool Success, string Message, CustomerResponseDto? Data)> RegisterAsync(StaffRegisterCustomerDto dto)
        {
            if (dto.Vehicles == null || dto.Vehicles.Count == 0)
                return (false, "At least one vehicle is required.", null);

            var existingUser = await userManager.FindByEmailAsync(dto.Email);
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

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors, null);
            }

            await userManager.AddToRoleAsync(user, "Customer");

            var vehicles = dto.Vehicles.Select(v => new Vehicle
            {
                CustomerId = user.Id,
                Customer = user,
                PlateNumber = v.PlateNumber,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year
            }).ToList();

            await context.Vehicles.AddRangeAsync(vehicles);
            await context.SaveChangesAsync();

            return (true, "Customer registered successfully.", new CustomerResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            });
        }

        public async Task<StaffCustomerProfileResponseDto?> GetProfileAsync(int customerId)
        {
            var user = await userManager.FindByIdAsync(customerId.ToString());
            if (user == null) return null;

            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Contains("Customer")) return null;

            var vehicles = await context.Vehicles
                .Where(v => v.CustomerId == customerId)
                .Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    PlateNumber = v.PlateNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year
                }).ToListAsync();

            var salesHistory = await context.SalesInvoices
                .Where(s => s.CustomerId == customerId)
                .Include(s => s.Items)
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
                    Items = s.Items.Select(i => new InvoiceItemSummaryDto
                    {
                        PartName = i.Part.PartName,
                        Quantity = i.Quantity,
                        UnitSellingPrice = i.UnitSellingPrice
                    }).ToList()
                }).ToListAsync();

            return new StaffCustomerProfileResponseDto
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

        public async Task<(bool Success, string Message)> UpdateCustomerAsync(int customerId, UpdateCustomerDto dto)
        {
            var user = await userManager.FindByIdAsync(customerId.ToString());
            if (user == null) return (false, "Customer not found.");

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.HomeAddress = dto.HomeAddress;

            await userManager.UpdateAsync(user);
            return (true, "Customer updated successfully.");
        }

        public async Task<(bool Success, string Message)> AddVehicleAsync(int customerId, AddVehicleDto dto)
        {
            var user = await userManager.FindByIdAsync(customerId.ToString());
            if (user == null) return (false, "Customer not found.");

            bool plateExists = await context.Vehicles.AnyAsync(v => v.PlateNumber == dto.PlateNumber);
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

            await context.Vehicles.AddAsync(vehicle);
            await context.SaveChangesAsync();
            return (true, "Vehicle added successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateVehicleAsync(int customerId, int vehicleId, UpdateVehicleDto dto)
        {
            var vehicle = await context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.CustomerId == customerId);
            if (vehicle == null) return (false, "Vehicle not found.");

            bool plateExists = await context.Vehicles
                .AnyAsync(v => v.PlateNumber == dto.PlateNumber && v.Id != vehicleId);
            if (plateExists) return (false, "A vehicle with this plate number already exists.");

            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;

            await context.SaveChangesAsync();
            return (true, "Vehicle updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteVehicleAsync(int customerId, int vehicleId)
        {
            var vehicle = await context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.CustomerId == customerId);
            if (vehicle == null) return (false, "Vehicle not found.");

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();
            return (true, "Vehicle deleted successfully.");
        }
    }
}
