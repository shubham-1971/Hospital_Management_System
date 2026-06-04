using Hospital_Management_System.DTOs;
using Hospital_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _service.AddAppointment(dto);
                return StatusCode(201, new { message = "Appointment created" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message }); // 400
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        // 2. CANCEL APPOINTMENT (sp_CancelAppointment)
        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            try
            {
                _service.CancelAppointment(id);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new { message = ex.Message }); // 404

                if (ex.Message.Contains("Only scheduled"))
                    return Conflict(new { message = ex.Message }); // 409

                return StatusCode(500, new { message = ex.Message });
            }
        }

        // 3. UPCOMING APPOINTMENTS (sp_GetUpcomingAppointments)
        [HttpGet("upcoming")]
        public IActionResult GetUpcoming()
        {
            try
            {
                var data = _service.GetUpcomingAppointments();

                if (data == null || data.Count == 0)
                    return NotFound(new { message = "No upcoming appointments found" });

                return Ok(data); // 200
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // 4. BY DOCTOR (sp_GetAppointmentsByDoctor)
        [HttpGet("doctor/{doctorId}")]
        public IActionResult GetByDoctor(int doctorId)
        {
            try
            {
                var data = _service.GetAppointmentsByDoctor(doctorId);

                if (data == null || data.Count == 0)
                    return NotFound(new { message = "No appointments found for this doctor" });

                return Ok(data);
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // 5. CONSOLIDATED VIEW (sp_GetAppointmentDetails)
        [HttpGet("report/details")]
        public IActionResult GetDetails()
        {
            try
            {
                var data = _service.GetAppointmentDetails();

                if (data == null || data.Count == 0)
                    return NotFound(new { message = "No data available" });

                return Ok(data);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    message = "Database error occurred",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    detail = ex.Message
                });
            }
        }

        // 6. TOP DOCTORS (sp_GetDoctorsWithMoreAppointments)
        [HttpGet("report/top-doctors")]
        public IActionResult GetTopDoctors()
        {
            try
            {
                var data = _service.GetDoctorsWithMoreAppointments();

                if (data.Count == 0)
                    return NotFound(new { message = "No doctors found" });

                return Ok(data);
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // 7. REVENUE (sp_GetRevenueBySpecialization)
        [HttpGet("report/revenue")]
        public IActionResult GetRevenue()
        {
            try
            {
                var data = _service.GetRevenueBySpecialization();

                if (data.Count == 0)
                    return NotFound(new { message = "No revenue data" });

                return Ok(data);
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // 8. DUPLICATE APPOINTMENTS (sp_GetDuplicateAppointments)
        [HttpGet("report/duplicates")]
        public IActionResult GetDuplicates()
        {
            try
            {
                var data = _service.GetDuplicateAppointments();

                if (data.Count == 0)
                    return NotFound(new { message = "No duplicate appointments" });

                return Ok(data);
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // 9. NEXT 7 DAYS (sp_GetNext7DaysAppointments)
        [HttpGet("report/next7days")]
        public IActionResult GetNext7Days()
        {
            try
            {
                var data = _service.GetNext7DaysAppointments();

                if (data.Count == 0)
                    return NotFound(new { message = "No upcoming appointments in next 7 days" });

                return Ok(data);
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}