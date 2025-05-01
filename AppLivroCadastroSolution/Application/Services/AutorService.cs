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
    public class AutorService : IAutorService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AutorService> _logger;

        public AutorService(AppDbContext dbContext, IMapper mapper, ILogger<AutorService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AutorDTO>> GetAllAsync()
        {
            try
            {
                var autores = await _dbContext.Autores.ToListAsync();
                return _mapper.Map<IEnumerable<AutorDTO>>(autores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os autores.");
                throw;
            }
        }

        public async Task<AutorDTO> GetByIdAsync(int id)
        {
            try
            {
                var autor = await _dbContext.Autores.FindAsync(id);
                if (autor == null)
                {
                    _logger.LogWarning($"Autor com ID {id} não encontrado.");
                    return null;
                }
                return _mapper.Map<AutorDTO>(autor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter autor por ID {id}.", id);
                throw;
            }
        }

        public async Task<AutorDTO> AddAsync(CreateAutorDTO autorDTO)
        {
            try
            {
                var autor = _mapper.Map<Autor>(autorDTO);
                _dbContext.Autores.Add(autor);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Autor com ID {autor.CodAu} adicionado com sucesso.");
                return _mapper.Map<AutorDTO>(autor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar autor.");
                throw;
            }
        }

        public async Task UpdateAsync(int id, CreateAutorDTO autorDTO)
        {
            try
            {
                var autor = await _dbContext.Autores.FindAsync(id);
                if (autor == null)
                {
                    _logger.LogWarning($"Autor com ID {id} não encontrado ao atualizar.");
                    throw new KeyNotFoundException($"Autor com ID {id} não encontrado.");
                }
                _mapper.Map(autorDTO, autor);
                _dbContext.Entry(autor).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Autor com ID {id} atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar autor com ID {id}.", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var autor = await _dbContext.Autores.FindAsync(id);
                if (autor == null)
                {
                    _logger.LogWarning($"Autor com ID {id} não encontrado ao deletar.");
                    throw new KeyNotFoundException($"Autor com ID {id} não encontrado.");
                }

                var hasLivros = await _dbContext.LivroAutores.AnyAsync(la => la.AutorCodAu == id);
                if (hasLivros)
                {
                    _logger.LogWarning($"Autor com ID {id} não pode ser excluído porque está associado a um ou mais livros.");
                    throw new InvalidOperationException($"Autor com ID {id} não pode ser excluído porque está associado a um ou mais livros.");
                }

                _dbContext.Autores.Remove(autor);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Autor com ID {id} deletado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar autor com ID {id}.", id);
                throw;
            }
        }
    }
}
