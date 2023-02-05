using ASP_Web_API.Models.CoreModels.Request.Teacher;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Controllers.ModelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        // GET: api/Teacher
        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            return Ok(await _teacherRepository.GetAllTeachers());
        }

        // GET: api/Teacher/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            return Ok(await _teacherRepository.GetTeacher(id));
        }

        // PUT: api/Teacher/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutTeacher(int id,[FromBody] UpdateTeacher teacher)
        {
            return Ok(await _teacherRepository.UpdateTeacher(id, teacher));
        }

        // POST: api/Teacher
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostTeacher([FromBody] CreateTeacher teacher)
        {
            return Ok(await _teacherRepository.AddTeacher(teacher));
        }

        // DELETE: api/Teacher/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            return Ok(await _teacherRepository.DeleteTeacher(id));
        }
        
    }
}