using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Course;

namespace ASP_Web_API.Services.Interface.InterfaceCoreModels;

public interface ICourseRepository
{
    public Task<IEnumerable<Course>> GetAllCourses();
    public Task<Course> GetCourse(int id);
    public Task<Course> UpdateCourse(int id, UpdateCourse course);
    public Task<Course> AddCourse(CreateCourse course);
    public Task<Course> DeleteCourse(int id);
}