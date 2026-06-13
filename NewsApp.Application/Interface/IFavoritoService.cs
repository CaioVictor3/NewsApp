using NewsApp.Application.Models;
using NewsApp.Application.Models.Favorito;

namespace NewsApp.Application.Interface
{
    public interface IFavoritoService
    {
        Task<Response<FavoritoResponseModel>?> AdicionarFavoritoAsync(CriarFavoritoRequestModel request);
        Task<Response<FavoritoResponseModel>?> RemoverFavoritoAsync(int idFavorito);
        Task<Response<ListarFavoritoResponseModel>?> ListarFavoritosPorUsuarioAsync(int idUsuario);
    }
}
