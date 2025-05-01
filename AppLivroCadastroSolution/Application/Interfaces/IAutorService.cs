using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;

namespace AppLivroCadastro.Application.Interfaces
{
    public interface IAutorService
    {
        Task<IEnumerable<AutorDTO>> GetAllAsync();
        Task<AutorDTO> GetByIdAsync(int id);
        Task<AutorDTO> AddAsync(CreateAutorDTO autorDTO);
        Task UpdateAsync(int id, CreateAutorDTO autorDTO);
        Task DeleteAsync(int id);
    }
}
