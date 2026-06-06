namespace NewsApp.Application.Models.Noticia
{
    public class SincronizarNoticiasResponseModel
    {
        public int TotalResultadosEncontradosNaNewsApi { get; set; }
        public int QuantidadeNoticiasRetornadasNaPaginaAtual { get; set; }
        public int QuantidadeNoticiasSalvasNoBanco { get; set; }
        public List<NoticiaResumoResponseModel> Noticias { get; set; } = new List<NoticiaResumoResponseModel>();
    }

    public class ListarNoticiaResponseModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRegistros { get; set; }
        public List<NoticiaResumoResponseModel> Lista { get; set; } = new List<NoticiaResumoResponseModel>();
    }

    public class NoticiaResumoResponseModel
    {
        public int IdNoticia { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string FonteNome { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string UrlImagem { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string Url { get; set; } = string.Empty;
    }

    public class NoticiaDetalheResponseModel
    {
        public int IdNoticia { get; set; }
        public string FonteId { get; set; } = string.Empty;
        public string FonteNome { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string UrlImagem { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string Conteudo { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public List<ComentarioNoticiaResponseModel> Comentarios { get; set; } = new List<ComentarioNoticiaResponseModel>();
    }

    public class ComentarioNoticiaResponseModel
    {
        public int IdComentario { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime DataComentario { get; set; }
    }
}
