

using ERP_Application.Models;
using ERP_Application.Models.Comentario;

namespace ERP_Application.Interface
{
    public interface IComentarioService
    {
        Task<Response<ComentarioResponseModel>?> CriarComentarioAsync(CriarComentarioRequestModel request);
        Task<Response<ComentarioResponseModel>?> ExcluirComentarioAsync(int idComentario);
        Task<Response<ListarComentarioResponseModel>?> ListarComentarioPorNoticiaAsync(int idNoticia);
        Task<Response<ComentarioResponseModel>?> AtualizarComentarioAsync(AtualizarComentarioRequestModel request);
    }
}
