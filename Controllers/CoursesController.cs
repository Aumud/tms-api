using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/courses")]
public class CoursesController(ICourseService courseService)
    : ControllerBase
{
// GET api/courses
[HttpGet]
public async Task<IActionResult> GetAll()
    {
        var courses = await courseService.GetAllAsync();

        return Ok(courses);
        
    }
// GET api/courses/CS101
[HttpGet("{code}")]
public async Task<IActionResult> GetByCode(string code)
    {
        var course = await courseService.GetByCodeAsync(code);

            return course is not null
            ? Ok(course)
            : NotFound();
    }

// POST api/courses
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
    {
        var course = await courseService.CreateAsync(
            request.Code,
            request.Title,
            request.Capacity);

            return CreatedAtAction(
                nameof(GetByCode),
                new { code = course.Code },
                       course);
    }
// DELETE api/courses/CS101
[HttpDelete("{code}")]
public async Task<IActionResult> Delete(string code)
    {
        var deleted = await courseService.DeleteAsync(code);
            return deleted
                ? NoContent()
                : NotFound();
    }
}