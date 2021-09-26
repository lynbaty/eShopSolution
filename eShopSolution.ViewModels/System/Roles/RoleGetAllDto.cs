using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Roles
{
    public class RoleGetAllDto : PagingRequestBase
    {
        public string Keyword { set; get; }
    }
}