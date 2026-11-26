using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeelteKoolV2.Controllers.Api
{
    [ApiController]
    [Route("api/languagecourses")]
    public class LanguageCoursesApiController : ControllerBase
    {
        private readonly KeelteKoolV2Context _context;
        private readonly ILanguageCoursesServices _languageCoursesServices;

        public LanguageCoursesApiController(
            KeelteKoolV2Context context,
            ILanguageCoursesServices languageCoursesServices)
        {
            _context = context;
            _languageCoursesServices = languageCoursesServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.LanguageCourses.ToListAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            var course = await _languageCoursesServices.DetailsAsync(id);
            if (course == null) return NotFound("Course not found.");

            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LanguageCourseDTO dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Nimetus) ||
                string.IsNullOrWhiteSpace(dto.Keel) ||
                string.IsNullOrWhiteSpace(dto.Tase) ||
                string.IsNullOrWhiteSpace(dto.Kirjeldus))
            {
                return BadRequest("Nimetus, Keel, Tase and Kirjeldus are required.");
            }

            var created = await _languageCoursesServices.Create(dto);
            if (created == null) return BadRequest("Failed to create course.");

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] LanguageCourseDTO dto)
        {
            if (id == Guid.Empty || dto == null || dto.Id != id)
                return BadRequest("ID mismatch or invalid body.");

            if (string.IsNullOrWhiteSpace(dto.Nimetus) ||
                string.IsNullOrWhiteSpace(dto.Keel) ||
                string.IsNullOrWhiteSpace(dto.Tase) ||
                string.IsNullOrWhiteSpace(dto.Kirjeldus))
            {
                return BadRequest("Nimetus, Keel, Tase and Kirjeldus are required.");
            }

            // Проверяем существование и забираем старый CreatedAt, чтобы сервис не упал на (DateTime)dto.CreatedAt
            var existing = await _context.LanguageCourses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null) return NotFound("Course not found.");

            dto.CreatedAt = existing.CreatedAt;

            var updated = await _languageCoursesServices.Update(dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            // Проверяем до вызова сервиса, т.к. сервис не проверяет null перед Remove()
            var existing = await _languageCoursesServices.DetailsAsync(id);
            if (existing == null) return NotFound("Course not found.");

            await _languageCoursesServices.Delete(id);
            return NoContent();
        }
    }
}