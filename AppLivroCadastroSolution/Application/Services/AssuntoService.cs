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
    public class AssuntoService : IAssuntoService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AssuntoService> _logger;

        public AssuntoService(AppDbContext dbContext, IMapper mapper, ILogger<AssuntoService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AssuntoDTO>> GetAllAsync()
        {
            try
            {
                var assuntos = await _dbContext.Assuntos.ToListAsync();
                return _mapper.Map<IEnumerable<AssuntoDTO>>(assuntos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os assuntos.");
                throw;
            }
        }

        public async Task<AssuntoDTO> GetByIdAsync(int id)
        {
            try
            {
                var assunto = await _dbContext.Assuntos.FindAsync(id);
                if (assunto == null)
                {
                    _logger.LogWarning($"Assunto com ID {id} não encontrado.");
                    return null;
                }
                return _mapper.Map<AssuntoDTO>(assunto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter assunto por ID {id}.", id);
                throw;
            }
        }

        public async Task<AssuntoDTO> AddAsync(CreateAssuntoDTO assuntoDTO)
        {
            try
            {
                var assunto = _mapper.Map<Assunto>(assuntoDTO);
                _dbContext.Assuntos.Add(assunto);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Assunto com ID {assunto.CodAs} adicionado com sucesso.");
                return _mapper.Map<AssuntoDTO>(assunto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar assunto.");
                throw;
            }
        }

        public async Task UpdateAsync(int id, CreateAssuntoDTO assuntoDTO)
        {
            try
            {
                var assunto = await _dbContext.Assuntos.FindAsync(id);
                if (assunto == null)
                {
                    _logger.LogWarning($"Assunto com ID {id} não encontrado ao atualizar.");
                    throw new KeyNotFoundException($"Assunto com ID {id} não encontrado.");
                }
                _mapper.Map(assuntoDTO, assunto);
                _dbContext.Entry(assunto).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Assunto com ID {id} atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar assunto com ID {id}.", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var assunto = await _dbContext.Assuntos.FindAsync(id);
                if (assunto == null)
                {
                    _logger.LogWarning($"Assunto com ID {id} não encontrado ao deletar.");
                    throw new KeyNotFoundException($"Assunto com ID {id} não encontrado.");
                }

                var hasLivros = await _dbContext.LivroAssuntos.AnyAsync(la => la.AssuntoCodAs == id);
                if (hasLivros)
                {
                    _logger.LogWarning($"Assunto com ID {id} não pode ser excluído porque está associado a um ou mais livros.");
                    throw new InvalidOperationException($"Assunto com ID {id} não pode ser excluído porque está associado a um ou mais livros.");
                }

                _dbContext.Assuntos.Remove(assunto);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Assunto com ID {id} deletado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar assunto com ID {id}.", id);
                throw;
            }
        }
    }
}
