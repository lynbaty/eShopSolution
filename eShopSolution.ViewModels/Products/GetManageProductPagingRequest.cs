using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { set; get; }

        public string LanguageId { set; get; }

        public string CategoryId { set; get; }
    }
}