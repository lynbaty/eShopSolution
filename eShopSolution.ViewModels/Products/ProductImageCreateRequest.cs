using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Products
{
    public class ProductImageCreateDto
    {
        public string Caption { get; set; }

        public int SortOrder { set; get; }

        public bool IsDefault { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}