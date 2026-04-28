using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Data.Entities;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class AuthService(SignInManager<User> signInManager, UserManager<User> userManager, IJwtService jwtService) : IAuthService
    {
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user == null)
            {
                return null;
            }
            var userRole = (await userManager.GetRolesAsync(user))[0];
            if (userRole != loginRequestDto.Role)
            {
                return null;
            }
            var signInResult = await signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, false);
            if (!signInResult.Succeeded)
            {
                return null;
            }

            var jwtToken = jwtService.GenerateToken(user.Id, user.FullName, user.Email, loginRequestDto.Role);

            return new LoginResponseDto {
                Token = jwtToken,
                UserId = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Role = loginRequestDto.Role
            };
        }
    }
}
