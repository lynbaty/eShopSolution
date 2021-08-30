using eShopSolution.Application.Catalog.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos.Public;
using eShopSolution.Data.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService

    {
        private readonly eShopDbContext _context;
        public PublicProductService(eShopDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request)
        {
            var items = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            if (request.CategoryId.HasValue && request.CategoryId > 0)
            {
                items = items.Where(i => request.CategoryId == i.pic.CategoryId);
            }
           

            int totalitems = items.Count();
            var data =await items.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                            .Select(i => new ProductViewModel()
                            {
                                Id = i.p.Id,
                                Price = i.p.Price,
                                OriginalPrice = i.p.OriginalPrice,
                                Stock = i.p.Stock,
                                ViewCount = i.p.ViewCount,
                                DateCreated = i.p.DateCreated,
                                Name = i.pt.Name,
                                Description = i.pt.Description,
                                Details = i.pt.Details,
                                SeoDescription = i.pt.SeoDescription,
                                SeoTitle = i.pt.SeoTitle,
                                SeoAlias = i.pt.SeoAlias,
                                LanguageId = i.pt.LanguageId
                            }).ToListAsync();
            var pageResult = new PagedResult<ProductViewModel>
            {
                TotalRecord = totalitems,
                items = data
            };
            return pageResult;
        }
    }
}
