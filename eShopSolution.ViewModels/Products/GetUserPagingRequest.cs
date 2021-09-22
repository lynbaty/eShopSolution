using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Products
{
    public class UserRequestDto : PagingRequestBase
    {
        public string Keyword { set; get; }
    }
}