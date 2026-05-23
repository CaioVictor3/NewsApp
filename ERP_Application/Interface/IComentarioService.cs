

using ERP_Application.Models;
using ERP_Domain;

namespace ERP_Application.Interface
{
    public interface IComentarioService
    {
        Task<Response<ComentarioResponseModel>?>CriarComentarioAsync(CriarComentarioRequestModel request);
        Task<Response<ComentarioResponseModel>?>ExcluirComentarioAsync(int idComentario);
        Task<Response<ListarComentarioResponseModel>?>ListarComentarioPorNoticiAsync(int idNoticia);
        Task<Response<ComentarioResponseModel>?>AtualizarComentarioAsync(AtualizarComentarioRequestModel request);
    }
}