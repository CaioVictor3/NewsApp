using NewsApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsApp.Infrastructure.Mapping
{
    public class ComentarioMap
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.HasKey(c => c.IdComentario);
            builder.Property(c => c.Conteudo).IsRequired();
            builder.Property(c => c.DataComentario).IsRequired();
            builder.Property(c => c.IdNoticia).IsRequired();
            builder.Property(c => c.DataInclusao);

            builder.HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.IdUsuario);

            builder.HasOne(c => c.Noticia)
                .WithMany()
                .HasForeignKey(c => c.IdNoticia);
        }
    }
}
