using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Teacher;

namespace ASP_Web_API.Services.Interface.InterfaceCoreModels;

public interface ITeacherRepository
{
    public Task<IEnumerable<Teacher>> GetAllTeachers();
    public Task<Teacher> GetTeacher(int id);
    public Task<Teacher> UpdateTeacher(int id, UpdateTeacher teacher);
    public Task<Teacher> AddTeacher(CreateTeacher teacher);
    public Task<Teacher> DeleteTeacher(int id);
}