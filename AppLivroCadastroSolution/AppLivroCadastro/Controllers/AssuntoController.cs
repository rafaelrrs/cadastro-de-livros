using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;
using AppLivroCadastro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppLivroCadastro.Api.Controllers
{
    [ApiController]
    [Route("api/assuntos")]
    public class AssuntoController : ControllerBase
    {
        private readonly IAssuntoService _assuntoService;
        private readonly ILogger<AssuntoController> _logger;

        public AssuntoController(IAssuntoService assuntoService, ILogger<AssuntoController> logger)
        {
            _assuntoService = assuntoService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AssuntoDTO>>> GetAll()
        {
            try
            {
                var assuntos = await _assuntoService.GetAllAsync();
                return Ok(assuntos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os assuntos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AssuntoDTO>> GetById(int id)
        {
            try
            {
                var assunto = await _assuntoService.GetByIdAsync(id);
                if (assunto == null)
                {
                    _logger.LogWarning($"Assunto com ID {id} não encontrado.");
                    return NotFound();
                }
                return Ok(assunto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter assunto por ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AssuntoDTO>> Add([FromBody] CreateAssuntoDTO assuntoDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para adicionar assunto.");
                return BadRequest(ModelState);
            }

            try
            {
                var novoAssunto = await _assuntoService.AddAsync(assuntoDTO);
                return CreatedAtAction(nameof(GetById), new { id = novoAssunto.CodAs }, novoAssunto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar assunto.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAssuntoDTO assuntoDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para atualizar assunto.");
                return BadRequest(ModelState);
            }

            try
            {
                await _assuntoService.UpdateAsync(id, assuntoDTO);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar assunto com ID {id}.", id);
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
                await _assuntoService.DeleteAsync(id);
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
                _logger.LogError(ex, "Erro ao deletar assunto com ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}
