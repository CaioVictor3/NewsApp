using NewsApp.Domain;
using NewsApp.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsApp.Infrastructure.Mapping
{
    public class NoticiaMap : BaseModelMap<Noticia>
    {
        public override void Configure(EntityTypeBuilder<Noticia> builder)
        {
            base.Configure(builder);

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

            builder.HasIndex(n => n.Url).IsUnique();
        }
    }
}
