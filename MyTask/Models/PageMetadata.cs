using System.Linq.Dynamic.Core;

namespace MyTask.Models
{
    public class PageMetadata<T> : List<T>
    {     
        public PageMetadata(List<T> items, int totalCount, int currentPage, int pageSize)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);
        }

        public int TotalCount { get; set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get => (int) Math.Ceiling(TotalCount / (double) PageSize); }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public static List<T> ToPagedSortList(IQueryable<T> source, string orderBy, bool orderAsc)
        {
            string orderFlow = orderAsc ? "ascending" : "descending";
            var items = new List<T>();
            try
            {
                items = source.OrderBy($"{orderBy} {orderFlow}").ToList();
            }
            catch (Exception ex)
            {
                return null;
            }           

            return items;
        }
        public static PageMetadata<T> ToPagedPaginationList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PageMetadata<T>(items, count, pageNumber, pageSize);
        }
    }
}
