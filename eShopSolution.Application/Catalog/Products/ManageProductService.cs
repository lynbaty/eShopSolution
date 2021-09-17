
using AutoMapper;
using eShopSolution.Application.Catalog.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly eShopDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public ManageProductService(eShopDbContext context,IStorageService storageService,IMapper mapper)
        {
            _context = context;
            _storageService = storageService;
            _mapper = mapper;
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
            if(request.ThumbailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }    
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Can't find product {productId}");
            var fileName = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var file in fileName)
            {
                await _storageService.DeleteFileAsync(file.ImagePath);
            }
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

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
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

            if (request.ThumbailImage != null)
            {
                var thumbnail = _context.ProductImages.Where(i => i.IsDefault == true && i.ProductId == request.Id).FirstOrDefault();
                if(thumbnail != null)
                {
                        thumbnail.FileSize = request.ThumbailImage.Length;
                        thumbnail.ImagePath = await this.SaveFile(request.ThumbailImage);
                    _context.ProductImages.Update(thumbnail);
                }    
            }

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

        public async Task<int> AddImages(int productId, List<IFormFile> files)
        {
            var product = await _context.Products.FindAsync(productId);

            foreach(var file in files)
            {

                product.ProductImages.Add(
                    new ProductImage()
                    {
                        Caption = Guid.NewGuid() + "Thumbail image",
                        DateCreated = DateTime.Now,
                        FileSize = file.Length,
                        ImagePath = await this.SaveFile(file),
                        IsDefault = true,
                        SortOrder = 1
                    });
                
            }
            return await _context.SaveChangesAsync();


        }

        public async Task<int> Remove(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            await _storageService.DeleteFileAsync(image.ImagePath);
            _context.ProductImages.Remove(image);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, string caption, bool isDefaut)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image != null && isDefaut)
            {
                foreach (var item in _context.ProductImages)
                {
                    item.IsDefault = false;
                }
            }
            if (image != null)
            {
                image.Caption = caption;
              
                image.IsDefault = isDefaut;
            }
            return await _context.SaveChangesAsync();
            

        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
   
            var Imageviews =await _context.ProductImages.Where(i => i.Id == productId)
                                                .Select(i => _mapper.Map<ProductImageViewModel>(i)).ToListAsync();
                       
            
            return Imageviews;
        }
    }
}
