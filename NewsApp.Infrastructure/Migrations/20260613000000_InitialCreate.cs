using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Noticia",
                columns: table => new
                {
                    IdNoticia = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FonteId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    FonteNome = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Autor = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    UrlImagem = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Conteudo = table.Column<string>(type: "TEXT", nullable: true),
                    DataInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Situacao = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noticia", x => x.IdNoticia);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Senha = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Endereco = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CPF = table.Column<string>(type: "TEXT", maxLength: 14, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TipoUsuario = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    DataInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Situacao = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Comentario",
                columns: table => new
                {
                    IdComentario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Conteudo = table.Column<string>(type: "TEXT", nullable: true),
                    DataComentario = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    IdNoticia = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Situacao = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.IdComentario);
                    table.ForeignKey(
                        name: "FK_Comentario_Noticia_IdNoticia",
                        column: x => x.IdNoticia,
                        principalTable: "Noticia",
                        principalColumn: "IdNoticia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentario_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorito",
                columns: table => new
                {
                    IdFavorito = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    IdNoticia = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Situacao = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorito", x => x.IdFavorito);
                    table.ForeignKey(
                        name: "FK_Favorito_Noticia_IdNoticia",
                        column: x => x.IdNoticia,
                        principalTable: "Noticia",
                        principalColumn: "IdNoticia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorito_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_IdNoticia",
                table: "Comentario",
                column: "IdNoticia");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_IdUsuario",
                table: "Comentario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_IdNoticia",
                table: "Favorito",
                column: "IdNoticia");

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_IdUsuario_IdNoticia",
                table: "Favorito",
                columns: new[] { "IdUsuario", "IdNoticia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Noticia_Url",
                table: "Noticia",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Comentario");
            migrationBuilder.DropTable(name: "Favorito");
            migrationBuilder.DropTable(name: "Noticia");
            migrationBuilder.DropTable(name: "Usuario");
        }
    }
}
