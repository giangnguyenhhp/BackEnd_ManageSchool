namespace ASP_Web_API.Models.CoreModels.Request.Department;

public class CreateDepartment
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    
    public IEnumerable<int>? TeacherIds { get; set; }
    public int? SchoolId { get; set; }
}