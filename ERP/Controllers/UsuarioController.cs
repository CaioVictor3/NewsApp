using ERP_Application.Interface;
using ERP_Application.Models;
using ERP_Application.Models.Usuario;
using ERP_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private IConfiguration _config;

        public UsuarioController(IConfiguration config, IUsuarioService usuarioService)
        {
            _config = config;
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel parametros)
        {
            var usuario = await _usuarioService.LoginUsuarioAsync(parametros.Login, parametros.Senha, 0);
            return Ok(usuario);
        }

        [HttpPost("cadastrar-mobile")]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarMobileAsync([FromBody] CriarUsuarioMobileRequestModel request)
        {
            var result = await _usuarioService.CadastrarUsuarioMobileAsync(request);
            return Ok(result);
        }

        [HttpGet("obter-por-id")]
        public async Task<IActionResult> ObterPorIdAsync([FromQuery] int id)
        {
            return Ok(await _usuarioService.ObterPorIdAsync(id));
        }
    }

    public class LoginRequestModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
