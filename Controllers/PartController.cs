using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartController(IPartService partService)
        {
            _partService = partService;
        }

        // GET /api/part
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parts = await _partService.GetAllAsync();
            return Ok(parts);
        }

        // GET /api/part/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var part = await _partService.GetByIdAsync(id);
            if (part is null) return NotFound("Part not found.");
            return Ok(part);
        }

        // POST /api/part
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePartDto dto)
        {
            var (success, message, data) = await _partService.CreateAsync(dto);
            if (!success) return BadRequest(message);
            return CreatedAtAction(nameof(GetById), new { id = data!.Id }, data);
        }

        // PUT /api/part/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePartDto dto)
        {
            var (success, message, data) = await _partService.UpdateAsync(id, dto);
            if (!success) return BadRequest(message);
            return Ok(data);
        }

        // DELETE /api/part/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _partService.DeleteAsync(id);
            if (!success) return BadRequest(message);
            return Ok(message);
        }
    }
}
