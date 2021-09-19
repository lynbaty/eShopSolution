using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Products
{
    public class ProductImageUpdateRequest
    {
        public string Caption { get; set; }

        public int SortOrder { set; get; } = 0;

        public bool IsDefault { get; set; } = true;

        public IFormFile ImageFile { get; set; }
    }
}