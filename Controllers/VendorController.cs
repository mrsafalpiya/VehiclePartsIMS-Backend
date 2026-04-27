using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        // GET /api/vendor?search=abc
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var vendors = await _vendorService.GetAllAsync(search);
            return Ok(vendors);
        }

        // GET /api/vendor/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vendor = await _vendorService.GetByIdAsync(id);
            if (vendor is null) return NotFound("Vendor not found.");
            return Ok(vendor);
        }

        // POST /api/vendor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVendorDto dto)
        {
            var created = await _vendorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/vendor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVendorDto dto)
        {
            var updated = await _vendorService.UpdateAsync(id, dto);
            if (updated is null) return NotFound("Vendor not found.");
            return Ok(updated);
        }

        // DELETE /api/vendor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _vendorService.DeleteAsync(id);
            if (!success) return BadRequest(message);
            return Ok(message);
        }
    }
}
