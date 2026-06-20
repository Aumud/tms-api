using TmsApi.DTOs;
public class DashboardResponse
{


    public List<StudentSummary> Students { get; set; }
    public List<CourseSummary> TopCourses { get; set; }
    public int TotalStudents { get; set; }
}


public class CourseSummary
{
    public string Course { get; set; }
    public int EnrollmentCount { get; set; }
}
