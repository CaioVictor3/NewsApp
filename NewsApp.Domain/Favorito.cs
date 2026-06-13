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

        public Favorito(int idUsuario, int idNoticia)
        {
            IdUsuario = idUsuario;
            IdNoticia = idNoticia;
            DataInclusao = DateTime.Now;
        }
    }
}
