using ERP_Application.Models;
using ERP_Application.Models.Noticia;

namespace ERP_Application.Interface
{
    public interface INoticiaService
    {
        Task<Response<SincronizarNoticiasResponseModel>?> BuscarNoticiasDaNewsApiESalvarNoBancoAsync(int page = 1, int pageSize = 20);
        Task<Response<ListarNoticiaResponseModel>?> ListarNoticiasSalvasNoBancoAsync(int page = 1, int pageSize = 20);
        Task<Response<NoticiaDetalheResponseModel>?> BuscarNoticiaSalvaPorIdAsync(int idNoticia);
    }
}
