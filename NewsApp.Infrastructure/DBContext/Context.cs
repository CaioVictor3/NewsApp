using NewsApp.Domain;
using NewsApp.Infrastructure.Base;
using NewsApp.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Proclin.Models;

namespace NewsApp.Infrastructure.DBContext
{
	public class Context : ContextBase, IUnitOfWork
	{
		public DbSet<Usuario> Usuario { get; set; }
		public DbSet<Comentario> Comentario { get; set; }
		public DbSet<Noticia> Noticia { get; set; }

		public Context(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Usuario>(new UsuarioMap().Configure);
			modelBuilder.Entity<Comentario>(new ComentarioMap().Configure);
			modelBuilder.Entity<Noticia>(new NoticiaMap().Configure);

			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				foreach (var property in entityType.GetProperties())
				{
					if (property.ClrType == typeof(string))
					{
						property.IsNullable = true;
					}
				}
			}
		}

		public async Task<bool> CommitAsync()
		{
			return await base.SaveChangesAsync() > 0;
		}
	}
}
