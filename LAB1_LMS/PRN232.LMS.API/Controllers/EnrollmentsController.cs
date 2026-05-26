using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service) => _service = service;

    /// <summary>Get paginated list of enrollments. Use expand=student,course to include related data.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<EnrollmentResponse>>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _service.GetAllAsync(parameters);
        return Ok(ApiResponse<PagedResponse<EnrollmentResponse>>.Ok(result));
    }

    /// <summary>Get enrollment by ID with student and course details</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Enrollment with id {id} not found."));
        return Ok(ApiResponse<EnrollmentResponse>.Ok(result));
    }

    /// <summary>Create a new enrollment</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Create([FromBody] EnrollmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.EnrollmentId },
            ApiResponse<EnrollmentResponse>.Ok(result, "Enrollment created successfully."));
    }

    /// <summary>Update an existing enrollment</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Update(int id, [FromBody] EnrollmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.UpdateAsync(id, request);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Enrollment with id {id} not found."));

        return Ok(ApiResponse<EnrollmentResponse>.Ok(result, "Enrollment updated successfully."));
    }

    /// <summary>Delete an enrollment</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail($"Enrollment with id {id} not found."));

        return Ok(ApiResponse<object>.Ok((object?)null, "Enrollment deleted successfully."));
    }
}
