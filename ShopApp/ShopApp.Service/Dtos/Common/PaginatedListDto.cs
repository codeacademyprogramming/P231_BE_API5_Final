using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service.Dtos.Common
{
    public class PaginatedListDto<T>
    {
        public PaginatedListDto(List<T> items, int pageIndex, int pageSize,int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext => PageIndex < TotalPages;
        public bool HasPrev => PageIndex > 1;
    }
}
