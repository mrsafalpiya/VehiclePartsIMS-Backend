using Microsoft.AspNetCore.Identity;
using VehiclePartsIMS_Backend.Data;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class StaffService(UserManager<User> userManager, AppDbContext dbContext) : IStaffService
    {
        private static readonly string[] AllowedRoles = ["Admin", "Staff"];

        private static StaffResponseDto MapToDto(User user, string role) => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Role = role
        };

        public async Task<List<StaffResponseDto>> GetAllAsync()
        {
            var result = new List<StaffResponseDto>();

            foreach (var role in AllowedRoles)
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(role);
                result.AddRange(usersInRole.Select(u => MapToDto(u, role)));
            }

            return result;
        }

        public async Task<StaffResponseDto?> GetByIdAsync(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;

            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault(r => AllowedRoles.Contains(r));
            if (role == null) return null;

            return MapToDto(user, role);
        }

        public async Task<(bool Success, string Message, StaffResponseDto? Data)> CreateAsync(CreateStaffDto dto)
        {
            if (!AllowedRoles.Contains(dto.Role))
                return (false, "Role must be 'Admin' or 'Staff'.", null);

            if (dto.Password != dto.ConfirmPassword)
                return (false, "Password and confirm password do not match.", null);

            var existingUser = await userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return (false, "A user with this email already exists.", null);

            var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var user = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    EmailConfirmed = true,
                    UserName = dto.Email,
                    PhoneNumber = dto.PhoneNumber
                };

                var createResult = await userManager.CreateAsync(user, dto.Password);
                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    var errors = createResult.Errors.Select(e => e.Description).ToList();
                    return (false, "Failed to create staff account.", null);
                }

                var roleResult = await userManager.AddToRoleAsync(user, dto.Role);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    var errors = roleResult.Errors.Select(e => e.Description).ToList();
                    return (false, "Failed to assign role to staff account.", null);
                }

                await transaction.CommitAsync();

                return (true, "Staff account created successfully.", MapToDto(user, dto.Role));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, "Failed to create staff account.", null);
            }
        }

        public async Task<(bool Success, string Message, StaffResponseDto? Data)> UpdateAsync(int id, UpdateStaffDto dto)
        {
            if (!AllowedRoles.Contains(dto.Role))
                return (false, "Role must be 'Admin' or 'Staff'.", null);

            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return (false, "Staff member not found.", null);

            // Ensure the user is an Admin or Staff (not a Customer)
            var currentRoles = await userManager.GetRolesAsync(user);
            if (!currentRoles.Any(r => AllowedRoles.Contains(r)))
                return (false, "Staff member not found.", null);

            // Validate email uniqueness (excluding self)
            var userWithEmail = await userManager.FindByEmailAsync(dto.Email);
            if (userWithEmail != null && userWithEmail.Id != id)
                return (false, "A user with this email already exists.", null);

            // Validate password change if provided
            if (dto.Password != null || dto.ConfirmPassword != null)
            {
                if (dto.Password != dto.ConfirmPassword)
                    return (false, "Password and confirm password do not match.", null);
            }

            // Update fields
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.NormalizedEmail = dto.Email.ToUpperInvariant();
            user.NormalizedUserName = dto.Email.ToUpperInvariant();
            user.PhoneNumber = dto.PhoneNumber;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return (false, "Failed to update staff account.", null);

            // Update role if changed
            var currentRole = currentRoles.FirstOrDefault(r => AllowedRoles.Contains(r))!;
            if (currentRole != dto.Role)
            {
                await userManager.RemoveFromRoleAsync(user, currentRole);
                await userManager.AddToRoleAsync(user, dto.Role);
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(dto.Password))
            {
                await userManager.RemovePasswordAsync(user);
                await userManager.AddPasswordAsync(user, dto.Password);
            }

            return (true, "Staff account updated successfully.", MapToDto(user, dto.Role));
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id, int requestingUserId)
        {
            if (id == requestingUserId)
                return (false, "You cannot delete your own account.");

            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return (false, "Staff member not found.");

            // Ensure the user is an Admin or Staff (not a Customer)
            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Any(r => AllowedRoles.Contains(r)))
                return (false, "Staff member not found.");

            var deleteResult = await userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
                return (false, "Failed to delete staff account.");

            return (true, "Staff account deleted successfully.");
        }
    }
}
