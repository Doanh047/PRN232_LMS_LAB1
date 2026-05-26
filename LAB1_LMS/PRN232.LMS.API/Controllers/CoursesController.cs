using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service) => _service = service;

    /// <summary>Get paginated list of courses</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<CourseResponse>>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _service.GetAllAsync(parameters);
        return Ok(ApiResponse<PagedResponse<CourseResponse>>.Ok(result));
    }

    /// <summary>Get course by ID with semester and subject details</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CourseResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Course with id {id} not found."));
        return Ok(ApiResponse<CourseResponse>.Ok(result));
    }

    /// <summary>Create a new course</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CourseResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Create([FromBody] CourseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.CourseId },
            ApiResponse<CourseResponse>.Ok(result, "Course created successfully."));
    }

    /// <summary>Update an existing course</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CourseResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Update(int id, [FromBody] CourseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.UpdateAsync(id, request);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Course with id {id} not found."));

        return Ok(ApiResponse<CourseResponse>.Ok(result, "Course updated successfully."));
    }

    /// <summary>Delete a course</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail($"Course with id {id} not found."));

        return Ok(ApiResponse<object>.Ok((object?)null, "Course deleted successfully."));
    }

    /// <summary>Get enrollments of a specific course. Use expand=student to include student details.</summary>
    [HttpGet("{id:int}/enrollments")]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<EnrollmentResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetEnrollments(int id, [FromQuery] QueryParameters parameters)
    {
        var result = await _service.GetEnrollmentsByCourseIdAsync(id, parameters);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Course with id {id} not found."));

        return Ok(ApiResponse<PagedResponse<EnrollmentResponse>>.Ok(result));
    }
}
