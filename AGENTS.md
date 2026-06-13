# NewsApp — Agent Guide

## Quick start

```bash
dotnet restore NewsApp.sln
dotnet build NewsApp.sln
dotnet run --project NewsApp.Api/NewsApp.Api.csproj
```

Swagger at `https://localhost:5001/swagger` (profile `DESENV`; also `PROD`).

## Architecture

4-project .NET 8 solution, clean-ish layers:

| Project | Responsibility |
|---|---|---|
| `NewsApp.Api` | Controllers, Startup, Swagger, MVC Views, wwwroot static files |
| `NewsApp.Application` | Services, interfaces, DTOs (`Models/`), JWT token generation |
| `NewsApp.Domain` | Entities (`Usuario`, `Noticia`, `Comentario`, `Favorito`), base model, domain exceptions |
| `NewsApp.Infrastructure` | EF Core `DbContext`, entity mappings, repository base, SQLite |

## Key conventions

- **All endpoints return `Response<T>`** (`Application/Models/Response.cs`) with `Success`, `Message`, `Data`.
- **Soft delete** — entities have `Situacao` ("Ativo"/"Excluido") via `BaseModel.SetUsuarioExclusao()`. Queries filter `Situacao != "Excluido"`.
- **Portuguese route naming** (e.g. `/api/usuario/login`, `/api/noticia/sincronizar-news-api`).
- **`DomainException`** returns generic "Internal Server Error" in error handler. **`ServiceException`** leaks its message to the client.
- **Duplicate news prevention**: unique index on `Noticia.Url`.
- **Duplicate favorito prevention**: unique index on `Favorito (IdUsuario, IdNoticia)`.

## Tooling

```bash
# Format (csharpier installed as local tool)
dotnet csharpier .

# EF migrations (must run from NewsApp.Api directory for the local tool manifest)
dotnet ef migrations add NomeMigration --project ../NewsApp.Infrastructure --startup-project .
dotnet ef database update --project ../NewsApp.Infrastructure --startup-project .
```

`dotnet-ef` is declared in `NewsApp.Api/.config/dotnet-tools.json` (run `dotnet tool restore` first if missing). CSharpier is at the repo root.

## Endpoints — Favorito

| Method | Route | Action |
|---|---|---|
| POST | `/api/favorito/adicionar` | Add favorite (`{ idUsuario, idNoticia }`) |
| DELETE | `/api/favorito/remover?idFavorito=` | Remove favorite (soft delete) |
| GET | `/api/favorito/listar-por-usuario?idUsuario=` | List user's favorites |

All protected by JWT. Duplicate (user+news) prevented by unique index.

## Gotchas

- **No tests exist** — no test project in the solution.
- **DB path in `appsettings.json` is hardcoded** to a developer machine path. Override via `ConnectionStrings:DefaultConnection`.
- **JWT secret hardcoded** in two places (`Startup.cs:45`, `TokenService.cs:13`). Both must stay in sync.
- **EF Core Migrations folder** lives at `NewsApp.Infrastructure/Migrations/`.
- **MediatR registered** in Startup (`Line 28`) but not actually used by any visible handler.
- Namespace leak: several Domain files use `Proclin.Models` (from a former project template).
- `package-lock.json` refers to `AppNews` (old name) — no real Node deps.
- `libman.json` at `NewsApp.Api/libman.json` is empty / unused.

## Node.js backend (`backendNode/`)

Simplified Express reimplementation of the same API, written in TypeScript.

```bash
cd backendNode
npm install
npm run dev      # dev with hot-reload (tsx watch)
npm run build    # compile to dist/
npm start        # run compiled dist/server.js
```

Single-process, synchronous SQLite (`better-sqlite3`). Same endpoints and `Response<T>` shape as the .NET API. JWT secret matches the C# one for dev convenience. Swagger UI at `http://localhost:3000/api-docs`. No test suite.
