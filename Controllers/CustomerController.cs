using Microsoft.AspNetCore.Mvc;
using VehiclePartsIMS_Backend.Data.Dtos.Requests;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerDto dto)
        {
            var (success, message, data) = await _customerService.RegisterAsync(dto);
            if (!success) return BadRequest(new { message });
            return CreatedAtAction(nameof(Register), new { id = data!.Id }, data);
        }
    }
}