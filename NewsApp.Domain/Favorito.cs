namespace NewsApp.Domain
{
    public class Favorito
    {
        public int IdFavorito { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdNoticia { get; set; }
        public Noticia Noticia { get; set; }
        public DateTime DataInclusao { get; set; }

        public Favorito() { }

        public Favorito(Usuario usuario, Noticia noticia)
        {
            Usuario = usuario;
            Noticia = noticia;
            DataInclusao = DateTime.Now;
        }
    }
}
