namespace ASP_Web_API.Models.CoreModels.Request.Student;

public class CreateStudent
{
    public string Name { get; set; }
    public string DateOfBirth { get; set; }
    public string Gender { get; set; }

    public int? SpecializationId { get; set; }
    public IEnumerable<int>? CourseIds { get; set; }
}