using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/staff-customers")]
    [Authorize(Roles = "Staff,Admin")]
    public class StaffCustomerController(IStaffCustomerService staffCustomerService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CustomerResponseDto>>> GetAll()
        {
            var customers = await staffCustomerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> Register([FromBody] StaffRegisterCustomerDto dto)
        {
            var (success, message, data) = await staffCustomerService.RegisterAsync(dto);
            if (!success) return BadRequest(new { message });
            return CreatedAtAction(nameof(GetProfile), new { id = data!.Id }, data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffCustomerProfileResponseDto>> GetProfile(int id)
        {
            var profile = await staffCustomerService.GetProfileAsync(id);
            if (profile is null) return NotFound(new { message = "Customer not found." });
            return Ok(profile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto dto)
        {
            var (success, message) = await staffCustomerService.UpdateCustomerAsync(id, dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        [HttpPost("{id}/vehicles")]
        public async Task<IActionResult> AddVehicle(int id, [FromBody] AddVehicleDto dto)
        {
            var (success, message) = await staffCustomerService.AddVehicleAsync(id, dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        [HttpPut("{id}/vehicles/{vehicleId}")]
        public async Task<IActionResult> UpdateVehicle(int id, int vehicleId, [FromBody] UpdateVehicleDto dto)
        {
            var (success, message) = await staffCustomerService.UpdateVehicleAsync(id, vehicleId, dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        [HttpDelete("{id}/vehicles/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int id, int vehicleId)
        {
            var (success, message) = await staffCustomerService.DeleteVehicleAsync(id, vehicleId);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }
    }
}
