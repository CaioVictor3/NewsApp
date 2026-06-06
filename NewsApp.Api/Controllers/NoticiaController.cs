using NewsApp.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoticiaController : ControllerBase
    {
        private readonly INoticiaService _noticiaService;

        public NoticiaController(INoticiaService noticiaService)
        {
            _noticiaService = noticiaService;
        }

        [HttpPost("sincronizar-news-api")]
        public async Task<IActionResult> BuscarNoticiasDaNewsApiESalvarNoBancoAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(await _noticiaService.BuscarNoticiasDaNewsApiESalvarNoBancoAsync(page, pageSize));
        }

        [HttpGet("listar-noticias-salvas")]
        public async Task<IActionResult> ListarNoticiasSalvasNoBancoAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(await _noticiaService.ListarNoticiasSalvasNoBancoAsync(page, pageSize));
        }

        [HttpGet("obter-noticia-por-id")]
        public async Task<IActionResult> BuscarNoticiaSalvaPorIdAsync([FromQuery] int idNoticia)
        {
            return Ok(await _noticiaService.BuscarNoticiaSalvaPorIdAsync(idNoticia));
        }
    }
}
