using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsApp.Domain;

namespace NewsApp.Infrastructure.Mapping
{
    public class UsuarioMap
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(c => c.IdUsuario);
            builder.Property(c => c.Nome).HasMaxLength(200);
            builder.Property(c => c.Login).HasMaxLength(50);
            builder.Property(c => c.Senha).HasMaxLength(100);
            builder.Property(c => c.Email).HasMaxLength(150);
            builder.Property(c => c.CPF).HasMaxLength(14);
            builder.Property(c => c.Endereco).HasMaxLength(500);
            builder.Property(c => c.DataNascimento);
            builder.Property(c => c.TipoUsuario).HasMaxLength(20);
            builder.Property(c => c.DataInclusao);
            builder.Property(c => c.Situacao).HasColumnType("varchar(255)");
        }
    }
}
