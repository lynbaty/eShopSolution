﻿using AutoMapper;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Products;
using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.BackendApi.Profiles
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImage, ProductImageViewModel>();
            CreateMap<AppRole, RoleViewDto>();
        }
    }
}