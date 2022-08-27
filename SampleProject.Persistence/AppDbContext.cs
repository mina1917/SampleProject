using Microsoft.EntityFrameworkCore;
using SampleProject.Framework.Contracts;

namespace SampleProject.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {

                if (entry.Entity is not IAuditable<Guid>)
                    continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["CreatedOn"] = DateTime.Now;
                        entry.CurrentValues["Creator"] = Guid.NewGuid();
                        entry.CurrentValues["ModifiedOn"] = null;
                        entry.CurrentValues["Modifire"] = null;
                        break;

                    case EntityState.Modified:
                        entry.CurrentValues["CreatedOn"] = entry.OriginalValues["CreatedOn"];
                        entry.CurrentValues["Creator"] = entry.OriginalValues["Creator"];
                        entry.CurrentValues["ModifiedOn"] = DateTime.Now;
                        entry.CurrentValues["Modifire"] = null;
                        break;
                }
            }

            var entiries = await base.SaveChangesAsync(cancellationToken);


            return entiries;
        }
    }
}
