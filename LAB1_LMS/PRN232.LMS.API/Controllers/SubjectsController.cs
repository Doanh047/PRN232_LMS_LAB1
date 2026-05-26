using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _service;

    public SubjectsController(ISubjectService service) => _service = service;

    /// <summary>Get paginated list of subjects</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<SubjectResponse>>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _service.GetAllAsync(parameters);
        return Ok(ApiResponse<PagedResponse<SubjectResponse>>.Ok(result));
    }

    /// <summary>Get subject by ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Subject with id {id} not found."));
        return Ok(ApiResponse<SubjectResponse>.Ok(result));
    }

    /// <summary>Create a new subject</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Create([FromBody] SubjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.SubjectId },
            ApiResponse<SubjectResponse>.Ok(result, "Subject created successfully."));
    }

    /// <summary>Update an existing subject</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Update(int id, [FromBody] SubjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request.", ModelState));

        var result = await _service.UpdateAsync(id, request);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Subject with id {id} not found."));

        return Ok(ApiResponse<SubjectResponse>.Ok(result, "Subject updated successfully."));
    }

    /// <summary>Delete a subject</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail($"Subject with id {id} not found."));

        return Ok(ApiResponse<object>.Ok((object?)null, "Subject deleted successfully."));
    }
}
