using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class PartController(IPartService partService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parts = await partService.GetAllAsync();
            return Ok(parts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var part = await partService.GetByIdAsync(id);
            if (part is null) return NotFound("Part not found.");
            return Ok(part);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePartDto dto)
        {
            var (success, message, data) = await partService.CreateAsync(dto);
            if (!success) return BadRequest(message);
            return CreatedAtAction(nameof(GetById), new { id = data!.Id }, data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePartDto dto)
        {
            var (success, message, data) = await partService.UpdateAsync(id, dto);
            if (!success) return BadRequest(message);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await partService.DeleteAsync(id);
            if (!success) return BadRequest(message);
            return Ok(message);
        }
    }
}
