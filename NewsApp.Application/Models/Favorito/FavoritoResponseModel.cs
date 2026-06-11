namespace NewsApp.Application.Models.Favorito
{
    public class FavoritoResponseModel
    {
        public int IdFavorito { get; set; }
        public int IdUsuario { get; set; }
        public int IdNoticia { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string UrlImagem { get; set; } = string.Empty;
        public string FonteNome { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public DateTime DataFavoritada { get; set; }
    }
}
