using NewsApp.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Infrastructure.Base
{
    public abstract class ContextBase : DbContext, IUnitOfWorkBase
    {
        // protected readonly AuditoriaContext AuditoriaContext;

        protected ContextBase(DbContextOptions pOptions) : base(pOptions)
        {

        }

        public virtual int SaveChanges()
        {
            var lEntitiesAdded = new List<EntityEntry>();
            var lEntitiesModified = new List<EntityEntry>();
            var lEntitiesDeleted = new List<EntityEntry>();

            /* if (psSessaoId.PossuiUmValor())
            {
                foreach (var lEntry in ChangeTracker.Entries())
                {
                    switch (lEntry.State)
                    {
                        case EntityState.Added: lEntitiesAdded.Add(lEntry); break;
                        case EntityState.Modified: lEntitiesModified.Add(lEntry); break;
                        case EntityState.Deleted: lEntitiesDeleted.Add(lEntry); break;
                    }
                }

                //await GravarLogAuditoriaAsync(lEntitiesAdded, psSessaoId!, EntityState.Added);
                //await GravarLogAuditoriaAsync(lEntitiesModified, psSessaoId!, EntityState.Modified);
                //await GravarLogAuditoriaAsync(lEntitiesDeleted, psSessaoId!, EntityState.Deleted);
            } */
            try
            {
                var x = base.SaveChanges();
                return x;
            }
            catch (DbUpdateException ex)
            {
                return 0;
            }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            /*  var lEntitiesAdded = new List<EntityEntry>();
            var lEntitiesModified = new List<EntityEntry>();
            var lEntitiesDeleted = new List<EntityEntry>();

            /* if (psSessaoId.PossuiUmValor())
            {
                foreach (var lEntry in ChangeTracker.Entries())
                {
                    switch (lEntry.State)
                    {
                        case EntityState.Added: lEntitiesAdded.Add(lEntry); break;
                        case EntityState.Modified: lEntitiesModified.Add(lEntry); break;
                        case EntityState.Deleted: lEntitiesDeleted.Add(lEntry); break;
                    }
                }

                await GravarLogAuditoriaAsync(lEntitiesAdded, psSessaoId!, EntityState.Added);
                await GravarLogAuditoriaAsync(lEntitiesModified, psSessaoId!, EntityState.Modified);
                await GravarLogAuditoriaAsync(lEntitiesDeleted, psSessaoId!, EntityState.Deleted);
            } */


            return await base.SaveChangesAsync();
        }

        private async Task GravarLogAuditoriaAsync(IEnumerable<EntityEntry> plEntities, string psSessaoId, EntityState pState)
        {
            //var lbSave = false;

            //foreach (var lEntity in plEntities)
            //{
            //    Guid lgPrimaryKeyId = Guid.Empty;
            //    string lsKeyName = "Id";

            //    if (lsKeyName != null && lEntity.Entity?.GetType().GetProperty(lsKeyName)?.GetValue(lEntity.Entity) != null)
            //        lgPrimaryKeyId = Guid.Parse(lEntity.Entity!.GetType().GetProperty(lsKeyName)!.GetValue(lEntity!.Entity!)!.ToString()!);


            //    AuditoriaContext.Auditoria.Add(new AuditoriaModel()
            //    {
            //        SessaoId = Guid.Parse(psSessaoId),
            //        Situacao = pState,
            //        DataHora = DateTime.Now,
            //        Classe = lEntity!.Entity!.ToString()!,
            //        Chave = lgPrimaryKeyId,
            //        Json = JsonConvert.SerializeObject(
            //            lEntity.Entity,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            })
            //    });

            //    lbSave = true;
            //}

            //if (lbSave)
            //    await AuditoriaContext.SaveChangesAsync();
        }

        public void DiscardChanges()
        {
            ChangeTracker.Clear();
            foreach (var lEntry in ChangeTracker.Entries())
            {
                switch (lEntry.State)
                {
                    case EntityState.Added:
                        lEntry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        lEntry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        lEntry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
