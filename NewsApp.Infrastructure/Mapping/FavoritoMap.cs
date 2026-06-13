using NewsApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsApp.Infrastructure.Mapping
{
    public class FavoritoMap
    {
        public void Configure(EntityTypeBuilder<Favorito> builder)
        {
            builder.HasKey(f => f.IdFavorito);
            builder.Property(f => f.DataInclusao);

            builder.HasOne(f => f.Usuario)
                .WithMany()
                .HasForeignKey(f => f.IdUsuario);

            builder.HasOne(f => f.Noticia)
                .WithMany()
                .HasForeignKey(f => f.IdNoticia);

            builder.HasIndex(f => new { f.IdUsuario, f.IdNoticia }).IsUnique();
        }
    }
}
