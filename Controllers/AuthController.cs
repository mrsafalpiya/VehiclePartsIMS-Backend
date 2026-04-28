using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ApiResponse<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
        {
            return await authService.LoginAsync(loginRequestDto);
        }
    }
}
