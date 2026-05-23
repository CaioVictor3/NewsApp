using ERP_Application.Models;
using ERP_Application.Models.Usuario;
using ERP_Application.Services;
using Proclin.Models;

namespace ERP_Application.Interface
{
    public interface IUsuarioService
    {
        Task<Response<LoginResponseModel>> LoginUsuarioAsync(string login, string senha, int idPlataforma);
        Task<Response<UsuarioResponseModel>?> CadastrarUsuarioMobileAsync(CriarUsuarioMobileRequestModel request);
        
        Task<Response<AlterarSenhaResponseModel>?> AlterarSenhaAsync(int idUsuario, string senha, string confirmarSenha, string usuarioAlteracao);
        Task<Response<EsqueceuSenhaResponseModel>?> EsqueceuSenhaAsync(EsqueceuSenhaRequestModel model);
        Task<Response<UsuarioListResponseModel>?> ListarUsuariosAsync();
        Task<Response<UsuarioResponseModel>?> ObterPorIdAsync(int id);
        Task<Response<UsuarioResponseModel>?> AtivarUsuarioAsync(int idUsuario, string situacao, string usuarioAlteracao);
        Task<Response<UsuarioResponseModel>?> InativarUsuarioAsync(int idUsuario, string situacao, string usuarioAlteracao);
        Task<Response<UsuarioResponseModel>?> CriarUsuarioAsync(CriarUsuarioRequestModel request);
        Task<Response<UsuarioResponseModel>?> ExcluirUsuarioAsync(int id);
        Task<Response<UsuarioResponseModel>?> AtualizarUsuarioAsync(AtualizarUsuarioRequestModel request);
        Task<Response<UsuarioResponseModel>?> VerificarHashAlterarSenhaAsync(string hash);
        Task<Response<UsuarioResponseModel>?> AlterarSenhaLinkAsync(string senha, string confirmarSenha, string hash);
        Task<Response<UsuarioResponseModel>?> CadastrarUsuarioPacienteAsync(CriarUsuarioPacienteRequestModel request);
    }
}
