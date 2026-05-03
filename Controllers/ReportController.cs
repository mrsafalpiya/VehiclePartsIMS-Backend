using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET api/report/daily?date=2026-05-03
        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyReport([FromQuery] string date)
        {
            try
            {
                if (!DateOnly.TryParse(date, out var parsedDate))
                    return BadRequest(new { message = "Invalid date format. Use YYYY-MM-DD" });

                var result = await _reportService.GetDailyReportAsync(parsedDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET api/report/monthly?month=5&year=2026
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                if (month < 1 || month > 12)
                    return BadRequest(new { message = "Month must be between 1 and 12" });

                if (year < 2000 || year > 2100)
                    return BadRequest(new { message = "Invalid year" });

                var result = await _reportService.GetMonthlyReportAsync(month, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET api/report/yearly?year=2026
        [HttpGet("yearly")]
        public async Task<IActionResult> GetYearlyReport([FromQuery] int year)
        {
            try
            {
                if (year < 2000 || year > 2100)
                    return BadRequest(new { message = "Invalid year" });

                var result = await _reportService.GetYearlyReportAsync(year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}