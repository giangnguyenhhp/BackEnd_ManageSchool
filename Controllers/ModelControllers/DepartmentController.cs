using ASP_Web_API.Models.CoreModels.Request.Department;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Controllers.ModelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            return Ok(await _departmentRepository.GetAllDepartments());
        }

        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            return Ok(await _departmentRepository.GetDepartment(id));
        }

        // PUT: api/Department/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutDepartment(int id, [FromBody] UpdateDepartment department)
        {
            return Ok(await _departmentRepository.UpdateDepartment(id, department));
        }

        // POST: api/Department
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostDepartment([FromBody] CreateDepartment department)
        {
            return Ok(await _departmentRepository.AddDepartment(department));
        }

        // DELETE: api/Department/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            return Ok(await _departmentRepository.DeleteDepartment(id));
        }
    }
}