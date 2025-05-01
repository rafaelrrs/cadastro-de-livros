using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;

namespace AppLivroCadastro.Application.Interfaces
{
    public interface IAssuntoService
    {
        Task<IEnumerable<AssuntoDTO>> GetAllAsync();
        Task<AssuntoDTO> GetByIdAsync(int id);
        Task<AssuntoDTO> AddAsync(CreateAssuntoDTO assuntoDTO);
        Task UpdateAsync(int id, CreateAssuntoDTO assuntoDTO);
        Task DeleteAsync(int id);
    }
}