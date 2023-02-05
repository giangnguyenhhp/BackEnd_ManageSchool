using ASP_Web_API.Models.CoreModels.Request.Specialization;
using ASP_Web_API.Services.Interface.InterfaceCoreModels;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Controllers.ModelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationRepository _specializationRepository;
        public SpecializationController(ISpecializationRepository specializationRepository)
        {
            _specializationRepository = specializationRepository;
        }

        // GET: api/Specialization
        [HttpGet]
        public async Task<IActionResult> GetSpecializations()
        {
            return Ok(await _specializationRepository.GetAllSpecializations());
        }

        // GET: api/Specialization/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecialization(int id)
        {
            return Ok(await _specializationRepository.GetSpecialization(id));
        }

        // PUT: api/Specialization/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutSpecialization(int id, [FromBody] UpdateSpecialization specialization)
        {
            return Ok(await _specializationRepository.UpdateSpecialization(id, specialization));
        }

        // POST: api/Specialization
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostSpecialization([FromBody] CreateSpecialization specialization)
        {
            return Ok(await _specializationRepository.AddSpecialization(specialization));
        }

        // DELETE: api/Specialization/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            return Ok(await _specializationRepository.DeleteSpecialization(id));
        }


    }
}
