namespace NewsApp.Application.Models.NewsApi
{
    public class ArtigoRetornadoPelaNewsApiModel
    {
        public FonteDoArtigoRetornadoPelaNewsApiModel? Source { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? Content { get; set; }
    }
}
