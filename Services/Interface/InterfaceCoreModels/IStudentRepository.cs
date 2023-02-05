using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Student;

namespace ASP_Web_API.Services.Interface.InterfaceCoreModels;

public interface IStudentRepository
{
    public Task<Student> UpdateStudentAsync(int id, UpdateStudent student);
    public Task<IEnumerable<Student>> GetAllStudents();
    public Task<Student> GetStudentAsync(int id);
    public Task<Student> AddStudentAsync(CreateStudent student);
    public Task<Student> DeleteStudentAsync(int id);
}