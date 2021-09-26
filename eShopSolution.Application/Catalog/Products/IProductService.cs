using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Categories;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService

    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);

        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<ProductViewModel> GetbyId(int productId, string languageId);

        Task<int> Delete(int productId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int Quantity);

        Task UpdateViewCount(int productId);

        Task<int> AddImage(int productId, ProductImageCreateDto request);

        Task<int> Remove(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<List<ProductImage>> GetListImage(int productId);

        Task<ProductImage> GetImageById(int imageId);

        //Task<List<ProductViewModel>> GetAll();

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<bool> SetCategories(int ProductId, List<ProductCategoryDto> request);
    }
}