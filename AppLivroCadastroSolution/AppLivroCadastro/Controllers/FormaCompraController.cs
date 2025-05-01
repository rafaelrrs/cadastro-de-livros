using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.DTOs.CreateDTOs;
using AppLivroCadastro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppLivroCadastro.Api.Controllers
{
    [ApiController]
    [Route("api/formas-compra")]
    public class FormaCompraController : ControllerBase
    {
        private readonly IFormaCompraService _formaCompraService;
        private readonly ILogger<FormaCompraController> _logger;

        public FormaCompraController(IFormaCompraService formaCompraService, ILogger<FormaCompraController> logger)
        {
            _formaCompraService = formaCompraService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FormaCompraDTO>>> GetAll()
        {
            try
            {
                var formasCompra = await _formaCompraService.GetAllAsync();
                return Ok(formasCompra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as formas de compra.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormaCompraDTO>> GetById(int id)
        {
            try
            {
                var formaCompra = await _formaCompraService.GetByIdAsync(id);
                if (formaCompra == null)
                {
                    _logger.LogWarning($"Forma de compra com ID {id} não encontrada.");
                    return NotFound();
                }
                return Ok(formaCompra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter forma de compra por ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormaCompraDTO>> Add([FromBody] CreateFormaCompraDTO formaCompraDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para adicionar forma de compra.");
                return BadRequest(ModelState);
            }

            try
            {
                var novaFormaCompra = await _formaCompraService.AddAsync(formaCompraDTO);
                return CreatedAtAction(nameof(GetById), new { id = novaFormaCompra.FormaCompraID }, novaFormaCompra);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar forma de compra.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateFormaCompraDTO formaCompraDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para atualizar forma de compra.");
                return BadRequest(ModelState);
            }

            try
            {
                await _formaCompraService.UpdateAsync(id, formaCompraDTO);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar forma de compra com ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _formaCompraService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar forma de compra com ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}