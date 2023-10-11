namespace MyTask.Models
{
    public class PageParams
    {
        private const int maxPageSize = 50;
        public int Pages { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string OrderBy { get; set; } = string.Empty;
        public bool OrderAsc { get; set; } = true;
        public string? FilteringByUserName { get; set; }  
        public int? FilteringByUserId { get; set; } 
        public int? FilteringByUserAge { get; set; }
        public string? FilteringByUserEmail { get; set; } 
        public string? FilteringByRoleName { get; set; } 
        public int? FilteringByRoleId { get; set; } 
    }
}
