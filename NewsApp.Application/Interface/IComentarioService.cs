

using NewsApp.Application.Models;
using NewsApp.Application.Models.Comentario;

namespace NewsApp.Application.Interface
{
    public interface IComentarioService
    {
        Task<Response<ComentarioResponseModel>?> CriarComentarioAsync(CriarComentarioRequestModel request);
        Task<Response<ComentarioResponseModel>?> ExcluirComentarioAsync(int idComentario);
        Task<Response<ListarComentarioResponseModel>?> ListarComentarioPorNoticiaAsync(int idNoticia);
        Task<Response<ComentarioResponseModel>?> AtualizarComentarioAsync(AtualizarComentarioRequestModel request);
    }
}
