using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.School;

namespace ASP_Web_API.Services.Interface.InterfaceCoreModels;

public interface ISchoolRepository
{
    public Task<IEnumerable<School>> GetAllSchools();
    public Task<School> GetSchool(int id);
    public Task<School> UpdateSchool(int id, UpdateSchool s);
    public Task<School> AddSchool(CreateSchool school);
    public Task<School> DeleteSchool(int id);
}