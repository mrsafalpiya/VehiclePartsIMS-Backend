using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        private int RequestingUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<CustomerProfileResponseDto>>> Register([FromBody] RegisterCustomerDto dto)
        {
            var (success, message, data) = await customerService.RegisterAsync(dto);
            if (!success) return BadRequest(ApiResponse<CustomerProfileResponseDto>.FailureResponse([message], message));
            return CreatedAtAction(nameof(GetProfile), ApiResponse<CustomerProfileResponseDto>.SuccessResponse(data!, message));
        }

        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<CustomerProfileResponseDto>>> GetProfile()
        {
            var profile = await customerService.GetProfileAsync(RequestingUserId);
            if (profile is null) return NotFound(ApiResponse<CustomerProfileResponseDto>.FailureResponse(["Customer not found."], "Not found"));
            return Ok(ApiResponse<CustomerProfileResponseDto>.SuccessResponse(profile));
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ApiResponse<CustomerProfileResponseDto>>> UpdateProfile([FromBody] UpdateCustomerProfileDto dto)
        {
            var (success, message, data) = await customerService.UpdateProfileAsync(RequestingUserId, dto);
            if (!success) return BadRequest(ApiResponse<CustomerProfileResponseDto>.FailureResponse([message], message));
            return Ok(ApiResponse<CustomerProfileResponseDto>.SuccessResponse(data!, message));
        }

        [HttpPut("password")]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var (success, message) = await customerService.ChangePasswordAsync(RequestingUserId, dto);
            if (!success) return BadRequest(ApiResponse<object>.FailureResponse([message], message));
            return Ok(ApiResponse<object>.SuccessResponse(null!, message));
        }

        [HttpGet("vehicles")]
        public async Task<ActionResult<ApiResponse<List<VehicleResponseDto>>>> GetVehicles()
        {
            var vehicles = await customerService.GetVehiclesAsync(RequestingUserId);
            return Ok(ApiResponse<List<VehicleResponseDto>>.SuccessResponse(vehicles));
        }

        [HttpPost("vehicles")]
        public async Task<ActionResult<ApiResponse<VehicleResponseDto>>> AddVehicle([FromBody] CreateVehicleDto dto)
        {
            var (success, message, data) = await customerService.AddVehicleAsync(RequestingUserId, dto);
            if (!success) return BadRequest(ApiResponse<VehicleResponseDto>.FailureResponse([message], message));
            return CreatedAtAction(nameof(GetVehicles), ApiResponse<VehicleResponseDto>.SuccessResponse(data!, message));
        }

        [HttpPut("vehicles/{id}")]
        public async Task<ActionResult<ApiResponse<VehicleResponseDto>>> UpdateVehicle(int id, [FromBody] UpdateVehicleDto dto)
        {
            var (success, message, data) = await customerService.UpdateVehicleAsync(id, RequestingUserId, dto);
            if (!success) return BadRequest(ApiResponse<VehicleResponseDto>.FailureResponse([message], message));
            return Ok(ApiResponse<VehicleResponseDto>.SuccessResponse(data!, message));
        }

        [HttpDelete("vehicles/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVehicle(int id)
        {
            var (success, message) = await customerService.DeleteVehicleAsync(id, RequestingUserId);
            if (!success) return BadRequest(ApiResponse<object>.FailureResponse([message], message));
            return Ok(ApiResponse<object>.SuccessResponse(null!, message));
        }
    }
}
