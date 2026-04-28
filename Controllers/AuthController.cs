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
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginRequestDto loginRequestDto)
        {
            var loginRes = await authService.LoginAsync(loginRequestDto);
            if (loginRes == null)
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.FailureResponse(["Invalid email or password"], "Invalid credentials"));
            }

            return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(loginRes, "Login successful"));
        }
    }
}
