using NewsApp.Application.Models;
using NewsApp.Application.Models.Usuario;
using NewsApp.Application.Services;

namespace NewsApp.Application.Interface
{
    public interface IUsuarioService
    {
        Task<Response<LoginResponseModel>> LoginUsuarioAsync(string login, string senha, int idPlataforma);
        Task<Response<UsuarioResponseModel>?> CadastrarUsuarioMobileAsync(CriarUsuarioMobileRequestModel request);
        Task<Response<UsuarioResponseModel>?> ObterPorIdAsync(int id);
    }
}
