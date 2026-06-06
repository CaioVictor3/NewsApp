using System.Globalization;
using System.Text.Json;
using NewsApp.Application.Interface;
using NewsApp.Application.Models;
using NewsApp.Application.Models.Noticia;
using NewsApp.Domain;
using NewsApp.Domain.Handle;
using NewsApp.Infrastructure.DBContext;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NewsApp.Application.Services
{
    public class NoticiaService : INoticiaService
    {
        private const int PageSizePadrao = 20;
        private const string QueryPadraoNoticiasInteligenciaArtificial = "OpenAI OR ChatGPT OR Anthropic OR Claude OR Gemini OR NVIDIA OR \"artificial intelligence\"";
        private const int TamanhoMinimoConteudoExtraido = 300;

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public NoticiaService(Context context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Response<SincronizarNoticiasResponseModel>?> BuscarNoticiasDaNewsApiESalvarNoBancoAsync(int page = 1, int pageSize = PageSizePadrao)
        {
            ValidarParametrosDePaginacao(page, pageSize);

            var noticiasDaNewsApi = await BuscarNoticiasNaNewsApiAsync(page, pageSize);
            var artigosValidosRetornadosPelaNewsApi = FiltrarArtigosValidosParaCadastro(noticiasDaNewsApi.Articles);
            var noticiasSalvas = await SalvarSomenteNoticiasAindaNaoCadastradasAsync(artigosValidosRetornadosPelaNewsApi);

            return new Response<SincronizarNoticiasResponseModel>
            {
                Success = true,
                Message = "Notícias sincronizadas com sucesso.",
                Data = new SincronizarNoticiasResponseModel
                {
                    TotalResultadosEncontradosNaNewsApi = noticiasDaNewsApi.TotalResults,
                    QuantidadeNoticiasRetornadasNaPaginaAtual = noticiasDaNewsApi.Articles.Count,
                    QuantidadeNoticiasSalvasNoBanco = noticiasSalvas.Count,
                    Noticias = noticiasSalvas.Select(MapearNoticiaParaResumoResponse).ToList()
                }
            };
        }

        public async Task<Response<ListarNoticiaResponseModel>?> ListarNoticiasSalvasNoBancoAsync(int page = 1, int pageSize = PageSizePadrao)
        {
            ValidarParametrosDePaginacao(page, pageSize);

            var consultaNoticiasSalvas = _context.Noticia
                .AsNoTracking()
                .Where(noticia => noticia.Situacao != "Excluido")
                .OrderByDescending(noticia => noticia.DataPublicacao);

            var totalRegistros = await consultaNoticiasSalvas.CountAsync();
            var noticias = await consultaNoticiasSalvas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(noticia => new NoticiaResumoResponseModel
                {
                    IdNoticia = noticia.IdNoticia,
                    Titulo = noticia.Titulo,
                    Descricao = noticia.Descricao,
                    FonteNome = noticia.FonteNome,
                    Autor = noticia.Autor,
                    UrlImagem = noticia.UrlImagem,
                    DataPublicacao = noticia.DataPublicacao,
                    Url = noticia.Url
                })
                .ToListAsync();

            return new Response<ListarNoticiaResponseModel>
            {
                Success = true,
                Message = "Notícias listadas com sucesso.",
                Data = new ListarNoticiaResponseModel
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalRegistros = totalRegistros,
                    Lista = noticias
                }
            };
        }

        public async Task<Response<NoticiaDetalheResponseModel>?> BuscarNoticiaSalvaPorIdAsync(int idNoticia)
        {
            if (idNoticia <= 0)
                throw new ServiceException("Notícia inválida.");

            var noticia = await _context.Noticia
                .AsNoTracking()
                .FirstOrDefaultAsync(noticia => noticia.IdNoticia == idNoticia && noticia.Situacao != "Excluido");

            if (noticia == null)
                throw new ServiceException("Notícia não encontrada.");

            var comentarios = await _context.Comentario
                .AsNoTracking()
                .Where(comentario => comentario.IdNoticia == idNoticia && comentario.Situacao != "Excluido")
                .OrderByDescending(comentario => comentario.DataComentario)
                .Select(comentario => new ComentarioNoticiaResponseModel
                {
                    IdComentario = comentario.IdComentario,
                    IdUsuario = comentario.IdUsuario,
                    Comentario = comentario.Conteudo,
                    DataComentario = comentario.DataComentario
                })
                .ToListAsync();

            return new Response<NoticiaDetalheResponseModel>
            {
                Success = true,
                Message = "Notícia encontrada com sucesso.",
                Data = MapearNoticiaParaDetalheResponse(noticia, comentarios)
            };
        }

        private async Task<NewsApiResponseModel> BuscarNoticiasNaNewsApiAsync(int page, int pageSize)
        {
            var urlDaNewsApi = MontarUrlDaNewsApiComFiltrosPadrao(page, pageSize);
            var apiKey = ObterApiKeyConfiguradaDaNewsApi();
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, urlDaNewsApi);

            request.Headers.Add("X-Api-Key", apiKey);
            request.Headers.UserAgent.ParseAdd("NewsApp/1.0");

            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ServiceException(MontarMensagemDeErroDaNewsApi(response, json));

            var retornoNewsApi = JsonSerializer.Deserialize<NewsApiResponseModel>(json, CriarOpcoesDeDesserializacaoDaNewsApi());

            if (retornoNewsApi == null || !string.Equals(retornoNewsApi.Status, "ok", StringComparison.OrdinalIgnoreCase))
                throw new ServiceException(MontarMensagemDeRespostaInvalidaDaNewsApi(json));

            return retornoNewsApi;
        }

        private string MontarUrlDaNewsApiComFiltrosPadrao(int page, int pageSize)
        {
            var baseUrl = _configuration["NewsApi:BaseUrl"];
            var query = _configuration["NewsApi:Query"] ?? QueryPadraoNoticiasInteligenciaArtificial;
            var language = _configuration["NewsApi:Language"] ?? "pt";
            var sortBy = _configuration["NewsApi:SortBy"] ?? "publishedAt";

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ServiceException("URL base da NewsAPI não configurada.");

            return string.Concat(
                baseUrl,
                "?q=", Uri.EscapeDataString(query),
                "&language=", Uri.EscapeDataString(language),
                "&sortBy=", Uri.EscapeDataString(sortBy),
                "&page=", page.ToString(CultureInfo.InvariantCulture),
                "&pageSize=", pageSize.ToString(CultureInfo.InvariantCulture));
        }

        private string ObterApiKeyConfiguradaDaNewsApi()
        {
            var apiKey = _configuration["NewsApi:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ServiceException("Chave da NewsAPI não configurada.");

            return apiKey;
        }

        private static List<NewsApiArticleModel> FiltrarArtigosValidosParaCadastro(IEnumerable<NewsApiArticleModel> artigos)
        {
            return artigos
                .Where(artigo => NoticiaDaNewsApiPossuiDadosMinimosParaCadastro(artigo))
                .ToList();
        }

        private async Task<List<Noticia>> SalvarSomenteNoticiasAindaNaoCadastradasAsync(List<NewsApiArticleModel> artigos)
        {
            var urlsRetornadasPelaNewsApi = artigos
                .Select(artigo => artigo.Url)
                .Distinct()
                .ToList();

            var urlsJaCadastradasNoBanco = await _context.Noticia
                .Where(noticia => urlsRetornadasPelaNewsApi.Contains(noticia.Url))
                .Select(noticia => noticia.Url)
                .ToListAsync();

            var urlsControladasDuranteEstaSincronizacao = urlsJaCadastradasNoBanco.ToHashSet();
            var noticiasSalvasNestaSincronizacao = new List<Noticia>();

            foreach (var artigo in artigos)
            {
                if (urlsControladasDuranteEstaSincronizacao.Contains(artigo.Url))
                    continue;

                var conteudoCompleto = await BuscarConteudoCompletoDaNoticiaNaUrlOriginalAsync(artigo);
                var noticia = CriarNoticiaDoDominioAPartirDoArtigoDaNewsApi(artigo, conteudoCompleto);

                _context.Noticia.Add(noticia);
                noticiasSalvasNestaSincronizacao.Add(noticia);
                urlsControladasDuranteEstaSincronizacao.Add(artigo.Url);
            }

            if (noticiasSalvasNestaSincronizacao.Count > 0)
                await _context.SaveChangesAsync();

            return noticiasSalvasNestaSincronizacao;
        }

        private async Task<string> BuscarConteudoCompletoDaNoticiaNaUrlOriginalAsync(NewsApiArticleModel artigo)
        {
            var conteudoOriginalDaNewsApi = artigo.Content ?? string.Empty;

            if (string.IsNullOrWhiteSpace(artigo.Url))
                return conteudoOriginalDaNewsApi;

            if (!Uri.TryCreate(artigo.Url, UriKind.Absolute, out var uriDaNoticiaOriginal))
                return conteudoOriginalDaNewsApi;

            var html = await BaixarHtmlDaNoticiaOriginalAsync(uriDaNoticiaOriginal);

            if (string.IsNullOrWhiteSpace(html))
                return conteudoOriginalDaNewsApi;

            var conteudoExtraidoDoHtml = ExtrairTextoPrincipalDaPaginaHtml(html);

            if (!string.IsNullOrWhiteSpace(conteudoExtraidoDoHtml)
                && conteudoExtraidoDoHtml.Length >= TamanhoMinimoConteudoExtraido)
            {
                return conteudoExtraidoDoHtml;
            }

            return conteudoOriginalDaNewsApi;
        }

        private async Task<string> BaixarHtmlDaNoticiaOriginalAsync(Uri uriDaNoticiaOriginal)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, uriDaNoticiaOriginal);

            request.Headers.UserAgent.ParseAdd("NewsApp/1.0");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return string.Empty;

            return await response.Content.ReadAsStringAsync();
        }

        private static string ExtrairTextoPrincipalDaPaginaHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            var documento = new HtmlDocument();
            documento.LoadHtml(html);

            RemoverNosQueNaoFazemParteDoConteudoPrincipal(documento);

            var noPrincipal = BuscarNoMaisProvavelDoConteudoPrincipal(documento);
            var textoExtraido = noPrincipal?.InnerText ?? string.Empty;

            return NormalizarEspacosDoTextoExtraido(textoExtraido);
        }

        private static void RemoverNosQueNaoFazemParteDoConteudoPrincipal(HtmlDocument documento)
        {
            var nosParaRemover = documento.DocumentNode.SelectNodes("//script|//style|//noscript|//iframe|//nav|//header|//footer|//aside|//form");

            if (nosParaRemover == null)
                return;

            foreach (var no in nosParaRemover)
                no.Remove();
        }

        private static HtmlNode? BuscarNoMaisProvavelDoConteudoPrincipal(HtmlDocument documento)
        {
            var seletoresPrioritarios = new[]
            {
                "//article",
                "//*[contains(@class, 'article')]",
                "//*[contains(@class, 'post-content')]",
                "//*[contains(@class, 'entry-content')]",
                "//*[contains(@class, 'content')]",
                "//*[@id='content']",
                "//main"
            };

            foreach (var seletor in seletoresPrioritarios)
            {
                var noEncontrado = documento.DocumentNode.SelectSingleNode(seletor);

                if (noEncontrado != null && NormalizarEspacosDoTextoExtraido(noEncontrado.InnerText).Length >= TamanhoMinimoConteudoExtraido)
                    return noEncontrado;
            }

            return documento.DocumentNode.SelectSingleNode("//body");
        }

        private static string NormalizarEspacosDoTextoExtraido(string texto)
        {
            var textoSemEspacosDuplicados = HtmlEntity.DeEntitize(texto);
            return string.Join(
                " ",
                textoSemEspacosDuplicados.Split(
                    new[] { ' ', '\r', '\n', '\t' },
                    StringSplitOptions.RemoveEmptyEntries));
        }

        private static Noticia CriarNoticiaDoDominioAPartirDoArtigoDaNewsApi(NewsApiArticleModel artigo, string conteudo)
        {
            return new Noticia(
                artigo.Source?.Id ?? string.Empty,
                artigo.Source?.Name ?? string.Empty,
                artigo.Author ?? string.Empty,
                artigo.Title ?? string.Empty,
                artigo.Description ?? string.Empty,
                artigo.Url,
                artigo.UrlToImage ?? string.Empty,
                artigo.PublishedAt,
                conteudo);
        }

        private static bool NoticiaDaNewsApiPossuiDadosMinimosParaCadastro(NewsApiArticleModel artigo)
        {
            return !string.IsNullOrWhiteSpace(artigo.Url)
                && !string.IsNullOrWhiteSpace(artigo.Title);
        }

        private static void ValidarParametrosDePaginacao(int page, int pageSize)
        {
            if (page <= 0)
                throw new ServiceException("Página inválida.");

            if (pageSize <= 0)
                throw new ServiceException("Tamanho da página inválido.");

            if (pageSize > PageSizePadrao)
                throw new ServiceException("A busca da NewsAPI está limitada a 20 notícias por página.");
        }

        private static JsonSerializerOptions CriarOpcoesDeDesserializacaoDaNewsApi()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private static string MontarMensagemDeErroDaNewsApi(HttpResponseMessage response, string json)
        {
            var erroNewsApi = DesserializarErroDaNewsApi(json);

            if (erroNewsApi != null && !string.IsNullOrWhiteSpace(erroNewsApi.Message))
                return $"Não foi possível buscar notícias na NewsAPI. Status HTTP: {(int)response.StatusCode}. Código: {erroNewsApi.Code}. Mensagem: {erroNewsApi.Message}";

            return $"Não foi possível buscar notícias na NewsAPI. Status HTTP: {(int)response.StatusCode}.";
        }

        private static string MontarMensagemDeRespostaInvalidaDaNewsApi(string json)
        {
            var erroNewsApi = DesserializarErroDaNewsApi(json);

            if (erroNewsApi != null && !string.IsNullOrWhiteSpace(erroNewsApi.Message))
                return $"A NewsAPI retornou uma resposta inválida. Código: {erroNewsApi.Code}. Mensagem: {erroNewsApi.Message}";

            return "A NewsAPI retornou uma resposta inválida.";
        }

        private static NewsApiErrorResponseModel? DesserializarErroDaNewsApi(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            var jsonNormalizado = json.Trim();

            if (!jsonNormalizado.StartsWith("{") || !jsonNormalizado.EndsWith("}"))
                return null;

            var code = ExtrairValorStringDoJsonPorNomeDaPropriedade(jsonNormalizado, "code");
            var message = ExtrairValorStringDoJsonPorNomeDaPropriedade(jsonNormalizado, "message");

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(message))
                return null;

            return new NewsApiErrorResponseModel
            {
                Code = code,
                Message = message
            };
        }

        private static string ExtrairValorStringDoJsonPorNomeDaPropriedade(string json, string nomePropriedade)
        {
            var propriedade = $"\"{nomePropriedade}\"";
            var indicePropriedade = json.IndexOf(propriedade, StringComparison.OrdinalIgnoreCase);

            if (indicePropriedade < 0)
                return string.Empty;

            var indiceSeparador = json.IndexOf(':', indicePropriedade + propriedade.Length);

            if (indiceSeparador < 0)
                return string.Empty;

            var indiceInicioValor = json.IndexOf('"', indiceSeparador + 1);

            if (indiceInicioValor < 0)
                return string.Empty;

            var indiceAtual = indiceInicioValor + 1;
            var valor = new List<char>();

            while (indiceAtual < json.Length)
            {
                var caractereAtual = json[indiceAtual];

                if (caractereAtual == '"' && json[indiceAtual - 1] != '\\')
                    break;

                if (caractereAtual == '\\' && indiceAtual + 1 < json.Length)
                {
                    indiceAtual++;
                    caractereAtual = json[indiceAtual];
                }

                valor.Add(caractereAtual);
                indiceAtual++;
            }

            return new string(valor.ToArray());
        }

        private static NoticiaResumoResponseModel MapearNoticiaParaResumoResponse(Noticia noticia)
        {
            return new NoticiaResumoResponseModel
            {
                IdNoticia = noticia.IdNoticia,
                Titulo = noticia.Titulo,
                Descricao = noticia.Descricao,
                FonteNome = noticia.FonteNome,
                Autor = noticia.Autor,
                UrlImagem = noticia.UrlImagem,
                DataPublicacao = noticia.DataPublicacao,
                Url = noticia.Url
            };
        }

        private static NoticiaDetalheResponseModel MapearNoticiaParaDetalheResponse(
            Noticia noticia,
            List<ComentarioNoticiaResponseModel> comentarios)
        {
            return new NoticiaDetalheResponseModel
            {
                IdNoticia = noticia.IdNoticia,
                FonteId = noticia.FonteId,
                FonteNome = noticia.FonteNome,
                Autor = noticia.Autor,
                Titulo = noticia.Titulo,
                Descricao = noticia.Descricao,
                Url = noticia.Url,
                UrlImagem = noticia.UrlImagem,
                DataPublicacao = noticia.DataPublicacao,
                Conteudo = noticia.Conteudo,
                DataCadastro = noticia.DataInclusao,
                Comentarios = comentarios
            };
        }

        private class NewsApiResponseModel
        {
            public string Status { get; set; } = string.Empty;
            public int TotalResults { get; set; }
            public List<NewsApiArticleModel> Articles { get; set; } = new List<NewsApiArticleModel>();
        }

        private class NewsApiErrorResponseModel
        {
            public string Status { get; set; } = string.Empty;
            public string Code { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
        }

        private class NewsApiArticleModel
        {
            public NewsApiSourceModel? Source { get; set; }
            public string? Author { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string Url { get; set; } = string.Empty;
            public string? UrlToImage { get; set; }
            public DateTime PublishedAt { get; set; }
            public string? Content { get; set; }
        }

        private class NewsApiSourceModel
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
    }
}
