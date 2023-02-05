using ASP_Web_API.Models;
using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Specialization;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Services.Impl.ImplCoreModels;

public class SpecializationRepository : ISpecializationRepository
{
    private readonly MasterDbContext _context;

    public SpecializationRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Specialization>> GetAllSpecializations()
    {
        return await _context.Specializations
            .Include(s=>s.Students)
            .ToListAsync();
    }

    public async Task<Specialization> GetSpecialization(int id)
    {
        var specialization = await _context.Specializations.FindAsync(id);

        if (specialization == null)
        {
            throw new Exception("Specialization not found!");
        }

        return specialization;
    }

    public async Task<Specialization> UpdateSpecialization(int id, UpdateSpecialization specialization)
    {
        var updateSpecialization = await _context.Specializations
            .Include(s => s.Students)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (updateSpecialization == null)
        {
            throw new Exception("Specialization not found!");
        }

        var listStudents = await _context.Students
            .Where(c => specialization.StudentIds != null && specialization.StudentIds.Contains(c.Id))
            .ToListAsync();

        updateSpecialization.Name = specialization.Name;
        updateSpecialization.Description = specialization.Description;
        updateSpecialization.Students = listStudents;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SpecializationExists(id))
            {
                throw new Exception("Specialization not found!");
            }
            else
            {
                throw;
            }
        }

        return updateSpecialization;
    }

    public async Task<Specialization> AddSpecialization(CreateSpecialization specialization)
    {
        var listStudents = await _context.Students
            .Where(c => specialization.StudentIds != null && specialization.StudentIds.Contains(c.Id))
            .ToListAsync();

        var newSpecialization = new Specialization()
        {
            Name = specialization.Name,
            Description = specialization.Description,
            Students = listStudents,
        };

        await _context.Specializations.AddAsync(newSpecialization);
        await _context.SaveChangesAsync();

        return newSpecialization;
    }

    public async Task<Specialization> DeleteSpecialization(int id)
    {
        var specialization = await _context.Specializations.FindAsync(id);
        if (specialization == null)
        {
            throw new Exception("Specialization not found");
        }

        _context.Specializations.Remove(specialization);
        await _context.SaveChangesAsync();

        return specialization;
    }

    private bool SpecializationExists(int id)
    {
        return (_context.Specializations?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}