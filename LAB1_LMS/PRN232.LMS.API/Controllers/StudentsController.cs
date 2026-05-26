using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service) => _service = service;

    /// <summary>Get paginated list of students</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<StudentResponse>>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _service.GetAllAsync(parameters);
        return Ok(ApiResponse<PagedResponse<StudentResponse>>.Ok(result));
    }

    /// <summary>Get student by ID with full enrollment details</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<StudentResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Student with id {id} not found."));
        return Ok(ApiResponse<StudentResponse>.Ok(result));
    }

    /// <summary>Create a new student</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<StudentResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Create([FromBody] StudentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.StudentId },
            ApiResponse<StudentResponse>.Ok(result, "Student created successfully."));
    }

    /// <summary>Update an existing student</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<StudentResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Update(int id, [FromBody] StudentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.UpdateAsync(id, request);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Student with id {id} not found."));

        return Ok(ApiResponse<StudentResponse>.Ok(result, "Student updated successfully."));
    }

    /// <summary>Delete a student</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail($"Student with id {id} not found."));

        return Ok(ApiResponse<object>.Ok((object?)null, "Student deleted successfully."));
    }
}
