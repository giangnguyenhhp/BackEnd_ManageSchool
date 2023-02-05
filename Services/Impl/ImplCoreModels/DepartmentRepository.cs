using ASP_Web_API.Models;
using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Department;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Services.Impl.ImplCoreModels;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly MasterDbContext _context;

    public DepartmentRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllDepartments()
    {
        return await _context.Departments
            .Include(d => d.Teachers)
            .Include(d => d.School)
            .ToListAsync();
    }

    public async Task<Department> GetDepartment(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
        {
            throw new Exception("Department not found!");
        }

        return department;
    }

    public async Task<Department> UpdateDepartment(int id, UpdateDepartment department)
    {
        var updateDepartment = await _context.Departments
            .Include(d => d.Teachers)
            .Include(d => d.School)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (updateDepartment == null)
        {
            throw new Exception("Department not found!");
        }

        var school = await _context.Schools.FindAsync(department.SchoolId);
        var listTeachers = await _context.Teachers
            .Where(c => department.TeacherIds != null && department.TeacherIds.Contains(c.Id)).ToListAsync();

        updateDepartment.Name = department.Name;
        updateDepartment.Description = department.Description;
        updateDepartment.School = school;
        updateDepartment.Teachers = listTeachers;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DepartmentExists(id))
            {
                throw new Exception("Department not found!");
            }
            else
            {
                throw;
            }
        }

        return updateDepartment;
    }

    public async Task<Department> AddDepartment(CreateDepartment department)
    {
        var school = await _context.Schools.FindAsync(department.SchoolId);
        var listTeachers = await _context.Teachers
            .Where(c => department.TeacherIds != null && department.TeacherIds.Contains(c.Id)).ToListAsync();

        var newDepartment = new Department()
        {
            Name = department.Name,
            Description = department.Description,
            School = school,
            Teachers = listTeachers,
        };

        await _context.Departments.AddAsync(newDepartment);
        await _context.SaveChangesAsync();

        return newDepartment;
    }

    public async Task<Department> DeleteDepartment(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
        {
            throw new Exception("Department not found");
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return department;
    }


    private bool DepartmentExists(int id)
    {
        return (_context.Departments?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}