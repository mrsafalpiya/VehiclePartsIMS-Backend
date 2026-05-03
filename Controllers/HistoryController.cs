using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        // GET api/history/purchases?customerId=1
        [HttpGet("purchases")]
        public async Task<IActionResult> GetPurchaseHistory([FromQuery] int customerId)
        {
            var result = await _historyService.GetPurchaseHistoryAsync(customerId);
            return Ok(result);
        }

        // GET api/history/appointments?customerId=1
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointmentHistory([FromQuery] int customerId)
        {
            var result = await _historyService.GetAppointmentHistoryAsync(customerId);
            return Ok(result);
        }
    }
}