using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Controllers.Api
{
    [ApiController]
    [Route("api/students")]
    public class StudentsApiController : ControllerBase
    {
        private readonly IStudentsServices _studentsService;

        public StudentsApiController(IStudentsServices studentsService)
        {
            _studentsService = studentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentsService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            var student = await _studentsService.GetByIdAsync(id);
            if (student == null) return NotFound("Student not found.");

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentDTO dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest("FirstName, LastName and Email are required.");
            }

            dto.CreatedAt = DateTime.Now;
            var created = await _studentsService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StudentDTO dto)
        {
            if (id == Guid.Empty || dto == null || id != dto.Id)
                return BadRequest("ID mismatch or invalid body.");

            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest("FirstName, LastName and Email are required.");
            }

            var updated = await _studentsService.UpdateAsync(dto);
            if (updated == null) return NotFound("Student not found.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            var deleted = await _studentsService.DeleteAsync(id);
            if (deleted == null) return NotFound("Student not found.");

            return NoContent();
        }
    }
}