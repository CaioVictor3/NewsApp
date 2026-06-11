using NewsApp.Application.Interface;
using NewsApp.Application.Models;
using NewsApp.Application.Models.Favorito;
using NewsApp.Domain;
using NewsApp.Domain.Handle;
using NewsApp.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Application.Services
{
    public class FavoritoService : IFavoritoService
    {
        private readonly Context _context;

        public FavoritoService(Context context)
        {
            _context = context;
        }

        public async Task<Response<FavoritoResponseModel>?> AdicionarFavoritoAsync(CriarFavoritoRequestModel request)
        {
            var retorno = new Response<FavoritoResponseModel>()
            {
                Data = new FavoritoResponseModel()
            };

            if (request.IdUsuario <= 0)
                throw new Exception("Usuário inválido.");

            if (request.IdNoticia <= 0)
                throw new Exception("Notícia inválida.");

            var usuarioExiste = await _context.Usuario.AnyAsync(x => x.IdUsuario == request.IdUsuario);
            if (!usuarioExiste)
                throw new Exception("Usuário não encontrado.");

            var noticia = await _context.Noticia
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdNoticia == request.IdNoticia && x.Situacao != "Excluido");

            if (noticia == null)
                throw new Exception("Notícia não encontrada.");

            var jaFavoritado = await _context.Favorito
                .AnyAsync(x => x.IdUsuario == request.IdUsuario && x.IdNoticia == request.IdNoticia && x.Situacao != "Excluido");

            if (jaFavoritado)
                throw new Exception("Notícia já favoritada.");

            var favorito = new Favorito(request.IdUsuario, request.IdNoticia);

            _context.Favorito.Add(favorito);
            await _context.SaveChangesAsync();

            retorno.Data = MapearFavorito(favorito, noticia);
            retorno.Success = true;
            retorno.Message = "Notícia favoritada com sucesso.";
            return retorno;
        }

        public async Task<Response<FavoritoResponseModel>?> RemoverFavoritoAsync(int idFavorito)
        {
            var retorno = new Response<FavoritoResponseModel>()
            {
                Data = new FavoritoResponseModel()
            };

            var favorito = await _context.Favorito
                .FirstOrDefaultAsync(x => x.IdFavorito == idFavorito && x.Situacao != "Excluido");

            if (favorito == null)
                throw new Exception("Favorito não encontrado.");

            var noticia = await _context.Noticia
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdNoticia == favorito.IdNoticia);

            favorito.SetUsuarioExclusao("Sistema");
            await _context.SaveChangesAsync();

            if (noticia != null)
                retorno.Data = MapearFavorito(favorito, noticia);

            retorno.Success = true;
            retorno.Message = "Favorito removido com sucesso.";
            return retorno;
        }

        public async Task<Response<ListarFavoritoResponseModel>?> ListarFavoritosPorUsuarioAsync(int idUsuario)
        {
            var retorno = new Response<ListarFavoritoResponseModel>()
            {
                Data = new ListarFavoritoResponseModel()
            };

            if (idUsuario <= 0)
                throw new Exception("Usuário inválido.");

            var favoritos = await _context.Favorito
                .AsNoTracking()
                .Include(x => x.Noticia)
                .Where(x => x.IdUsuario == idUsuario && x.Situacao != "Excluido")
                .OrderByDescending(x => x.DataInclusao)
                .Select(x => new FavoritoResponseModel
                {
                    IdFavorito = x.IdFavorito,
                    IdUsuario = x.IdUsuario,
                    IdNoticia = x.IdNoticia,
                    Titulo = x.Noticia.Titulo,
                    Descricao = x.Noticia.Descricao,
                    Url = x.Noticia.Url,
                    UrlImagem = x.Noticia.UrlImagem,
                    FonteNome = x.Noticia.FonteNome,
                    Autor = x.Noticia.Autor,
                    DataPublicacao = x.Noticia.DataPublicacao,
                    DataFavoritada = x.DataInclusao
                })
                .ToListAsync();

            foreach (var favorito in favoritos)
            {
                retorno.Data.Lista.Add(favorito);
            }

            retorno.Success = true;
            retorno.Message = "Favoritos listados com sucesso.";
            return retorno;
        }

        private static FavoritoResponseModel MapearFavorito(Favorito favorito, Noticia noticia)
        {
            return new FavoritoResponseModel
            {
                IdFavorito = favorito.IdFavorito,
                IdUsuario = favorito.IdUsuario,
                IdNoticia = favorito.IdNoticia,
                Titulo = noticia.Titulo,
                Descricao = noticia.Descricao,
                Url = noticia.Url,
                UrlImagem = noticia.UrlImagem,
                FonteNome = noticia.FonteNome,
                Autor = noticia.Autor,
                DataPublicacao = noticia.DataPublicacao,
                DataFavoritada = favorito.DataInclusao
            };
        }
    }
}
