using eShopSolution.Application.Catalog.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Products.Dtos.Manager
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string Keyword { set; get; }

        public int[] CategoryIds { set; get; }
    }
}
