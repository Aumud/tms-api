// --- The contract ---
using TmsApi.Entities;
public interface IStudentService
{
    Task<Student> CreateAsync(string RegistrationNumber, string name, int age, decimal gpa);
    Task<Student?> GetByIdAsync(string RegistrationNumber);
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<bool> DeleteAsync(string RegistrationNumber);
}

// --- The in-memory implementation ---
public class StudentService : IStudentService
{
    private readonly Dictionary<string, Student> _store = new();
    private readonly ILogger<StudentService> _logger;

    public StudentService(ILogger<StudentService> logger)
    {
        _logger = logger;
    }

    public Task<Student> CreateAsync(string RegistrationNumber, string name, int age, decimal gpa)
    {
        if (_store.ContainsKey(RegistrationNumber))
        {
            _logger.LogWarning("Student {Id} already exists", RegistrationNumber);
            throw new InvalidOperationException($"Student with id {RegistrationNumber} already exists");
        }

        var student = new Student
            {
                RegistrationNumber = RegistrationNumber,
                Name = name,
                GPA = gpa,
                IsActive = true
            };
            _store[RegistrationNumber] = student;

        _logger.LogInformation("Created student {Id} {Name}", RegistrationNumber, name);
        return Task.FromResult(student);
    }

    public Task<Student?> GetByIdAsync(string RegistrationNumber)
    {
        _store.TryGetValue(RegistrationNumber, out var student);

        if (student is null)
        {
            _logger.LogWarning("Student {Id} not found", RegistrationNumber);
        }

        return Task.FromResult(student);
    }

    public Task<IReadOnlyList<Student>> GetAllAsync()
    {
        IReadOnlyList<Student> all = _store.Values.ToList();
        return Task.FromResult(all);
    }

    public Task<bool> DeleteAsync(string RegistrationNumber)
    {
        var removed = _store.Remove(RegistrationNumber);

        if (removed)
            _logger.LogInformation("Deleted student {Id}", RegistrationNumber);
        else
            _logger.LogWarning("Delete failed student {Id} not found", RegistrationNumber);

        return Task.FromResult(removed);
    }
}

// --- The data shape ---
