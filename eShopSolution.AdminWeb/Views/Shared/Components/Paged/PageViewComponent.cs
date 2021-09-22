using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Controllers.ViewComponents
{
    public class Paged : ViewComponent
    {
        public IViewComponentResult Invoke(PagedResultBase result)
        {
            return View<PagedResultBase>(result);
        }
    }
}