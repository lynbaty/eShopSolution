using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Dtos
{
    public class PagedResult<T>
    {
        public int TotalRecord { set; get; }
        public List<T> items { set; get; }
    }
}
