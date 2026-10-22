using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeelteKoolV2.Core.ServiceInterface
{
    public interface IStudentsServices
    {
        Task<Student> Create(StudentDTO dto);
        Task<Student> Update(StudentDTO dto);
        Task<Student> DetailAsync(Guid id);
        Task<Student> Delete(Guid id);
    }
}
