using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class CustomerService(UserManager<User> userManager, AppDbContext context) : ICustomerService
    {
        private static CustomerProfileResponseDto MapToProfileDto(User user) => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            HomeAddress = user.HomeAddress ?? string.Empty
        };

        private static VehicleResponseDto MapToVehicleDto(Vehicle vehicle) => new()
        {
            Id = vehicle.Id,
            PlateNumber = vehicle.PlateNumber,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year
        };

        public async Task<(bool Success, string Message, CustomerProfileResponseDto? Data)> RegisterAsync(RegisterCustomerDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return (false, "Password and confirm password do not match.", null);

            var existingUser = await userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return (false, "A user with this email already exists.", null);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                EmailConfirmed = true,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                HomeAddress = dto.HomeAddress
            };

            var createResult = await userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(" ", createResult.Errors.Select(e => e.Description));
                return (false, errors, null);
            }

            await userManager.AddToRoleAsync(user, "Customer");

            return (true, "Registration successful.", MapToProfileDto(user));
        }

        public async Task<CustomerProfileResponseDto?> GetProfileAsync(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return user is null ? null : MapToProfileDto(user);
        }

        public async Task<(bool Success, string Message, CustomerProfileResponseDto? Data)> UpdateProfileAsync(int userId, UpdateCustomerProfileDto dto)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return (false, "Customer not found.", null);

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.HomeAddress = dto.HomeAddress;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return (false, "Failed to update profile.", null);

            return (true, "Profile updated successfully.", MapToProfileDto(user));
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
                return (false, "New password and confirm password do not match.");

            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return (false, "Customer not found.");

            var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            return (true, "Password changed successfully.");
        }

        public async Task<List<VehicleResponseDto>> GetVehiclesAsync(int userId)
        {
            return await context.Vehicles
                .Where(v => v.CustomerId == userId)
                .Select(v => MapToVehicleDto(v))
                .ToListAsync();
        }

        public async Task<(bool Success, string Message, VehicleResponseDto? Data)> AddVehicleAsync(int userId, CreateVehicleDto dto)
        {
            bool plateExists = await context.Vehicles.AnyAsync(v => v.PlateNumber == dto.PlateNumber);
            if (plateExists)
                return (false, "A vehicle with this plate number already exists.", null);

            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return (false, "Customer not found.", null);

            var vehicle = new Vehicle
            {
                CustomerId = userId,
                Customer = user,
                PlateNumber = dto.PlateNumber,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year
            };

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            return (true, "Vehicle added successfully.", MapToVehicleDto(vehicle));
        }

        public async Task<(bool Success, string Message, VehicleResponseDto? Data)> UpdateVehicleAsync(int vehicleId, int userId, UpdateVehicleDto dto)
        {
            var vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId && v.CustomerId == userId);
            if (vehicle is null)
                return (false, "Vehicle not found.", null);

            bool plateExists = await context.Vehicles.AnyAsync(v => v.PlateNumber == dto.PlateNumber && v.Id != vehicleId);
            if (plateExists)
                return (false, "A vehicle with this plate number already exists.", null);

            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;

            await context.SaveChangesAsync();

            return (true, "Vehicle updated successfully.", MapToVehicleDto(vehicle));
        }

        public async Task<(bool Success, string Message)> DeleteVehicleAsync(int vehicleId, int userId)
        {
            var vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId && v.CustomerId == userId);
            if (vehicle is null)
                return (false, "Vehicle not found.");

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            return (true, "Vehicle deleted successfully.");
        }
    }
}
