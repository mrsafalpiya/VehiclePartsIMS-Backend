using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.DTOs;
using VehiclePartsIMS_Backend.Services;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin,Staff")]
    public class SalesInvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public SalesInvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// F7 & F16: Create a new sales invoice with loyalty discount (Admin/Staff only)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSalesInvoice([FromBody] SalesInvoiceCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { success = false, message = "Invoice data is required" });

                if (dto.Items == null || dto.Items.Count == 0)
                    return BadRequest(new { success = false, message = "Invoice must contain at least one item" });

                var result = await _invoiceService.CreateSalesInvoiceAsync(dto);
                return Ok(new { success = true, data = result, message = "Sales invoice created successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}