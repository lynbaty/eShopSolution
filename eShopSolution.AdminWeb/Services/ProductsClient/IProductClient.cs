using eShopSolution.ViewModels.Categories;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.ProductsClient
{
    public interface IProductClient
    {
        Task<PagedResult<ProductViewModel>> GetAllPaging(string uri);

        Task<bool> Create(ProductCreateRequest request);

        Task<ProductViewModel> Details(int Id, string LanguagueId);

        Task<bool> Edit(ProductUpdateRequest request);

        //Task<UserViewModel> GetbyId(Guid Id);

        Task<bool> Delete(int id);

        Task<bool> SetCategory(int Id, List<ProductCategoryDto> request);

        //Task<IList<string>> GetRolesbyId(Guid Id);

        //Task<bool> AddRolesbyId(Guid Id, UserRolesDto roles);
        //Task<string> Authencate(LoginRequestDto Request);

        //Task<bool> Register(RegisterRequestDto Request);
    }
}