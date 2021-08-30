using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Dtos
{
    public class PagingRequestBase
    {
        public int pageSize { set; get; }
        public int pageIndex { set; get; }
    }
}
