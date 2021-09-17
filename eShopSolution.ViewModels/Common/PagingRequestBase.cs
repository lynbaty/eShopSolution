using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class PagingRequestBase
    {
        public int pageSize { set; get; }
        public int pageIndex { set; get; }
    }
}
