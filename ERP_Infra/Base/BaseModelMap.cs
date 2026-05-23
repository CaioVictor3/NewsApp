
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proclin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Infra.Base
{
    public abstract class BaseModelMap<T> where T : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            #region Auditoria
            builder.Property(c => c.UsuarioInclusao).HasColumnType("varchar(255)");
            builder.Property(c => c.DataInclusao);
            builder.Property(c => c.UsuarioAlteracao).HasColumnType("varchar(255)");
            builder.Property(c => c.DataAlteracao);
            builder.Property(c => c.UsuarioExclusao).HasColumnType("varchar(255)");
            builder.Property(c => c.DataExclusao);
            builder.Property(c => c.Situacao).HasColumnType("varchar(255)");
            #endregion
        }
    }
}
