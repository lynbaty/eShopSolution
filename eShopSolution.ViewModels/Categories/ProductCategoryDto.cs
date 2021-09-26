using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.OData.Query.SemanticAst;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Categories
{
    public class ProductCategoryDto
    {
        public string Name { set; get; }

        public int Id { set; get; }

        public bool Selected { set; get; }
    }
}