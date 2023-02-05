using ASP_Web_API.Models;
using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Student;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Services.Impl.ImplCoreModels;

public class StudentRepository : IStudentRepository
{
    private readonly MasterDbContext _context;

    public StudentRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<Student> UpdateStudentAsync(int id, UpdateStudent student)
    {
        var updateStudent = await _context.Students
            .Include(s=>s.Course)
            .Include(s=>s.Specialization)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (updateStudent == null)
        {
            throw new Exception("Student not found");
        }

        var specialization = await _context.Specializations.FindAsync(student.SpecializationId);
        var listCourses = await _context.Courses
            .Where(c => student.CourseIds != null && student.CourseIds.Contains(c.Id)).ToListAsync();

        updateStudent.Name = student.Name;
        updateStudent.Gender = student.Gender;
        updateStudent.DateOfBirth = student.DateOfBirth;
        updateStudent.Course = listCourses;
        updateStudent.Specialization = specialization;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(id))
            {
                throw new Exception("Student not found");
            }
            else
            {
                throw;
            }
        }

        return updateStudent;
    }

    public async Task<IEnumerable<Student>> GetAllStudents()
    {
        if (_context.Students == null)
        {
            throw new Exception("Entity set 'MasterDbContext.Students'  is null.");
        }

        return await _context.Students
            .Include(s=>s.Course)
            .Include(s=>s.Specialization)
            .ToListAsync();
    }

    public async Task<Student> GetStudentAsync(int id)
    {
        if (_context.Students == null)
        {
            throw new Exception("Entity set 'MasterDbContext.Students'  is null.");
        }

        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            throw new Exception("Student not found");
        }

        return student;
    }

    public async Task<Student> AddStudentAsync(CreateStudent student)
    {
        if (_context.Students == null)
        {
            throw new Exception("Entity set 'MasterDbContext.Students'  is null.");
        }
        var specialization = await _context.Specializations.FindAsync(student.SpecializationId);
        var listCourse = await _context.Courses
            .Where(c => student.CourseIds != null && student.CourseIds.Contains(c.Id)).ToListAsync();

        var newStudent = new Student()
        {
            Name = student.Name,
            DateOfBirth = student.DateOfBirth,
            Gender = student.Gender,
            Course = listCourse,
            Specialization = specialization
        };

        await _context.Students.AddAsync(newStudent);
        await _context.SaveChangesAsync();

        return newStudent;
    }

    public async Task<Student> DeleteStudentAsync(int id)
    {
        if (_context.Students == null)
        {
            throw new Exception("Entity set 'MasterDbContext.Students'  is null.");
        }

        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            throw new Exception("Student not found");
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return student;
    }

    private bool StudentExists(int id)
    {
        return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}