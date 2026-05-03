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
    [Authorize(Roles = "Admin")]
    public class StaffController(IStaffService staffService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<StaffResponseDto>>>> GetAll()
        {
            var staff = await staffService.GetAllAsync();
            return Ok(ApiResponse<List<StaffResponseDto>>.SuccessResponse(staff));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StaffResponseDto>>> GetById(int id)
        {
            var staff = await staffService.GetByIdAsync(id);
            if (staff is null) return NotFound(ApiResponse<StaffResponseDto>.FailureResponse(["Staff member not found."], "Not found"));
            return Ok(ApiResponse<StaffResponseDto>.SuccessResponse(staff));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<StaffResponseDto>>> Create([FromBody] CreateStaffDto dto)
        {
            var (success, message, data) = await staffService.CreateAsync(dto);
            if (!success) return BadRequest(ApiResponse<StaffResponseDto>.FailureResponse([message], message));
            return CreatedAtAction(nameof(GetById), new { id = data!.Id }, ApiResponse<StaffResponseDto>.SuccessResponse(data, message));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StaffResponseDto>>> Update(int id, [FromBody] UpdateStaffDto dto)
        {
            var (success, message, data) = await staffService.UpdateAsync(id, dto);
            if (!success) return BadRequest(ApiResponse<StaffResponseDto>.FailureResponse([message], message));
            return Ok(ApiResponse<StaffResponseDto>.SuccessResponse(data!, message));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (success, message) = await staffService.DeleteAsync(id, requestingUserId);
            if (!success) return BadRequest(ApiResponse<object>.FailureResponse([message], message));
            return Ok(ApiResponse<object>.SuccessResponse(null!, message));
        }
    }
}
