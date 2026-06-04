using Hospital_Management_System.DTOs;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientsController(IPatientService service)
    {
        _service = service;
    }

    // GET
    [HttpGet]
    public IActionResult Get()
    {
        var data = _service.GetActivePatients();
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreatePatientDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var patient = new Patient
            {
                FullName = dto.FullName,
                Dob = dto.Dob,
                Gender = dto.Gender,
                Mob = dto.Mob,
                Email = dto.Email,
                Status = dto.Status
            };

            _service.AddPatient(patient);

            return StatusCode(201, new
            {
                message = "Patient created successfully"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = ex.Message
            });
        }
    }

    // PUT
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Patient patient)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Override ID from URL
            patient.Id = id;

            _service.UpdatePatient(patient);

            return NoContent(); // 204
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = ex.Message
            });
        }
    }

    // DELETE (Deactivate)
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.DeactivatePatient(id);
        return NoContent(); // 204
    }
}
