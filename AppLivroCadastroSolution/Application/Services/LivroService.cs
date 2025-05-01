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
    public class LivroService : ILivroService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LivroService> _logger;

        public LivroService(AppDbContext dbContext, IMapper mapper, ILogger<LivroService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LivroDTO>> GetAllAsync()
        {
            try
            {
                var livros = await _dbContext.Livros
                    .Include(l => l.LivroAutores).ThenInclude(la => la.Autor)
                    .Include(l => l.LivroAssuntos).ThenInclude(la => la.Assunto)
                    .Include(l => l.PrecosLivroFormaCompra).ThenInclude(plfc => plfc.FormaCompra)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<LivroDTO>>(livros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os livros.");
                throw;
            }
        }

        public async Task<LivroDTO> GetByIdAsync(int id)
        {
            try
            {
                var livro = await _dbContext.Livros
                    .Where(l => l.Codl == id)
                    .Include(l => l.LivroAutores).ThenInclude(la => la.Autor)
                    .Include(l => l.LivroAssuntos).ThenInclude(la => la.Assunto)
                    .Include(l => l.PrecosLivroFormaCompra).ThenInclude(plfc => plfc.FormaCompra)
                    .FirstOrDefaultAsync();

                if (livro == null)
                {
                    _logger.LogWarning($"Livro com ID {id} não encontrado.");
                    return null;
                }

                var livroDTO = _mapper.Map<LivroDTO>(livro);

                livroDTO.Autores = livro.LivroAutores.Select(la => new AutorDTO
                {
                    CodAu = la.Autor.CodAu,
                    Nome = la.Autor.Nome
                }).ToList();

                livroDTO.Assuntos = livro.LivroAssuntos.Select(la => new AssuntoDTO
                {
                    CodAs = la.Assunto.CodAs,
                    Descricao = la.Assunto.Descricao
                }).ToList();

                livroDTO.PrecosFormaCompra = livro.PrecosLivroFormaCompra
                    .Select(p => new PrecoFormaCompraExibicaoDTO
                    {
                        FormaCompraNome = p.FormaCompra.Nome,
                        Preco = p.Preco
                    }).ToList();

                return livroDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter livro por ID {id}.", id);
                throw;
            }
        }

        public async Task<LivroDTO> AddAsync(CreateLivroDTO createLivroDTO)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var livro = _mapper.Map<Livro>(createLivroDTO);
                    livro.LivroAutores = new List<LivroAutor>();
                    livro.LivroAssuntos = new List<LivroAssunto>();
                    livro.PrecosLivroFormaCompra = new List<PrecoLivroFormaCompra>();

                    foreach (var autorId in createLivroDTO.AutorIds)
                    {
                        var autor = await _dbContext.Autores.FindAsync(autorId);

                        if (autor == null)
                        {
                            _logger.LogWarning($"Autor com ID {autorId} não encontrado ao adicionar livro.");
                            throw new KeyNotFoundException($"Autor com ID {autorId} não encontrado.");
                        }
                        livro.LivroAutores.Add(new LivroAutor { Autor = autor, AutorCodAu = autor.CodAu });

                    }

                    foreach (var assuntoId in createLivroDTO.AssuntoIds)
                    {
                        var assunto = await _dbContext.Assuntos.FindAsync(assuntoId);
                        if (assunto == null)
                        {
                            _logger.LogWarning($"Assunto com ID {assuntoId} não encontrado ao adicionar livro.");
                            throw new KeyNotFoundException($"Assunto com ID {assuntoId} não encontrado.");
                        }
                        livro.LivroAssuntos.Add(new LivroAssunto { Assunto = assunto, AssuntoCodAs = assunto.CodAs });

                    }

                    foreach (var precoFormaCompraDTO in createLivroDTO.PrecosFormaCompra)
                    {
                        var formaCompra = await _dbContext.FormasCompra.FindAsync(precoFormaCompraDTO.FormaCompraId);
                        if (formaCompra == null)
                        {
                            _logger.LogWarning($"Forma de Compra com ID {precoFormaCompraDTO.FormaCompraId} não encontrada ao adicionar livro.");
                            throw new KeyNotFoundException($"Forma de Compra com ID {precoFormaCompraDTO.FormaCompraId} não encontrada.");
                        }
                        livro.PrecosLivroFormaCompra.Add(new PrecoLivroFormaCompra
                        {
                            FormaCompraID = precoFormaCompraDTO.FormaCompraId,
                            FormaCompra = formaCompra,
                            Preco = precoFormaCompraDTO.Preco
                        });
                    }

                    _dbContext.Livros.Add(livro);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _logger.LogInformation($"Livro com ID {livro.Codl} adicionado com sucesso.");
                    return await GetByIdAsync(livro.Codl);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Erro ao adicionar livro.");
                    throw;
                }
            }
        }

        public async Task UpdateAsync(int id, CreateLivroDTO createLivroDTO)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var livro = await _dbContext.Livros
                        .Include(l => l.LivroAutores)
                        .Include(l => l.LivroAssuntos)
                        .Include(l => l.PrecosLivroFormaCompra)
                        .FirstOrDefaultAsync(l => l.Codl == id);

                    if (livro == null)
                    {
                        _logger.LogWarning($"Livro com ID {id} não encontrado ao atualizar.");
                        throw new KeyNotFoundException($"Livro com ID {id} não encontrado.");
                    }

                    _mapper.Map(createLivroDTO, livro);

                    var autoresParaAdicionar = createLivroDTO.AutorIds.Except(livro.LivroAutores.Select(la => la.AutorCodAu)).ToList();
                    var autoresParaRemover = livro.LivroAutores.Select(la => la.AutorCodAu).Except(createLivroDTO.AutorIds).ToList();

                    foreach (var autorId in autoresParaAdicionar)
                    {
                        var autor = await _dbContext.Autores.FindAsync(autorId);
                        if (autor != null)
                        {
                            livro.LivroAutores.Add(new LivroAutor { AutorCodAu = autorId, LivroCodl = id });
                        }
                        else
                        {
                            _logger.LogWarning($"Autor com ID {autorId} não encontrado ao atualizar livro.");
                            throw new KeyNotFoundException($"Autor com ID {autorId} não encontrado.");
                        }
                    }
                    foreach (var autorId in autoresParaRemover)
                    {
                        var livroAutorToRemove = livro.LivroAutores.FirstOrDefault(la => la.AutorCodAu == autorId);
                        if (livroAutorToRemove != null)
                        {
                            _dbContext.LivroAutores.Remove(livroAutorToRemove);
                        }
                    }

                    var assuntosParaAdicionar = createLivroDTO.AssuntoIds.Except(livro.LivroAssuntos.Select(la => la.AssuntoCodAs)).ToList();
                    var assuntosParaRemover = livro.LivroAssuntos.Select(la => la.AssuntoCodAs).Except(createLivroDTO.AssuntoIds).ToList();

                    foreach (var assuntoId in assuntosParaAdicionar)
                    {
                        var assunto = await _dbContext.Assuntos.FindAsync(assuntoId);
                        if (assunto != null)
                        {
                            livro.LivroAssuntos.Add(new LivroAssunto { AssuntoCodAs = assuntoId, LivroCodl = id });
                        }
                        else
                        {
                            _logger.LogWarning($"Assunto com ID {assuntoId} não encontrado ao atualizar livro.");
                            throw new KeyNotFoundException($"Assunto com ID {assuntoId} não encontrado.");
                        }
                    }
                    foreach (var assuntoId in assuntosParaRemover)
                    {
                        var livroAssuntoToRemove = livro.LivroAssuntos.FirstOrDefault(la => la.AssuntoCodAs == assuntoId);
                        if (livroAssuntoToRemove != null)
                        {
                            _dbContext.LivroAssuntos.Remove(livroAssuntoToRemove);
                        }
                    }

                    var precosParaAtualizar = createLivroDTO.PrecosFormaCompra;

                    var formasCompraIdsParaRemover = livro.PrecosLivroFormaCompra
                        .Select(p => p.FormaCompraID)
                        .Except(precosParaAtualizar.Select(dto => dto.FormaCompraId))
                        .ToList();

                    foreach (var formaCompraId in formasCompraIdsParaRemover)
                    {
                        var precoLivroFormaCompraToRemove = livro.PrecosLivroFormaCompra
                            .FirstOrDefault(p => p.FormaCompraID == formaCompraId);
                        if (precoLivroFormaCompraToRemove != null)
                        {
                            _dbContext.Set<PrecoLivroFormaCompra>().Remove(precoLivroFormaCompraToRemove);
                        }
                    }

                    foreach (var precoDTO in precosParaAtualizar)
                    {
                        var precoExistente = livro.PrecosLivroFormaCompra
                            .FirstOrDefault(p => p.FormaCompraID == precoDTO.FormaCompraId);

                        if (precoExistente != null)
                        {
                            precoExistente.Preco = precoDTO.Preco;
                        }
                        else
                        {
                            var formaCompra = await _dbContext.FormasCompra.FindAsync(precoDTO.FormaCompraId);
                            if (formaCompra != null)
                            {
                                livro.PrecosLivroFormaCompra.Add(new PrecoLivroFormaCompra
                                {
                                    FormaCompraID = precoDTO.FormaCompraId,
                                    Preco = precoDTO.Preco,
                                    LivroCodl = id
                                });
                            }
                            else
                            {
                                _logger.LogWarning($"Forma de Compra com ID {precoDTO.FormaCompraId} não encontrada ao atualizar livro.");
                                throw new KeyNotFoundException($"Forma de Compra com ID {precoDTO.FormaCompraId} não encontrada.");
                            }
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _logger.LogInformation($"Livro com ID {id} atualizado com sucesso.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Erro ao atualizar livro com ID {id}.", id);
                    throw;
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var livro = await _dbContext.Livros.FindAsync(id);
                    if (livro == null)
                    {
                        _logger.LogWarning($"Livro com ID {id} não encontrado ao deletar.");
                        throw new KeyNotFoundException($"Livro com ID {id} não encontrado.");
                    }

                    _dbContext.Livros.Remove(livro);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _logger.LogInformation($"Livro com ID {id} deletado com sucesso.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Erro ao deletar livro com ID {id}.", id);
                    throw;
                }
            }
        }
    }
}

