namespace ASP_Web_API.Models.CoreModels.Request.School;

public class UpdateSchool
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    
    public IEnumerable<int>? DepartmentIds { get; set; }
}