using Microsoft.EntityFrameworkCore;

namespace SampleProject.Framework.Pagination
{
    public static class QueryablePageListExtensions
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default)
        {
            if (pageSize == 0)
                pageSize = 10;

            if (pageIndex == 0)
                pageIndex = 1;

            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageNumber: {pageIndex}, must indexFrom <= pageNumber");
            }
            pageIndex--;
            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

            var items = await source.Skip((pageIndex - indexFrom) * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>
            {
                PageIndex = ++pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }
    }
}


