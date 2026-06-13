namespace NewsApp.Application.Models.NewsApi
{
    public class RespostaDaBuscaDeNoticiasNaNewsApiModel
    {
        public string Status { get; set; } = string.Empty;
        public int TotalResults { get; set; }
        public List<ArtigoRetornadoPelaNewsApiModel> Articles { get; set; } = new List<ArtigoRetornadoPelaNewsApiModel>();
    }
}
