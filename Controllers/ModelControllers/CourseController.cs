using ASP_Web_API.Models.CoreModels.Request.Course;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Controllers.ModelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            return Ok(await _courseRepository.GetAllCourses());
        }

        // GET: api/Course/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            return Ok(await _courseRepository.GetCourse(id));
        }

        // PUT: api/Course/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutCourse(int id, [FromBody] UpdateCourse course)
        {
            return Ok(await _courseRepository.UpdateCourse(id, course));
        }

        // POST: api/Course
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCourse([FromBody] CreateCourse course)
        {
            return Ok(await _courseRepository.AddCourse(course));
        }

        // DELETE: api/Course/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            return Ok(await _courseRepository.DeleteCourse(id));
        }
    }
}