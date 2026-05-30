using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SemestersController : ControllerBase
{
    private readonly ISemesterService _service;
    private readonly ICourseService _courseService;

    public SemestersController(ISemesterService service, ICourseService courseService)
    {
        _service = service;
        _courseService = courseService;
    }

    /// <summary>Get paginated list of semesters</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<SemesterResponse>>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _service.GetAllAsync(parameters);
        return Ok(ApiResponse<PagedResponse<SemesterResponse>>.Ok(result));
    }

    /// <summary>Get semester by ID with list of courses</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Semester with id {id} not found."));
        return Ok(ApiResponse<SemesterResponse>.Ok(result));
    }

    /// <summary>Create a new semester</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Create([FromBody] SemesterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.SemesterId },
            ApiResponse<SemesterResponse>.Ok(result, "Semester created successfully."));
    }

    /// <summary>Update an existing semester</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Update(int id, [FromBody] SemesterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.UpdateAsync(id, request);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Semester with id {id} not found."));

        return Ok(ApiResponse<SemesterResponse>.Ok(result, "Semester updated successfully."));
    }

    /// <summary>Delete a semester</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail($"Semester with id {id} not found."));

        return Ok(ApiResponse<object>.Ok((object?)null, "Semester deleted successfully."));
    }

    /// <summary>Create a new course attached to a specific semester</summary>
    [HttpPost("{id:int}/courses")]
    [ProducesResponseType(typeof(ApiResponse<CourseResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> CreateCourse(int id, [FromBody] CourseRequest request)
    {
        var semester = await _service.GetByIdAsync(id);
        if (semester is null)
            return NotFound(ApiResponse<object>.Fail($"Semester with id {id} not found."));

        request.SemesterId = id;

        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _courseService.CreateAsync(request);
        return CreatedAtAction(
            nameof(CoursesController.GetById),
            "Courses",
            new { id = result.CourseId },
            ApiResponse<CourseResponse>.Ok(result, "Course created successfully."));
    }
}
