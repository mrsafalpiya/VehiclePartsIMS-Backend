using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.DTOs;
using VehiclePartsIMS_Backend.Services;

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

        /// <summary>
        /// F13: Customer creates a part request (Anyone with customer role)
        /// </summary>
        [HttpPost]
        //[Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> CreatePartRequest([FromBody] PartRequestCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { success = false, message = "Request data is required" });

                if (string.IsNullOrWhiteSpace(dto.PartName))
                    return BadRequest(new { success = false, message = "Part name is required" });

                var result = await _partRequestService.CreatePartRequestAsync(dto);
                return Ok(new { success = true, data = result, message = "Part request submitted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// F13: Get all part requests (Admin only)
        /// </summary>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPartRequests()
        {
            try
            {
                var results = await _partRequestService.GetAllPartRequestsAsync();
                return Ok(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// F13: Get pending part requests (Admin/Staff)
        /// </summary>
        [HttpGet("pending")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                var results = await _partRequestService.GetPendingRequestsAsync();
                return Ok(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// F13: Get part requests by customer ID
        /// </summary>
        [HttpGet("customer/{customerId}")]
        //[Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetRequestsByCustomer(int customerId)
        {
            try
            {
                var results = await _partRequestService.GetRequestsByCustomerAsync(customerId);
                return Ok(new { success = true, data = results });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// F13: Update part request status (Admin/Staff)
        /// </summary>
        [HttpPut("status")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateRequestStatus([FromBody] PartRequestUpdateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { success = false, message = "Update data is required" });

                var result = await _partRequestService.UpdateRequestStatusAsync(dto);
                return Ok(new { success = true, data = result, message = "Request status updated" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}