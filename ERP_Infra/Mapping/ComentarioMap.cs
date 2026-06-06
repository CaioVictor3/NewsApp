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
            builder.Property(c => c.Conteudo).IsRequired();
            builder.Property(c => c.DataComentario).IsRequired();
            builder.Property(c => c.IdNoticia).IsRequired();

            builder.HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.IdUsuario);

            builder.HasOne(c => c.Noticia)
                .WithMany()
                .HasForeignKey(c => c.IdNoticia);
        }
    }
}
