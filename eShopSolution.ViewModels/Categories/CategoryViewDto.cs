using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Categories
{
    public class CategoryViewDto
    {
        public int Id { set; get; }
        public int SortOrder { set; get; }
        public string Name { set; get; }
        public Status Status { set; get; }
    }
}