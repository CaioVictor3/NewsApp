using ERP_Application.Interface;
using ERP_Application.Models;
using ERP_Application.Models.Usuario;
using ERP_Application.Token;
using ERP_Domain.Handle;
using ERP_Infra.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Proclin.Models;

namespace ERP_Application.Services
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
                throw new ServiceException("Usuário ou senha inválidos.");

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

            var checkUsuario = await _context.Usuario.AnyAsync(x => x.Login == request.Login);
            if (checkUsuario)
                throw new ServiceException("Usuário já cadastrado.");

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
            retorno.Success = true;
            retorno.Message = "Usuário cadastrado com sucesso.";

            return retorno;
        }

        public Task<Response<AlterarSenhaResponseModel>?> AlterarSenhaAsync(int idUsuario, string senha, string confirmarSenha, string usuarioAlteracao) => throw new NotImplementedException();
        public Task<Response<EsqueceuSenhaResponseModel>?> EsqueceuSenhaAsync(EsqueceuSenhaRequestModel model) => throw new NotImplementedException();
        public Task<Response<UsuarioListResponseModel>?> ListarUsuariosAsync() => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> ObterPorIdAsync(int id) => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> AtivarUsuarioAsync(int idUsuario, string situacao, string usuarioAlteracao) => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> InativarUsuarioAsync(int idUsuario, string situacao, string usuarioAlteracao) => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> CriarUsuarioAsync(CriarUsuarioRequestModel request) => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> ExcluirUsuarioAsync(int id) => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> AtualizarUsuarioAsync(AtualizarUsuarioRequestModel request) => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> VerificarHashAlterarSenhaAsync(string hash) => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> AlterarSenhaLinkAsync(string senha, string confirmarSenha, string hash) => throw new NotImplementedException();
        public Task<Response<UsuarioResponseModel>?> CadastrarUsuarioPacienteAsync(CriarUsuarioPacienteRequestModel request) => throw new NotImplementedException();
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
