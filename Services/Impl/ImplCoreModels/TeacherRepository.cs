using ASP_Web_API.Models;
using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Teacher;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Services.Impl.ImplCoreModels;

public class TeacherRepository : ITeacherRepository
{
    private readonly MasterDbContext _context;

    public TeacherRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachers()
    {
        return await _context.Teachers
            .Include(t=>t.Course)
            .Include(t=>t.Department)
            .ToListAsync();
    }

    public async Task<Teacher> GetTeacher(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher == null)
        {
            throw new Exception("Teacher not found!");
        }

        return teacher;
    }

    public async Task<Teacher> UpdateTeacher(int id, UpdateTeacher teacher)
    {
        var updateTeacher = await _context.Teachers
            .Include(t => t.Course)
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (updateTeacher == null)
        {
            throw new Exception("Teacher not found!");
        }

        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == teacher.DepartmentId);

        var listCourse = await _context.Courses
            .Where(c => teacher.CourseIds != null && teacher.CourseIds.Contains(c.Id)).ToListAsync();

        updateTeacher.DateOfBirth = teacher.DateOfBirth;
        updateTeacher.Name = teacher.Name;
        updateTeacher.Gender = teacher.Gender;
        updateTeacher.Department = department;
        updateTeacher.Course = listCourse;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TeacherExists(id))
            {
                throw new Exception("Teacher not found!");
            }
            else
            {
                throw;
            }
        }

        return updateTeacher;
    }

    public async Task<Teacher> AddTeacher(CreateTeacher teacher)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == teacher.DepartmentId);
        var listCourse = await _context.Courses
            .Where(c => teacher.CourseIds != null && teacher.CourseIds.Contains(c.Id)).ToListAsync();
        
        var newTeacher = new Teacher()
        {
            DateOfBirth = teacher.DateOfBirth,
            Gender = teacher.Gender,
            Name = teacher.Name,
            Department = department,
            Course = listCourse,
        };

        await _context.Teachers.AddAsync(newTeacher);
        await _context.SaveChangesAsync();

        return newTeacher;

    }

    public async Task<Teacher> DeleteTeacher(int id)
    {
        
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            throw new Exception("Teacher not found");
        }

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return teacher;
    }

    private bool TeacherExists(int id)
    {
        return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}