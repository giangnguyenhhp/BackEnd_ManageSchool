using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Specialization;

namespace ASP_Web_API.Services.Interface.InterfaceCoreModels;

public interface ISpecializationRepository
{
    public Task<IEnumerable<Specialization>> GetAllSpecializations();
    public Task<Specialization> GetSpecialization(int id);
    public Task<Specialization> UpdateSpecialization(int id, UpdateSpecialization specialization);
    public Task<Specialization> AddSpecialization(CreateSpecialization specialization);
    public Task<Specialization> DeleteSpecialization(int id);
}