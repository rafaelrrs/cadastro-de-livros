using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;

namespace AppLivroCadastro.Application.Interfaces
{
    public interface IFormaCompraService
    {
        Task<IEnumerable<FormaCompraDTO>> GetAllAsync();
        Task<FormaCompraDTO> GetByIdAsync(int id);
        Task<FormaCompraDTO> AddAsync(CreateFormaCompraDTO formaCompraDTO);
        Task UpdateAsync(int id, CreateFormaCompraDTO formaCompraDTO);
        Task DeleteAsync(int id);
    }
}
