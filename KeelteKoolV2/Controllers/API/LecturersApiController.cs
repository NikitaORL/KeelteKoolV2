using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Controllers.Api
{
    [ApiController]
    [Route("api/lecturers")]
    public class LecturersApiController : ControllerBase
    {
        private readonly ILecturersServices _lecturersServices;

        public LecturersApiController(ILecturersServices lecturersServices)
        {
            _lecturersServices = lecturersServices;
        }

        // Отдельная модель без IFormFile, чтобы Swagger корректно строил JSON
        public class LecturerApiRequest
        {
            public Guid Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Qualifications { get; set; }
            public string? UserID { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lecturers = await _lecturersServices.GetAllAsync();
            return Ok(lecturers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            var lecturer = await _lecturersServices.DetailAsync(id);
            if (lecturer == null) return NotFound("Lecturer not found.");

            return Ok(lecturer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LecturerApiRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Qualifications))
            {
                return BadRequest("FirstName, LastName and Qualifications are required.");
            }

            var dto = new LecturerDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Qualifications = request.Qualifications,
                UserID = request.UserID
            };

            var created = await _lecturersServices.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] LecturerApiRequest request)
        {
            if (id == Guid.Empty || request == null || id != request.Id)
                return BadRequest("ID mismatch or invalid body.");

            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Qualifications))
            {
                return BadRequest("FirstName, LastName and Qualifications are required.");
            }

            var existing = await _lecturersServices.DetailAsync(id);
            if (existing == null) return NotFound("Lecturer not found.");

            var dto = new LecturerDTO
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Qualifications = request.Qualifications,
                UserID = request.UserID
            };

            var updated = await _lecturersServices.Update(dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid ID.");

            var deleted = await _lecturersServices.Delete(id);
            if (deleted == null) return NotFound("Lecturer not found.");

            return NoContent();
        }
    }
}