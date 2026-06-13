namespace NewsApp.Application.Models.NewsApi
{
    public class RespostaDeErroRetornadaPelaNewsApiModel
    {
        public string Status { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
