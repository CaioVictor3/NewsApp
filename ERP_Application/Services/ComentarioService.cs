using ERP_Application.Interface;
using ERP_Application.Models;
using ERP_Application.Models.Comentario;
using ERP_Domain;
using ERP_Domain.Handle;
using ERP_Infra.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ERP_Application.Services
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
            ValidarRequest(request.IdUsuario, request.IdNoticia, request.Comentario);

            var usuarioExiste = await _context.Usuario.AnyAsync(x => x.IdUsuario == request.IdUsuario);
            if (!usuarioExiste)
                throw new ServiceException("Usuário não encontrado.");

            var comentario = new Comentario(request.IdUsuario, request.IdNoticia, request.Comentario);

            _context.Comentario.Add(comentario);
            await _context.SaveChangesAsync();

            return new Response<ComentarioResponseModel>
            {
                Data = MapearComentario(comentario),
                Success = true,
                Message = "Comentário criado com sucesso."
            };
        }

        public async Task<Response<ComentarioResponseModel>?> ExcluirComentarioAsync(int idComentario)
        {
            var comentario = await ObterComentarioAtivoAsync(idComentario);

            comentario.Remover("Sistema");
            await _context.SaveChangesAsync();

            return new Response<ComentarioResponseModel>
            {
                Data = MapearComentario(comentario),
                Success = true,
                Message = "Comentário excluído com sucesso."
            };
        }

        public async Task<Response<ListarComentarioResponseModel>?> ListarComentarioPorNoticiaAsync(int idNoticia)
        {
            if (idNoticia <= 0)
                throw new ServiceException("Notícia inválida.");

            var comentarios = await _context.Comentario
                .AsNoTracking()
                .Where(x => x.IdNoticia == idNoticia && x.Situacao != "Excluido")
                .OrderByDescending(x => x.DataComentario)
                .Select(x => MapearComentario(x))
                .ToListAsync();

            return new Response<ListarComentarioResponseModel>
            {
                Data = new ListarComentarioResponseModel { Lista = comentarios },
                Success = true,
                Message = "Comentários listados com sucesso."
            };
        }

        public async Task<Response<ComentarioResponseModel>?> AtualizarComentarioAsync(AtualizarComentarioRequestModel request)
        {
            ValidarRequest(request.IdUsuario, request.IdNoticia, request.Comentario);

            var comentario = await ObterComentarioAtivoAsync(request.IdComentario);
            if (comentario.IdUsuario != request.IdUsuario || comentario.IdNoticia != request.IdNoticia)
                throw new ServiceException("Comentário não encontrado para o usuário e notícia informados.");

            comentario.Atualizar(request.Comentario, "Sistema");
            await _context.SaveChangesAsync();

            return new Response<ComentarioResponseModel>
            {
                Data = MapearComentario(comentario),
                Success = true,
                Message = "Comentário atualizado com sucesso."
            };
        }

        private static void ValidarRequest(int idUsuario, int idNoticia, string comentario)
        {
            if (idUsuario <= 0)
                throw new ServiceException("Usuário inválido.");

            if (idNoticia <= 0)
                throw new ServiceException("Notícia inválida.");

            if (string.IsNullOrWhiteSpace(comentario))
                throw new ServiceException("Comentário é obrigatório.");
        }

        private async Task<Comentario> ObterComentarioAtivoAsync(int idComentario)
        {
            if (idComentario <= 0)
                throw new ServiceException("Comentário inválido.");

            var comentario = await _context.Comentario
                .FirstOrDefaultAsync(x => x.IdComentario == idComentario && x.Situacao != "Excluido");

            if (comentario == null)
                throw new ServiceException("Comentário não encontrado.");

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
