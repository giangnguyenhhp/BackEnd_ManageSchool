using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Department;

namespace ASP_Web_API.Services.Interface.InterfaceCoreModels;

public interface IDepartmentRepository
{
    public Task<IEnumerable<Department>> GetAllDepartments();
    public Task<Department> GetDepartment(int id);
    public Task<Department> UpdateDepartment(int id, UpdateDepartment department);
    public Task<Department> AddDepartment(CreateDepartment department);
    public Task<Department> DeleteDepartment(int id);
}