using ERP_Application.Interface;
using ERP_Application.Models.Comentario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;

        public ComentarioController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        [HttpPost("criar-comentario")]
        public async Task<IActionResult> CriarComentarioAsync([FromBody] CriarComentarioRequestModel request)
        {
            return Ok(await _comentarioService.CriarComentarioAsync(request));
        }

        [HttpGet("listar-por-noticia")]
        public async Task<IActionResult> ListarPorNoticiaAsync([FromQuery] int idNoticia)
        {
            return Ok(await _comentarioService.ListarComentarioPorNoticiaAsync(idNoticia));
        }

        [HttpPut("atualizar-comentario")]
        public async Task<IActionResult> AtualizarComentarioAsync([FromBody] AtualizarComentarioRequestModel request)
        {
            return Ok(await _comentarioService.AtualizarComentarioAsync(request));
        }

        [HttpDelete("excluir-comentario")]
        public async Task<IActionResult> ExcluirComentarioAsync([FromQuery] int idComentario)
        {
            return Ok(await _comentarioService.ExcluirComentarioAsync(idComentario));
        }
    }
}
