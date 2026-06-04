namespace Hospital_Management_System.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Hospital_Management_System.Services.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorsController(IDoctorService service)
        {
            _service = service;
        }

        // GET with filter
        [HttpGet]
        public IActionResult Get([FromQuery] string? specialization, [FromQuery] bool? available)
        {
            try
            {
                var data = _service.GetDoctors(specialization, available);

                if (data == null || data.Count == 0)
                    return NotFound(new { message = "No doctors found" });

                return Ok(data); // 200
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error"
                });
            }
        }
    }
}
