using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // F6 — POST /api/Customer
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerDto dto)
        {
            var (success, message, data) = await _customerService.RegisterAsync(dto);
            if (!success) return BadRequest(new { message });
            return CreatedAtAction(nameof(GetProfile), new { id = data!.Id }, data);
        }

        // F8 — GET /api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var profile = await _customerService.GetProfileAsync(id);
            if (profile is null) return NotFound(new { message = "Customer not found." });
            return Ok(profile);
        }

        // F8 — PUT /api/Customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto dto)
        {
            var (success, message) = await _customerService.UpdateCustomerAsync(id, dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        // F8 — POST /api/Customer/{id}/vehicles
        [HttpPost("{id}/vehicles")]
        public async Task<IActionResult> AddVehicle(int id, [FromBody] AddVehicleDto dto)
        {
            var (success, message) = await _customerService.AddVehicleAsync(id, dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        // F8 — PUT /api/Customer/{id}/vehicles/{vehicleId}
        [HttpPut("{id}/vehicles/{vehicleId}")]
        public async Task<IActionResult> UpdateVehicle(int id, int vehicleId, [FromBody] UpdateVehicleDto dto)
        {
            var (success, message) = await _customerService.UpdateVehicleAsync(id, vehicleId, dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        // F8 — DELETE /api/Customer/{id}/vehicles/{vehicleId}
        [HttpDelete("{id}/vehicles/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int id, int vehicleId)
        {
            var (success, message) = await _customerService.DeleteVehicleAsync(id, vehicleId);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }
    }
}