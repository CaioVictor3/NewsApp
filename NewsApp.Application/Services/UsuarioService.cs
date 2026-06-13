using NewsApp.Application.Interface;
using NewsApp.Application.Models;
using NewsApp.Application.Models.Usuario;
using NewsApp.Application.Token;
using NewsApp.Domain;
using NewsApp.Domain.Handle;
using NewsApp.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NewsApp.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _configuration;
        private readonly Context _context;

        public UsuarioService(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Response<LoginResponseModel>> LoginUsuarioAsync(string login, string senha, int idPlataforma)
        {
            var retorno = new Response<LoginResponseModel>()
            {
                Data = new LoginResponseModel()
            };

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(x => x.Login == login && x.Senha == senha);

            if (usuario == null)
                throw new Exception("Usuário ou senha inválidos.");

            retorno.Data = new LoginResponseModel()
            {
                Nome = usuario.Nome,
                TipoUsuario = usuario.TipoUsuario ?? "Usuario",
                Token = TokenService.GenerateToken(
                    usuario.Login!, 
                    usuario.TipoUsuario ?? "Usuario", 
                    usuario.IdUsuario, 
                    usuario.Nome ?? ""),
                IdUsuario = usuario.IdUsuario
            };

            retorno.Success = true;
            retorno.Message = "Sucesso login";

            return retorno;
        }

        public async Task<Response<UsuarioResponseModel>?> CadastrarUsuarioMobileAsync(CriarUsuarioMobileRequestModel request)
        {
            var retorno = new Response<UsuarioResponseModel>()
            {
                Data = new UsuarioResponseModel()
            };

            var usuarioExistente = await _context.Usuario.FirstOrDefaultAsync(x => x.Login == request.Login);
            if (usuarioExistente != null)
                throw new Exception("Usuário já cadastrado.");

            var usuario = new Usuario(
                request.Login,
                request.Nome,
                request.Email,
                request.Senha,
                request.Cpf,
                request.DataNascimento,
                request.Endereco
            );

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            retorno.Data.IdUsuario = usuario.IdUsuario;
            retorno.Data.Nome = usuario.Nome;
            retorno.Data.Login = usuario.Login;
            retorno.Data.Email = usuario.Email;
            retorno.Data.Cpf = usuario.CPF;
            retorno.Data.DataNascimento = usuario.DataNascimento;
            retorno.Data.DataInclusao = usuario.DataInclusao;
            retorno.Data.Endereco = usuario.Endereco;
            retorno.Data.TipoUsuario = usuario.TipoUsuario;
            retorno.Success = true;
            retorno.Message = "Usuário cadastrado com sucesso.";
            return retorno;
        }

        public async Task<Response<UsuarioResponseModel>?> ObterPorIdAsync(int id)
        {
            var retorno = new Response<UsuarioResponseModel>()
            {
                Data = new UsuarioResponseModel()
            };

            if (id <= 0)
                throw new ServiceException("Usuário inválido.");

            var usuario = await _context.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (usuario == null)
                throw new ServiceException("Usuário não encontrado.");

            retorno.Data.IdUsuario = usuario.IdUsuario;
            retorno.Data.Nome = usuario.Nome;
            retorno.Data.Login = usuario.Login;
            retorno.Data.Email = usuario.Email;
            retorno.Data.Cpf = usuario.CPF;
            retorno.Data.DataNascimento = usuario.DataNascimento;
            retorno.Data.DataInclusao = usuario.DataInclusao;
            retorno.Data.Endereco = usuario.Endereco;
            retorno.Data.TipoUsuario = usuario.TipoUsuario;
            retorno.Success = true;
            retorno.Message = "Usuário obtido com sucesso.";

            return retorno;
        }
    }

    public class CriarUsuarioMobileRequestModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
    }
}
