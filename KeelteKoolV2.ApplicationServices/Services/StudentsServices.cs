using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class StudentsServices : IStudentsServices
    {
        private readonly KeelteKoolV2Context _context;
        public StudentsServices(KeelteKoolV2Context context)
        {
            _context = context;
        }
        public async Task<Student> CreateAsync(StudentDTO dto)
        {
            //check if not null
            if (dto == null)
            {
                return null;
            }
            //convert to domain-type
            Student domain = new Student();
            domain.Id = Guid.NewGuid();
            if (dto.FirstName.Length < 1)
            {
                dto.FirstName = "Cool";
            }
            if (dto.LastName.Length < 1)
            {
                dto.LastName = "Student";
            }
            domain.FirstName = dto.FirstName;
            domain.LastName = dto.LastName;
            domain.Email = dto.Email;

            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            await _context.Students.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public Task<Student> Delete(Guid id)
        {
            return null;
        }

        public Task<Student> DetailAsync(Guid id)
        {
            return null;
        }

        public Task<Student> Update(StudentDTO dto)
        {
            return null;
        }
    }
}
