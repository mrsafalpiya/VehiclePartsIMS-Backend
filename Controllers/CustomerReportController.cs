using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Responses;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/customer-reports")]
    [Authorize(Roles = "Staff,Admin")]
    public class CustomerReportController(IReportService reportService) : ControllerBase
    {
        // GET api/customer-reports/regular
        [HttpGet("regular")]
        public async Task<ActionResult<ApiResponse<List<RegularCustomerReportItemDto>>>> GetRegularCustomersReport()
        {
            var data = await reportService.GetRegularCustomersReportAsync();
            return Ok(ApiResponse<List<RegularCustomerReportItemDto>>.SuccessResponse(data));
        }

        // GET api/customer-reports/high-spenders
        [HttpGet("high-spenders")]
        public async Task<ActionResult<ApiResponse<List<HighSpenderReportItemDto>>>> GetHighSpendersReport()
        {
            var data = await reportService.GetHighSpendersReportAsync();
            return Ok(ApiResponse<List<HighSpenderReportItemDto>>.SuccessResponse(data));
        }

        // GET api/customer-reports/pending-credits
        [HttpGet("pending-credits")]
        public async Task<ActionResult<ApiResponse<List<PendingCreditReportItemDto>>>> GetPendingCreditsReport()
        {
            var data = await reportService.GetPendingCreditsReportAsync();
            return Ok(ApiResponse<List<PendingCreditReportItemDto>>.SuccessResponse(data));
        }
    }
}
