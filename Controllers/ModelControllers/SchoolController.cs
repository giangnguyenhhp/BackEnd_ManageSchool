using ASP_Web_API.Models.CoreModels.Request.School;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Controllers.ModelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolController(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        // GET: api/School
        [HttpGet]
        public async Task<IActionResult> GetSchools()
        {
            return Ok(await _schoolRepository.GetAllSchools());
        }

        // GET: api/School/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchool(int id)
        {
            return Ok(await _schoolRepository.GetSchool(id));
        }

        // PUT: api/School/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutSchool(int id, [FromBody] UpdateSchool school)
        {
            return Ok(await _schoolRepository.UpdateSchool(id, school));
        }

        // POST: api/School
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostSchool([FromBody] CreateSchool school)
        {
            return Ok(await _schoolRepository.AddSchool(school));
        }

        // DELETE: api/School/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            return Ok(await _schoolRepository.DeleteSchool(id));
        }
    }
}
