using KeelteKoolV2.Core.DTO;
using System;
using System.Threading.Tasks;

namespace KeelteKoolV2.Core.ServiceInterface
{
    public interface ILecturersServices
    {
        Task<LecturerDTO> Create(LecturerDTO dto);
        Task<LecturerDTO> DetailAsync(Guid id);
        Task<LecturerDTO> Update(LecturerDTO dto);
        Task<LecturerDTO> Delete(Guid id);
    }
}