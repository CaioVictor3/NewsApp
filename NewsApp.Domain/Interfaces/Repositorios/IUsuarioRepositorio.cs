using Proclin.Models;

namespace NewsApp.Domain.Interfaces.Repositorios
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario?> ObterPorIdAsync(int idUsuario);
        Task<Usuario> ObterPorLoginAsync(string login);
        Task<IEnumerable<Usuario?>> ListarUsuariosAsync();
        void Atualizar(Usuario usuario);


        #region Segurança
        Task<Usuario?> LoginUsuarioAsync(string login, string senha);
        #endregion

    }
}
