using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/roster-report")]
public class RosterReportController(TmsDbContext context) : ControllerBase
{
    // Exercise 3 - 1) Paged list of students (page size 20, stable sort by name)
    // GET api/roster-report/students?pageNumber=1
    [HttpGet("students")]
    public async Task<IActionResult> GetStudents(
        [FromQuery] int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        const int pageSize = 20;
        if (pageNumber < 1) pageNumber = 1;

        var students = await context.Students
            .OrderBy(s => s.Name) // stable sort requirement
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Ok(students);
    }

    // Exercise 3 - 2) Summary query: top 5 courses by enrollment count
    // GET api/roster-report/top-courses
    [HttpGet("top-courses")]
    public async Task<IActionResult> GetTopCoursesByEnrollmentCount(
        CancellationToken cancellationToken = default)
    {
        var topCourses = await context.Enrollments
            .GroupBy(e => new { e.CourseId, e.Course.Title })
            .Select(g => new
            {
                CourseTitle = g.Key.Title,
                EnrollmentCount = g.Count()
            })
            .OrderByDescending(x => x.EnrollmentCount)
            .Take(5)
            .ToListAsync(cancellationToken);

        return Ok(topCourses);
    }
}

