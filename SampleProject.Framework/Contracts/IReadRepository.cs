using Ardalis.Specification;
using SampleProject.Framework.Pagination;

namespace SampleProject.Framework.Contracts
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }
        Task<TEntity> GetAsync(CancellationToken cancellationToken);
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TEntity> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken);
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken);
        Task<bool> AnyAsync(CancellationToken cancellationToken);
        Task<bool> AnyAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken);
        Task<PagedList<TEntity>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<PagedList<TEntity>> GetPageAsync(ISpecification<TEntity> spec, int pageIndex, int pageSize, CancellationToken cancellationToken);

    }

}
