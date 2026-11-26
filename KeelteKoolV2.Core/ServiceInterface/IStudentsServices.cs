using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeelteKoolV2.Core.DTO;

namespace KeelteKoolV2.Core.ServiceInterface
{
    public interface IStudentsServices
    {
        Task<IEnumerable<StudentDTO>> GetAllAsync();
        Task<StudentDTO> GetByIdAsync(Guid id);
        Task<StudentDTO> CreateAsync(StudentDTO dto);
        Task<StudentDTO> UpdateAsync(StudentDTO dto);
        Task<StudentDTO> DeleteAsync(Guid id);
    }
}