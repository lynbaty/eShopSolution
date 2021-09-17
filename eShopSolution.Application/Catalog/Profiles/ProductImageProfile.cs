using AutoMapper;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catalog.Profiles
{
    class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImage, ProductImageViewModel>();
        }
    }
}
