# NewsApp

NewsApp e uma API em .NET para sincronizar noticias sobre inteligencia artificial, salvar os registros em SQLite e permitir comentarios por usuario em cada noticia.

O projeto segue uma separacao em camadas:

- `NewsApp.Api`: API, controllers, configuracao e Swagger.
- `NewsApp.Application`: services, interfaces e DTOs de entrada/saida.
- `NewsApp.Domain`: entidades de dominio.
- `NewsApp.Infrastructure`: contexto EF Core e mapeamentos.

## Funcionalidades

- Autenticacao de usuario com JWT.
- Cadastro de usuario mobile.
- Sincronizacao de noticias da NewsAPI.
- Persistencia das noticias no banco de dados.
- Evita duplicidade de noticias usando a URL original.
- Listagem resumida de noticias salvas para feed.
- Busca de noticia por id com conteudo completo e comentarios.
- Criacao, atualizacao, exclusao logica e listagem de comentarios.

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger
- JWT Bearer
- NewsAPI
- HtmlAgilityPack

## Configuracao

As principais configuracoes ficam em:

```text
NewsApp.Api/appsettings.json
```

Exemplo da secao da NewsAPI:

```json
"NewsApi": {
  "BaseUrl": "https://newsapi.org/v2/everything",
  "ApiKey": "SUA_CHAVE_DA_NEWSAPI",
  "Query": "OpenAI OR ChatGPT OR Anthropic OR Claude OR Gemini OR NVIDIA OR \"artificial intelligence\"",
  "Language": "pt",
  "SortBy": "publishedAt"
}
```

Exemplo da conexao SQLite:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=/caminho/para/newsapp.db"
}
```

## Como Executar

Restaure os pacotes:

```bash
dotnet restore NewsApp.sln
```

Compile a solucao:

```bash
dotnet build NewsApp.sln
```

Execute a API:

```bash
dotnet run --project NewsApp.Api/NewsApp.Api.csproj
```

Depois acesse o Swagger:

```text
https://localhost:5001/swagger
```

## Fluxo de Noticias

1. O endpoint de sincronizacao chama a NewsAPI.
2. A API externa retorna no maximo a quantidade definida em `pageSize`.
3. O sistema verifica se cada noticia ja existe no banco pela URL.
4. Noticias novas sao salvas.
5. O feed lista noticias ja persistidas.
6. A tela de detalhe busca uma noticia pelo `IdNoticia` e retorna tambem seus comentarios.

## Endpoints Principais

### Usuario

```http
POST /api/usuario/login
POST /api/usuario/cadastrar-mobile
GET  /api/usuario/obter-por-id
```

### Noticias

```http
POST /api/noticia/sincronizar-news-api?page=1&pageSize=20
GET  /api/noticia/listar?page=1&pageSize=20
GET  /api/noticia/obter-por-id?idNoticia=1
```

O endpoint `obter-por-id` retorna os dados completos da noticia e a lista de comentarios vinculados.

### Comentarios

```http
POST   /api/comentario/criar
GET    /api/comentario/listar-por-noticia?idNoticia=1
PUT    /api/comentario/atualizar
DELETE /api/comentario/excluir?idComentario=1
```

Exemplo para criar comentario:

```json
{
  "idUsuario": 1,
  "idNoticia": 10,
  "comentario": "Comentario do usuario sobre a noticia."
}
```

## Banco de Dados

As principais tabelas usadas pelo fluxo atual sao:

- `Usuario`
- `Noticia`
- `Comentario`

A tabela `Comentario` referencia:

- `Usuario.IdUsuario`
- `Noticia.IdNoticia`

A tabela `Noticia` possui indice unico em `Url` para evitar duplicidade.

## Observacoes Sobre Conteudo Completo

A NewsAPI normalmente retorna o campo `content` truncado, com marcadores como:

```text
[+1859 chars]
```

Por isso, durante a sincronizacao, o sistema tenta baixar a URL original da noticia e extrair o texto principal da pagina. Se a pagina bloquear a leitura, retornar HTML insuficiente ou nao permitir extracao adequada, o sistema usa o conteudo retornado pela NewsAPI como fallback.

## Autenticacao

Os endpoints de noticia e comentario ficam protegidos por JWT.

Para acessar os endpoints protegidos pelo Swagger, primeiro cadastre um usuario pelo endpoint de cadastro. Depois, faca login com esse usuario. O login retorna um token de acesso JWT.

No Swagger, clique em `Authorize` e informe o token no formato:

```http
Authorization: Bearer SEU_TOKEN
```

Exemplo:

```text
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Estrutura Resumida

```text
NewsApp.sln
NewsApp.Api/
  Controllers/
  Startup.cs
NewsApp.Application/
  Interface/
  Models/
  Services/
NewsApp.Domain/
  Usuario.cs
  Noticia.cs
  Comentario.cs
NewsApp.Infrastructure/
  DBContext/
  Mapping/
```

## Padrao de Desenvolvimento

O projeto busca manter:

- responsabilidades separadas por camada;
- services focados em regra de aplicacao;
- controllers simples, apenas delegando chamadas;
- entidades representando o estado do dominio;
- DTOs separados para entrada e saida;
- verificacoes explicitas antes de executar operacoes sensiveis.
