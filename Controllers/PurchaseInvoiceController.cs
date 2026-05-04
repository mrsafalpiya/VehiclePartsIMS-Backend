using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.DTOs;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PurchaseInvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public PurchaseInvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// F4: Create a new purchase invoice (Admin only)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePurchaseInvoice([FromBody] PurchaseInvoiceCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { success = false, message = "Invoice data is required" });

                if (dto.Items == null || dto.Items.Count == 0)
                    return BadRequest(new { success = false, message = "Invoice must contain at least one item" });

                var result = await _invoiceService.CreatePurchaseInvoiceAsync(dto);
                return Ok(new { success = true, data = result, message = "Purchase invoice created successfully" });
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
        /// F4: Get all purchase invoices (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseInvoices()
        {
            try
            {
                var result = await _invoiceService.GetAllPurchaseInvoicesAsync();
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// F4: Get a single purchase invoice by id (Admin only)
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPurchaseInvoice(int id)
        {
            try
            {
                var result = await _invoiceService.GetPurchaseInvoiceByIdAsync(id);
                if (result == null)
                    return NotFound(new { success = false, message = $"Invoice {id} not found" });
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}