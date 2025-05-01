using AppLivroCadastro.Reports.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppLivroCadastro.Api.Controllers
{
    [ApiController]
    [Route("api/relatorios")]
    public class RelatorioController : ControllerBase
    {
        private readonly LivroAutorAssuntoReportService _reportService;

        public RelatorioController(LivroAutorAssuntoReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("livro-autor-assunto")]
        public async Task<IActionResult> GetLivroAutorAssuntoRelatorio()
        {
            var dadosRelatorio = await _reportService.GerarRelatorioAsync();
            return Ok(dadosRelatorio);
        }
    }
}