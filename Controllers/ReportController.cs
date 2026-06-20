using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TmsApi.Data;

namespace Report.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportingServiceController : ControllerBase
    {
        private readonly TmsDbContext _context;

        public ReportingServiceController(TmsDbContext context)
        {
            _context = context;
        }

        // 1. Active students with GPA >= 3.0
        [HttpGet("active-students-gpa")]
        public async Task<IActionResult> GetActiveStudentsWithHighGPA()
        {
            var count = await _context.Students
                .Where(s => s.IsActive && s.GPA >= 3.0m)
                .CountAsync();

            return Ok(count);
        }

        // 2. Courses with most enrollments
        [HttpGet("courses-most-enrollments")]
        public async Task<IActionResult> GetCoursesWithMostEnrollments()
        {
            var list = await _context.Courses
                .Select(c => new
                {
                    c.Title,
                    EnrollmentCount = c.Enrollments.Count
                })
                .OrderByDescending(x => x.EnrollmentCount)
                .ToListAsync();

            return Ok(list);
        }

        // 3. Average GPA per course
        [HttpGet("average-gpa-per-course")]
        public async Task<IActionResult> GetAverageGpaPerCourse()
        {
            var list = await _context.Enrollments
                .GroupBy(e => e.Course.Title)
                .Select(g => new
                {
                    Course = g.Key,
                    AverageGPA = g.Average(e => e.Student.GPA)
                })
                .ToListAsync();

            return Ok(list);
        }

        // 4. Students with zero enrollments
        [HttpGet("students-no-enrollments")]
        public async Task<IActionResult> GetStudentsWithNoEnrollments()
        {
            // Approach A: Subquery (NOT EXISTS)
            var listA = await _context.Students
                .Where(s => !s.Enrollments.Any())
                .Select(s => s.Name)
                .ToListAsync();

            // Approach B: Left Join (IS NULL)
            var listB = await _context.Students
                .GroupJoin(_context.Enrollments,
                    s => s.Id,
                    e => e.StudentId,
                    (s, e) => new { s, e })
                .SelectMany(
                    x => x.e.DefaultIfEmpty(),
                    (x, e) => new { x.s, e })
                .Where(x => x.e == null)
                .Select(x => x.s.Name)
                .ToListAsync();

            return Ok(new { SubqueryPattern = listA, LeftJoinPattern = listB });
    }
}

}
