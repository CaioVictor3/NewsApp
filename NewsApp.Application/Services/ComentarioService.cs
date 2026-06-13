using NewsApp.Application.Interface;
using NewsApp.Application.Models;
using NewsApp.Application.Models.Comentario;
using NewsApp.Domain;
using NewsApp.Domain.Handle;
using NewsApp.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Application.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly Context _context;

        public ComentarioService(Context context)
        {
            _context = context;
        }

        public async Task<Response<ComentarioResponseModel>?> CriarComentarioAsync(CriarComentarioRequestModel request)
        {
            var retorno = new Response<ComentarioResponseModel>()
            {
                Data = new ComentarioResponseModel()
            };

            ValidarRequest(request.IdUsuario, request.IdNoticia, request.Comentario);

            var usuarioExiste = await _context.Usuario.AnyAsync(x => x.IdUsuario == request.IdUsuario);
            if (!usuarioExiste)
                throw new Exception("Usuário não encontrado.");

            var noticiaExiste = await _context.Noticia.AnyAsync(x => x.IdNoticia == request.IdNoticia);
            if (!noticiaExiste)
                throw new Exception("Notícia não encontrada.");

            var comentario = new Comentario(request.IdUsuario, request.IdNoticia, request.Comentario);

            _context.Comentario.Add(comentario);
            await _context.SaveChangesAsync();

            retorno.Data = MapearComentario(comentario);
            retorno.Success = true;
            retorno.Message = "Comentário criado com sucesso.";
            return retorno;
        }

        public async Task<Response<ComentarioResponseModel>?> ExcluirComentarioAsync(int idComentario)
        {
            var retorno = new Response<ComentarioResponseModel>()
            {
                Data = new ComentarioResponseModel()
            };

            var comentario = await ObterComentarioAsync(idComentario);

            _context.Comentario.Remove(comentario);
            await _context.SaveChangesAsync();

            retorno.Data = MapearComentario(comentario);
            retorno.Success = true;
            retorno.Message = "Comentário excluído com sucesso.";
            return retorno;
        }

        public async Task<Response<ListarComentarioResponseModel>?> ListarComentarioPorNoticiaAsync(int idNoticia)
        {
            var retorno = new Response<ListarComentarioResponseModel>()
            {
                Data = new ListarComentarioResponseModel()
            };

            if (idNoticia <= 0)
                throw new Exception("Notícia inválida.");

            var comentarios = await _context.Comentario
                .AsNoTracking()
                .Where(x => x.IdNoticia == idNoticia)
                .OrderByDescending(x => x.DataComentario)
                .Select(x => new ComentarioResponseModel
                {
                    IdComentario = x.IdComentario,
                    Comentario = x.Conteudo,
                    IdUsuario = x.IdUsuario,
                    IdNoticia = x.IdNoticia,
                    DataComentario = x.DataComentario
                })
                .ToListAsync();

            foreach (var comentario in comentarios)
            {
                retorno.Data.Lista.Add(comentario);
            }

            retorno.Success = true;
            retorno.Message = "Comentários listados com sucesso.";
            return retorno;
        }

        public async Task<Response<ComentarioResponseModel>?> AtualizarComentarioAsync(AtualizarComentarioRequestModel request)
        {
            var retorno = new Response<ComentarioResponseModel>()
            {
                Data = new ComentarioResponseModel()
            };

            ValidarRequest(request.IdUsuario, request.IdNoticia, request.Comentario);

            var comentario = await ObterComentarioAsync(request.IdComentario);
            if (comentario.IdUsuario != request.IdUsuario || comentario.IdNoticia != request.IdNoticia)
                throw new Exception("Comentário não encontrado para o usuário e notícia informados.");

            comentario.Atualizar(request.Comentario);
            await _context.SaveChangesAsync();

            retorno.Data = MapearComentario(comentario);
            retorno.Success = true;
            retorno.Message = "Comentário atualizado com sucesso.";
            return retorno;
        }

        private static void ValidarRequest(int idUsuario, int idNoticia, string comentario)
        {
            if (idUsuario <= 0)
                throw new Exception("Usuário inválido.");

            if (idNoticia <= 0)
                throw new Exception("Notícia inválida.");

            if (string.IsNullOrWhiteSpace(comentario))
                throw new Exception("Comentário é obrigatório.");
        }

        private async Task<Comentario> ObterComentarioAsync(int idComentario)
        {
            if (idComentario <= 0)
                throw new Exception("Comentário inválido.");

            var comentario = await _context.Comentario
                .FirstOrDefaultAsync(x => x.IdComentario == idComentario);

            if (comentario == null)
                throw new Exception("Comentário não encontrado.");

            return comentario;
        }

        private static ComentarioResponseModel MapearComentario(Comentario comentario)
        {
            return new ComentarioResponseModel
            {
                IdComentario = comentario.IdComentario,
                Comentario = comentario.Conteudo,
                IdUsuario = comentario.IdUsuario,
                IdNoticia = comentario.IdNoticia,
                DataComentario = comentario.DataComentario
            };
        }
    }
}
