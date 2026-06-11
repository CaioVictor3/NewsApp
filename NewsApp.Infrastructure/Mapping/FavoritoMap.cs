using NewsApp.Domain;
using NewsApp.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsApp.Infrastructure.Mapping
{
    public class FavoritoMap : BaseModelMap<Favorito>
    {
        public override void Configure(EntityTypeBuilder<Favorito> builder)
        {
            base.Configure(builder);

            builder.HasKey(f => f.IdFavorito);

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
