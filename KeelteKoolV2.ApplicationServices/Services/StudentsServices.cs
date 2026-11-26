using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class StudentsService : IStudentsServices
    {
        private readonly KeelteKoolV2Context _context;

        public StudentsService(KeelteKoolV2Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllAsync()
        {
            return await _context.Students
                .Select(x => new StudentDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<StudentDTO> GetByIdAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            return new StudentDTO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                CreatedAt = student.CreatedAt
            };
        }

        public async Task<StudentDTO> CreateAsync(StudentDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var student = new Student
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                CreatedAt = dto.CreatedAt == default ? DateTime.Now : dto.CreatedAt
            };

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            dto.Id = student.Id;
            return dto;
        }

        public async Task<StudentDTO> UpdateAsync(StudentDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var student = await _context.Students.FindAsync(dto.Id);
            if (student == null) return null;

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<StudentDTO> DeleteAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return new StudentDTO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                CreatedAt = student.CreatedAt
            };
        }
    }
}
