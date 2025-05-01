using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;
using AppLivroCadastro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppLivroCadastro.Api.Controllers
{
    [ApiController]
    [Route("api/livros")]
    public class LivroController : ControllerBase
    {
        private readonly ILivroService _livroService;
        private readonly ILogger<LivroController> _logger;

        public LivroController(ILivroService livroService, ILogger<LivroController> logger)
        {
            _livroService = livroService;
            _logger = logger;
        }

        /// <summary>
        /// Todos os livros.
        /// </summary>
        /// <returns>Uma lista de livros.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetAll()
        {
            try
            {
                var livros = await _livroService.GetAllAsync();
                return Ok(livros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os livros.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        /// <summary>
        /// Consulta um livro pelo seu ID.
        /// </summary>
        /// <param name="id"> ID do livro.</param>
        /// <returns>O livro por ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LivroDTO>> GetById(int id)
        {
            try
            {
                var livro = await _livroService.GetByIdAsync(id);
                if (livro == null)
                {
                    _logger.LogWarning($"Livro com ID {id} não encontrado.");
                    return NotFound();
                }
                return Ok(livro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter livro por ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        /// <summary>
        /// Adiciona um novo livro.
        /// </summary>
        /// <param name="createLivroDTO">Os dados do livro a serem adicionados.</param>
        /// <returns>O livro criado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LivroDTO>> Add([FromBody] CreateLivroDTO createLivroDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para adicionar livro.");
                return BadRequest(ModelState);
            }

            try
            {
                var livroAdicionado = await _livroService.AddAsync(createLivroDTO);
                return CreatedAtAction(nameof(GetById), new { id = livroAdicionado.Codl }, livroAdicionado);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar livro.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        /// <summary>
        /// Atualiza um livro existente.
        /// </summary>
        /// <param name="id">O ID do livro a ser atualizado.</param>
        /// <param name="createLivroDTO">Os novos dados do livro.</param>
        /// <returns>No Content se a atualização for bem-sucedida.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateLivroDTO createLivroDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para atualizar livro.");
                return BadRequest(ModelState);
            }

            try
            {
                await _livroService.UpdateAsync(id, createLivroDTO);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar livro com ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        /// <summary>
        /// Deleta um livro.
        /// </summary>
        /// <param name="id">O ID do livro a ser deletado.</param>
        /// <returns>exclusão bem-sucedida.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _livroService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar livro com ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}

