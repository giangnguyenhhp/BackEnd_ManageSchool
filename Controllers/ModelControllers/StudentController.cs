using ASP_Web_API.Models.AuthModels;
using ASP_Web_API.Models.CoreModels.Request.Student;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Controllers.ModelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: api/Student
        [HttpGet]
        [Authorize(Roles = SystemPermission.ReadPermission)]
        public async Task<IActionResult> GetStudents()
        {
            return Ok(await _studentRepository.GetAllStudents());
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        [Authorize(Roles = SystemPermission.ReadPermission)]
        public async Task<IActionResult> GetStudent(int id)
        {
            return Ok(await _studentRepository.GetStudentAsync(id));
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        [Authorize(Roles = SystemPermission.UpdatePermission)]
        public async Task<IActionResult> PutStudent(int id,[FromBody] UpdateStudent student)
        {
            return Ok(await _studentRepository.UpdateStudentAsync(id, student));
        }

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = SystemPermission.CreatePermission)]
        public async Task<IActionResult> PostStudent([FromBody] CreateStudent student)
        {
            return Ok(await _studentRepository.AddStudentAsync(student));
        }

        // DELETE: api/Student/5
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = SystemPermission.DeletePermission)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            return Ok(await _studentRepository.DeleteStudentAsync(id));
        }
        
    }
}