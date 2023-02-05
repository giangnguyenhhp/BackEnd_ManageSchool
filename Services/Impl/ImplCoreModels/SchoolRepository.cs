using ASP_Web_API.Models;
using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.School;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Services.Impl.ImplCoreModels;

public class SchoolRepository : ISchoolRepository
{
    private readonly MasterDbContext _context;

    public SchoolRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<School>> GetAllSchools()
    {
        return await _context.Schools
            .Include(s=>s.Departments)
            .ToListAsync();
    }

    public async Task<School> GetSchool(int id)
    {
        var school = await _context.Schools.FindAsync(id);

        if (school == null)
        {
            throw new Exception("School not found!");
        }

        return school;
    }

    public async Task<School> UpdateSchool(int id, UpdateSchool school)
    {
        var updateSchool = await _context.Schools
            .Include(s=>s.Departments)
            .FirstOrDefaultAsync(s=>s.Id == id);
        if (updateSchool == null)
        {
            throw new Exception("School not found!");
        }

        var listDepartments = await _context.Departments
            .Where(c => school.DepartmentIds != null && school.DepartmentIds.Contains(c.Id)).ToListAsync();

        updateSchool.Name = school.Name;
        updateSchool.Description = school.Description;
        updateSchool.Departments = listDepartments;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SchoolExists(id))
            {
                throw new Exception("School not found!");
            }
            else
            {
                throw;
            }
        }

        return updateSchool;
    }

    public async Task<School> AddSchool(CreateSchool school)
    {
        var listDepartments = await _context.Departments
            .Where(c => school.DepartmentIds != null && school.DepartmentIds.Contains(c.Id)).ToListAsync();

        var newSchool = new School()
        {
            Name = school.Name,
            Description = school.Description,
            Departments = listDepartments,
        };

        await _context.Schools.AddAsync(newSchool);
        await _context.SaveChangesAsync();

        return newSchool;
    }

    public async Task<School> DeleteSchool(int id)
    {
        var school = await _context.Schools.FindAsync(id);
        if (school == null)
        {
            throw new Exception("School not found");
        }

        _context.Schools.Remove(school);
        await _context.SaveChangesAsync();

        return school;
    }
    
    private bool SchoolExists(int id)
    {
        return (_context.Schools?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}