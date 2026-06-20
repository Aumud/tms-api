// --- The contract ---
using TmsApi.Entities;
public interface ICourseService
{
    Task<Course> CreateAsync(string Code, string Title, int Capacity);
    Task<Course?> GetByCodeAsync(string Code);
    Task<IReadOnlyList<Course>> GetAllAsync();
    Task<bool> DeleteAsync(string Code);
}

// --- The in-memory implementation ---
public class CourseService : ICourseService
{
    private readonly Dictionary<string, Course> _store = new();
    private readonly ILogger<CourseService> _logger;

    public CourseService(ILogger<CourseService> logger)
    {
        _logger = logger;
    }

    public Task<Course> CreateAsync(string Code, string Title, int Capacity)
    {
        if (_store.ContainsKey(Code))
        {
            _logger.LogWarning("Course {Code} already exists", Code);
            throw new InvalidOperationException($"Course with Code {Code} already exists");
        }

        var course = new Course
            {
                Code = Code,
                Title = Title,
                Capacity = Capacity
            };
        _store[Code] = course;

        _logger.LogInformation("Created course {Code} {Title}", Code, Title);
        return Task.FromResult(course);
    }

    public Task<Course?> GetByCodeAsync(string Code)
    {
        _store.TryGetValue(Code, out var course);

        if (course is null)
        {
            _logger.LogWarning("Course {Code} not found", Code);
        }

        return Task.FromResult(course);
    }

    public Task<IReadOnlyList<Course>> GetAllAsync()
    {
        IReadOnlyList<Course> all = _store.Values.ToList();
        return Task.FromResult(all);
    }

    public Task<bool> DeleteAsync(string Code)
    {
        var removed = _store.Remove(Code);

        if (removed)
            _logger.LogInformation("Deleted course {Code}", Code);
        else
            _logger.LogWarning("Delete failed course {Code} not found", Code);

        return Task.FromResult(removed);
    }
}

// --- The data shape ---
