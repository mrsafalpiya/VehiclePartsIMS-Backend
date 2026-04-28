using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceReviewController : ControllerBase
    {
        private readonly IServiceReviewService _serviceReviewService;

        public ServiceReviewController(IServiceReviewService serviceReviewService)
        {
            _serviceReviewService = serviceReviewService;
        }

        // POST api/servicereview?customerId=1
        [HttpPost]
        public async Task<IActionResult> SubmitReview([FromBody] CreateServiceReviewDto dto, [FromQuery] int customerId)
        {
            try
            {
                var result = await _serviceReviewService.SubmitReviewAsync(customerId, dto);
                return CreatedAtAction(nameof(SubmitReview), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}