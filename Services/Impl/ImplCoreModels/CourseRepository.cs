using ASP_Web_API.Models;
using ASP_Web_API.Models.CoreModels;
using ASP_Web_API.Models.CoreModels.Request.Course;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Services.Impl.ImplCoreModels;

public class CourseRepository : ICourseRepository
{
    private readonly MasterDbContext _context;

    public CourseRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllCourses()
    {
        return await _context.Courses
            .Include(c=>c.Students)
            .Include(c=>c.Teacher)
            .ToListAsync();
    }

    public async Task<Course> GetCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            throw new Exception("Course not found!");
        }

        return course;
    }

    public async Task<Course> UpdateCourse(int id, UpdateCourse course)
    {
        var updateCourse = await _context.Courses
            .Include(c=>c.Students)
            .Include(c=>c.Teacher)
            .FirstOrDefaultAsync(c=>c.Id == id);
        if (updateCourse == null)
        {
            throw new Exception("Course not found!");
        }

        var listStudents = await _context.Students
            .Where(c => course.StudentIds != null && course.StudentIds.Contains(c.Id)).ToListAsync();
        var listTeachers = await _context.Teachers
            .Where(c => course.TeacherIds != null && course.TeacherIds.Contains(c.Id)).ToListAsync();

        updateCourse.Name = course.Name;
        updateCourse.Credit = course.Credit;
        updateCourse.Description = course.Description;
        updateCourse.Students = listStudents;
        updateCourse.Teacher = listTeachers;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(id))
            {
                throw new Exception("Course not found!");
            }
            else
            {
                throw;
            }
        }

        return updateCourse;
    }

    public async Task<Course> AddCourse(CreateCourse course)
    {
        var listStudents = await _context.Students
            .Where(c => course.StudentIds != null && course.StudentIds.Contains(c.Id)).ToListAsync();
        var listTeachers = await _context.Teachers
            .Where(c => course.TeacherIds != null && course.TeacherIds.Contains(c.Id)).ToListAsync();
        
        var newCourse = new Course()
        {
            Name = course.Name,
            Credit = course.Credit,
            Description = course.Description,
            Students = listStudents,
            Teacher = listTeachers,
        };

        await _context.Courses.AddAsync(newCourse);
        await _context.SaveChangesAsync();

        return newCourse;
    }

    public async Task<Course> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            throw new Exception("Course not found");
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return course;
    }
    
    private bool CourseExists(int id)
    {
        return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}