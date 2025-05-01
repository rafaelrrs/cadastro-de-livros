using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;
using AppLivroCadastro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppLivroCadastro.Api.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutorController : ControllerBase
    {
        private readonly IAutorService _autorService;
        private readonly ILogger<AutorController> _logger;

        public AutorController(IAutorService autorService, ILogger<AutorController> logger)
        {
            _autorService = autorService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> GetAll()
        {
            try
            {
                var autores = await _autorService.GetAllAsync();
                return Ok(autores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os autores.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AutorDTO>> GetById(int id)
        {
            try
            {
                var autor = await _autorService.GetByIdAsync(id);
                if (autor == null)
                {
                    _logger.LogWarning($"Autor com ID {id} não encontrado.");
                    return NotFound();
                }
                return Ok(autor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter autor por ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AutorDTO>> Add([FromBody] CreateAutorDTO autorDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para adicionar autor.");
                return BadRequest(ModelState);
            }

            try
            {
                var novoAutor = await _autorService.AddAsync(autorDTO);
                return CreatedAtAction(nameof(GetById), new { id = novoAutor.CodAu }, novoAutor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar autor.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAutorDTO autorDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para atualizar autor.");
                return BadRequest(ModelState);
            }

            try
            {
                await _autorService.UpdateAsync(id, autorDTO);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar autor com ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _autorService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar autor com ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}
