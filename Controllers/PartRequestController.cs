using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartRequestController : ControllerBase
    {
        private readonly IPartRequestService _partRequestService;

        public PartRequestController(IPartRequestService partRequestService)
        {
            _partRequestService = partRequestService;
        }

        // POST api/partrequest?customerId=1
        [HttpPost]
        public async Task<IActionResult> SubmitPartRequest([FromBody] CreatePartRequestDto dto, [FromQuery] int customerId)
        {
            try
            {
                var result = await _partRequestService.SubmitPartRequestAsync(customerId, dto);
                return CreatedAtAction(nameof(SubmitPartRequest), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}