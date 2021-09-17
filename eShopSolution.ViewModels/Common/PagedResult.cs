using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class PagedResult<T>
    {
        public int TotalRecord { set; get; }
        public List<T> items { set; get; }
    }
}
