using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET api/appointment  (Staff only)
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<ApiResponse<List<AppointmentResponseDto>>>> GetAll()
        {
            var result = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(ApiResponse<List<AppointmentResponseDto>>.SuccessResponse(result));
        }

        // POST api/appointment?customerId=1
        [HttpPost]
        public async Task<IActionResult> BookAppointment([FromBody] CreateAppointmentDto dto, [FromQuery] int customerId)
        {
            try
            {
                var result = await _appointmentService.BookAppointmentAsync(customerId, dto);
                return CreatedAtAction(nameof(BookAppointment), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET api/appointment/my?customerId=1
        [HttpGet("my")]
        public async Task<IActionResult> GetMyAppointments([FromQuery] int customerId)
        {
            try
            {
                var result = await _appointmentService.GetMyAppointmentsAsync(customerId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}