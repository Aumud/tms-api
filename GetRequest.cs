public record EnrollmentRecord(
    string Id,
    string StudentId,
    string CourseCode,
    DateTime EnrolledAt);

// // public record Student(
// //     string RegistrationNumber,
// //     string Name,
// //     int Age,
// //     decimal GPA);

public record CreateStudentRequest(
    string RegistrationNumber,
    string Name,
    int Age,
    decimal GPA);

// // public record Course(
// //     string Code,
// //     string Title,
// //     int Capacity);

public record CreateCourseRequest(
    string Code,
    string Title,
    int Capacity);
