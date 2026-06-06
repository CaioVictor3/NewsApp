using NewsApp.Application.Models;
using NewsApp.Application.Models.Noticia;

namespace NewsApp.Application.Interface
{
    public interface INoticiaService
    {
        Task<Response<SincronizarNoticiasResponseModel>?> BuscarNoticiasDaNewsApiESalvarNoBancoAsync(int page = 1, int pageSize = 20);
        Task<Response<ListarNoticiaResponseModel>?> ListarNoticiasSalvasNoBancoAsync(int page = 1, int pageSize = 20);
        Task<Response<NoticiaDetalheResponseModel>?> BuscarNoticiaSalvaPorIdAsync(int idNoticia);
    }
}
