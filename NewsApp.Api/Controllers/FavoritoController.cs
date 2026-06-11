using NewsApp.Application.Interface;
using NewsApp.Application.Models.Favorito;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritoController : ControllerBase
    {
        private readonly IFavoritoService _favoritoService;

        public FavoritoController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        [HttpPost("adicionar-favorito")]
        public async Task<IActionResult> AdicionarFavoritoAsync([FromBody] CriarFavoritoRequestModel request)
        {
            return Ok(await _favoritoService.AdicionarFavoritoAsync(request));
        }

        [HttpDelete("remover-favorito")]
        public async Task<IActionResult> RemoverFavoritoAsync([FromQuery] int idFavorito)
        {
            return Ok(await _favoritoService.RemoverFavoritoAsync(idFavorito));
        }

        [HttpGet("listar-favoritos-por-usuario")]
        public async Task<IActionResult> ListarFavoritosPorUsuarioAsync([FromQuery] int idUsuario)
        {
            return Ok(await _favoritoService.ListarFavoritosPorUsuarioAsync(idUsuario));
        }
    }
}
