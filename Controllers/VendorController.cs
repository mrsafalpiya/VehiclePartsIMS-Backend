using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var vendors = await vendorService.GetAllAsync(search);
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vendor = await vendorService.GetByIdAsync(id);
            if (vendor is null) return NotFound("Vendor not found.");
            return Ok(vendor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVendorDto dto)
        {
            var created = await vendorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVendorDto dto)
        {
            var updated = await vendorService.UpdateAsync(id, dto);
            if (updated is null) return NotFound("Vendor not found.");
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await vendorService.DeleteAsync(id);
            if (!success) return BadRequest(message);
            return Ok(message);
        }
    }
}
