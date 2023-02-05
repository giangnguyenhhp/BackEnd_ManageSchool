namespace ASP_Web_API.Models.CoreModels.Request.Course;

public class CreateCourse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Credit { get; set; }
    
    
    public IEnumerable<int>? StudentIds { get; set; }
    public IEnumerable<int>? TeacherIds { get; set; }
}