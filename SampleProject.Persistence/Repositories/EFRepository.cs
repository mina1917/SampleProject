using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SampleProject.Framework.Contracts;
using SampleProject.Framework.Pagination;

namespace SampleProject.Persistence.Repositories
{
    public class EFRepository<TEntity> : IWriteRepository<TEntity>, IReadRepository<TEntity>
         where TEntity : class
    {
        private readonly AppDbContext dbContext;

        public EFRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual IQueryable<TEntity> Table => dbContext.Set<TEntity>();
        public virtual IQueryable<TEntity> TableNoTracking => dbContext.Set<TEntity>().AsNoTracking();

        public TEntity Add(TEntity entity)
        {
            return dbContext.Add(entity).Entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await dbContext.AddAsync(entity, cancellationToken);

            return entity;
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            var dbSet = dbContext.Set<TEntity>();
            await dbSet.AddRangeAsync(entities);
            return;
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            var dbSet = dbContext.Set<TEntity>();
            dbSet.RemoveRange(entities);
            return;
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            var dbSet = dbContext.Set<TEntity>();
            dbSet.UpdateRange(entities);
            return;
        }


        public async Task CommitTransactionAsync(Action action, CancellationToken cancellationToken = default)
        {
            try
            {
                await dbContext.Database.BeginTransactionAsync(cancellationToken);
                action.Invoke();
                await SaveChangeAsync(cancellationToken);
                await dbContext.Database.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await dbContext.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public TEntity Delete(TEntity entity)
        {
            return dbContext.Remove(entity).Entity;
        }

        public Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(dbContext.Remove(entity).Entity);
        }

        public Task SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public TEntity Update(TEntity entity)
        {
            return dbContext.Update(entity).Entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(dbContext.Update(entity).Entity);
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return dbContext.Set<TEntity>().AnyAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken)
        {
            return ApplySpecification(spec).AnyAsync(cancellationToken);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return dbContext.Set<TEntity>().CountAsync(cancellationToken);
        }

        public Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken)
        {
            return ApplySpecification(spec).CountAsync(cancellationToken);
        }

        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return dbContext.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken)
        {
            return ApplySpecification(spec).ToListAsync(cancellationToken);
        }

        public Task<TEntity> GetAsync(CancellationToken cancellationToken)
        {
            return dbContext.Set<TEntity>().FirstOrDefaultAsync(cancellationToken);
        }

        public Task<TEntity> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken)
        {
            return ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(dbContext.Set<TEntity>().AsQueryable(), spec);
        }

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await dbContext.Set<TEntity>().FindAsync(id);
        }

        public Task<PagedList<TEntity>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return dbContext.Set<TEntity>().ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }

        public Task<PagedList<TEntity>> GetPageAsync(ISpecification<TEntity> spec, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return ApplySpecification(spec).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }
    }
}
