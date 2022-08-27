using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleProject.Framework.Pagination
{

    public class PagedList<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int IndexFrom { get; set; }

        public IList<T> Items { get; set; }

        public bool HasPreviousPage => PageIndex - IndexFrom > 0;

        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;


        public PagedList(IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
        {
            PageIndex = pageNumber;
            PageSize = pageSize;
            IndexFrom = pageNumber * pageSize;

            var itemList = source.ToList();
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            Items = itemList.Skip(IndexFrom).Take(PageSize).ToList();
        }

        public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            PageIndex = pageNumber;
            PageSize = pageSize;
            IndexFrom = pageNumber * pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = source.Skip(IndexFrom).Take(PageSize).ToList();
        }

        public PagedList()
        {
            Items = new T[0];
        }
    }

}