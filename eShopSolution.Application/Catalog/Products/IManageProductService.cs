using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int productId);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int Quantity);
        Task UpdateViewCount(int productId);

        Task<int> AddImages(int productId, List<IFormFile> files);

        Task<int> Remove(int imageId);
        Task<int> UpdateImage(int imageId, string caption, bool isDefaut);

        Task<List<ProductImageViewModel>> GetListImage(int productId);


        //Task<List<ProductViewModel>> GetAll();

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);
            
    }
}
