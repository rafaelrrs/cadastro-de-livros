using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;

namespace AppLivroCadastro.Application.Interfaces
{
    public interface ILivroService
    {
        Task<IEnumerable<LivroDTO>> GetAllAsync();
        Task<LivroDTO> GetByIdAsync(int id);
        Task<LivroDTO> AddAsync(CreateLivroDTO createLivroDTO);
        Task UpdateAsync(int id, CreateLivroDTO createLivroDTO);
        Task DeleteAsync(int id);
    }
}