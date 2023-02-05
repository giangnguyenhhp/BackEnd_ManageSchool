namespace ASP_Web_API.Models.CoreModels.Request.Teacher;

public class UpdateTeacher
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public string DateOfBirth { get; set; }
    public int? DepartmentId { get; set; }
    public IEnumerable<int>? CourseIds { get; set; }
}