using eShopSolution.Application.Catalog.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos.Manager;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly eShopDbContext _context;
        public ManageProductService(eShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                ViewCount = 0,
                Stock = request.Stock,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                   new ProductTranslation()
                   {
                        Name = request.Name,
                    Description = request.Description,
                    Details = request.Details,
                    SeoAlias = request.SeoAlias,
                    SeoTitle = request.SeoTitle,
                    SeoDescription = request.SeoDescription,
                    LanguageId = request.LanguageId
                   }
                }
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Can't find product {productId}");
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();

        }

        //public async Task<List<ProductViewModel>> GetAll()
        //{
        //    var items = from p in _context.Products
        //                join pt in _context.ProductTranslations on p.Id equals pt.ProductId
        //                select new { p, pt };
        //    var totalitems =await items.CountAsync();
        //    var data =await items.Select(i => new ProductViewModel()
        //    {
        //        Id = i.p.Id,
        //        Price = i.p.Price,
        //        OriginalPrice = i.p.OriginalPrice,
        //        Stock = i.p.Stock,
        //        ViewCount = i.p.ViewCount,
        //        DateCreated = i.p.DateCreated,
        //        Name = i.pt.Name,
        //        Description = i.pt.Description,
        //        Details = i.pt.Details,
        //        SeoDescription = i.pt.SeoDescription,
        //        SeoTitle = i.pt.SeoTitle,
        //        SeoAlias = i.pt.SeoAlias,
        //        LanguageId = i.pt.LanguageId
        //    }).ToListAsync();

        //    return data;
            

        //}

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            var items = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            if(!string.IsNullOrEmpty(request.Keyword))
            {
                items = items.Where(i => i.pt.Name.Contains(request.Keyword));
            }
            if(request.CategoryIds.Count() >0)
            {
                items = items.Where(i => request.CategoryIds.Contains(i.pic.CategoryId));
            }
            
            int totalitems = items.Count();
            var data = await items.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
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
                            })
                            .ToListAsync();
            var pageResult = new PagedResult<ProductViewModel>
            {
                TotalRecord = totalitems,
                items = data
            };
            return pageResult;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            
            var producttranslation = await _context.ProductTranslations.FirstOrDefaultAsync(pt => pt.LanguageId == request.LanguageId && pt.Id == request.Id);
            if (producttranslation == null) throw new eShopException("Can't find Product to Update");
            producttranslation.Name = request.Name;
            producttranslation.Description = request.Description;
            producttranslation.Details = request.Details;
            producttranslation.SeoDescription = request.SeoDescription;
            producttranslation.SeoTitle = request.SeoTitle;
            producttranslation.SeoAlias = request.SeoAlias;
            producttranslation.LanguageId = request.LanguageId;

            return await _context.SaveChangesAsync();

           
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException("Can't find Product to Update Price");
            product.Price = newPrice;

            return (await _context.SaveChangesAsync() >0);
        }

        public async Task<bool> UpdateStock(int productId, int Quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException("Can't find Product to Update Stock");
            product.Stock = Quantity;

            return (await _context.SaveChangesAsync() > 0);

        }

        public async Task UpdateViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }
    }
}
