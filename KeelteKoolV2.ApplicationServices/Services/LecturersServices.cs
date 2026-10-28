using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class LecturersServices : ILecturersServices
    {
        private readonly KeelteKoolV2Context _context;
        public LecturersServices(KeelteKoolV2Context context)
        {
            _context = context;
        }
        public async Task<LecturerDTO> Create(LecturerDTO dto)
        {
            if (dto == null) return null;

            var domain = new Lecturer
            {
                Id = Guid.NewGuid(),
                FirstName = string.IsNullOrEmpty(dto.FirstName) ? "Jane" : dto.FirstName,
                LastName = string.IsNullOrEmpty(dto.LastName) ? "Doe" : dto.LastName,
                Qualifications = dto.Qualifications,
                UserID = dto.UserID,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            await _context.Lecturers.AddAsync(domain);
            await _context.SaveChangesAsync();

            dto.Id = domain.Id;
            return dto;
        }

        public async Task<LecturerDTO> Delete(Guid id)
        {
            var domain = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == id);
            if (domain == null) return null;

            _context.Lecturers.Remove(domain);
            await _context.SaveChangesAsync();

            return new LecturerDTO
            {
                Id = domain.Id,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                Qualifications = domain.Qualifications
            };
        }

        public async Task<LecturerDTO> DetailAsync(Guid id)
        {
            var domain = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == id);
            if (domain == null) return null;

            return new LecturerDTO
            {
                Id = domain.Id,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                Qualifications = domain.Qualifications,
                UserID = domain.UserID
            };
        }

        public async Task<LecturerDTO> Update(LecturerDTO dto)
        {
            var domain = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (domain == null) return null;

            domain.FirstName = dto.FirstName;
            domain.LastName = dto.LastName;
            domain.Qualifications = dto.Qualifications;
            domain.ModifiedAt = DateTime.Now;

            _context.Lecturers.Update(domain);
            await _context.SaveChangesAsync();

            return dto;
        }
    }
}
