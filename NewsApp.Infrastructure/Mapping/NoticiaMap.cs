using NewsApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsApp.Infrastructure.Mapping
{
    public class NoticiaMap
    {
        public void Configure(EntityTypeBuilder<Noticia> builder)
        {
            builder.HasKey(n => n.IdNoticia);

            builder.Property(n => n.FonteId).HasMaxLength(100);
            builder.Property(n => n.FonteNome).HasMaxLength(255);
            builder.Property(n => n.Autor).HasMaxLength(500);
            builder.Property(n => n.Titulo).IsRequired().HasMaxLength(1000);
            builder.Property(n => n.Descricao).HasMaxLength(2000);
            builder.Property(n => n.Url).IsRequired().HasMaxLength(2048);
            builder.Property(n => n.UrlImagem).HasMaxLength(2048);
            builder.Property(n => n.DataPublicacao).IsRequired();
            builder.Property(n => n.Conteudo);
            builder.Property(n => n.DataInclusao);
            builder.Property(n => n.Situacao).HasColumnType("varchar(255)");

            builder.HasIndex(n => n.Url).IsUnique();
        }
    }
}
