using ERP_Domain;
using ERP_Infra.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_Infra.Mapping
{
    public class ComentarioMap : BaseModelMap<Comentario>
    {
        public override void Configure(EntityTypeBuilder<Comentario> builder)
        {
            base.Configure(builder);

            builder.HasKey(c => c.IdComentario);
            builder.Property(c => c.Conteudo).IsRequired().HasMaxLength(1000);
            builder.Property(c => c.DataPublicacao);
            builder.Property(c => c.ExternalNewsId).IsRequired().HasMaxLength(100);

            builder.HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.IdUsuario);
        }
    }
}
