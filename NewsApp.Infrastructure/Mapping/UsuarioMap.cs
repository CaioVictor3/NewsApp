using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proclin.Models;
using NewsApp.Infrastructure.Base;

namespace NewsApp.Infrastructure.Mapping
{
    public class UsuarioMap : BaseModelMap<Usuario>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            base.Configure(builder);

            builder.HasKey(c => c.IdUsuario);
            builder.Property(c => c.Nome).HasMaxLength(200);
            builder.Property(c => c.Login).HasMaxLength(50);
            builder.Property(c => c.Senha).HasMaxLength(100);
            builder.Property(c => c.Email).HasMaxLength(150);
            builder.Property(c => c.CPF).HasMaxLength(14);
            builder.Property(c => c.Endereco).HasMaxLength(500);
            builder.Property(c => c.DataNascimento);
            builder.Property(c => c.TipoUsuario).HasMaxLength(20);
        }
    }
}
