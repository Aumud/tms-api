using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
{
// GET/api/enrollments returns all enrollment records
[HttpGet]
public async Task<IActionResult> GetAll()
{
var enrollments = await enrollmentService.GetAllAsync();
return Ok(enrollments);
}
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest request)
{
var record = await enrollmentService.EnrollAsync(request.StudentId, request.CourseCode);
return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
}
// GET/api/enrollments/{id} returns one or 404
[HttpGet("{id}")]
public async Task<IActionResult> GetById(string id)
{
var record = await enrollmentService.GetByIdAsync(id);
return record is not null ? Ok(record) : NotFound();
}

[HttpDelete("{id}")]
public async Task<IActionResult> Delete(string id)
{
var deleted = await enrollmentService.DeleteAsync(id);
return deleted ? NoContent() : NotFound();
}
[Route("api/[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        throw new Exception("Intentional test error");
    }
}
}


public record CreateEnrollmentRequest(string StudentId, string CourseCode);
