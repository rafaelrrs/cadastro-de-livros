using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;
using AppLivroCadastro.Application.Interfaces;
using AppLivroCadastro.Domain.Entities;
using AppLivroCadastro.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppLivroCadastro.Application.Services
{
    public class FormaCompraService : IFormaCompraService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<FormaCompraService> _logger;

        public FormaCompraService(AppDbContext dbContext, IMapper mapper, ILogger<FormaCompraService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FormaCompraDTO>> GetAllAsync()
        {
            try
            {
                var formasCompra = await _dbContext.FormasCompra.ToListAsync();
                return _mapper.Map<IEnumerable<FormaCompraDTO>>(formasCompra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as formas de compra.");
                throw;
            }
        }

        public async Task<FormaCompraDTO> GetByIdAsync(int id)
        {
            try
            {
                var formaCompra = await _dbContext.FormasCompra.FindAsync(id);
                if (formaCompra == null)
                {
                    _logger.LogWarning($"Forma de compra com ID {id} não encontrada.");
                    return null;
                }
                return _mapper.Map<FormaCompraDTO>(formaCompra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter forma de compra por ID {id}.", id);
                throw;
            }
        }

        public async Task<FormaCompraDTO> AddAsync(CreateFormaCompraDTO formaCompraDTO)
        {
            try
            {
                var formaCompra = _mapper.Map<FormaCompra>(formaCompraDTO);
                _dbContext.FormasCompra.Add(formaCompra);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Forma de compra com ID {formaCompra.FormaCompraID} adicionada com sucesso.");
                return _mapper.Map<FormaCompraDTO>(formaCompra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar forma de compra.");
                throw;
            }
        }

        public async Task UpdateAsync(int id, CreateFormaCompraDTO formaCompraDTO)
        {
            try
            {
                var formaCompra = await _dbContext.FormasCompra.FindAsync(id);
                if (formaCompra == null)
                {
                    _logger.LogWarning($"Forma de compra com ID {id} não encontrada ao atualizar.");
                    throw new KeyNotFoundException($"Forma de compra com ID {id} não encontrada.");
                }
                _mapper.Map(formaCompraDTO, formaCompra);
                _dbContext.Entry(formaCompra).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Forma de compra com ID {id} atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar forma de compra com ID {id}.", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var formaCompra = await _dbContext.FormasCompra.FindAsync(id);
                if (formaCompra == null)
                {
                    _logger.LogWarning($"Forma de compra com ID {id} não encontrada ao deletar.");
                    throw new KeyNotFoundException($"Forma de compra com ID {id} não encontrada.");
                }
                _dbContext.FormasCompra.Remove(formaCompra);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Forma de compra com ID {id} deletada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar forma de compra com ID {id}.", id);
                throw;
            }
        }
    }
}
