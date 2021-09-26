using AutoMapper;
using eShopSolution.Application.Catalog.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Categories;
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
    public class ProductService : IProductService
    {
        private readonly eShopDbContext _context;
        private readonly IStorageService _storageService;

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            var items = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pic };

            if (request.CategoryId.HasValue && request.CategoryId > 0)
            {
                items = items.Where(i => request.CategoryId == i.pic.CategoryId);
            }
            if (request.CategoryId != null)
            {
                items = items.Where(i => i.pic.CategoryId == request.CategoryId);
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
                            }).ToListAsync();
            var data2 = data.GroupBy(d => d.Id).Select(g => g.First()).ToList();
            var pageResult = new PagedResult<ProductViewModel>
            {
                TotalRecords = totalitems,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                items = data2
            };
            return pageResult;
        }

        public ProductService(eShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
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
            if (request.ThumbailImage != null)
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
            await _context.SaveChangesAsync();

            return product.Id;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
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
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                            //join c in _context.Categories on pic.CategoryId equals c.Id into picc
                            //from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pic };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                items = items.Where(i => i.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null)
            {
                items = items.Where(i => i.pic.CategoryId.ToString() == request.CategoryId);
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
            var data2 = data.GroupBy(d => d.Id).Select(g => g.First()).ToList();
            var pageResult = new PagedResult<ProductViewModel>
            {
                TotalRecords = totalitems,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                items = data2
            };
            return pageResult;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var producttranslation = await _context.ProductTranslations.FirstOrDefaultAsync(pt => pt.LanguageId == request.LanguageId && pt.ProductId == request.Id);
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
                if (thumbnail != null)
                {
                    thumbnail.FileSize = request.ThumbailImage.Length;
                    thumbnail.ImagePath = await this.SaveFile(request.ThumbailImage);
                    _context.ProductImages.Update(thumbnail);
                }
            }
            _context.ProductTranslations.Update(producttranslation);
            await _context.SaveChangesAsync();
            return request.Id;
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException("Can't find Product to Update Price");
            product.Price = newPrice;

            return (await _context.SaveChangesAsync() > 0);
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

        public async Task<int> AddImage(int productId, ProductImageCreateDto request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> Remove(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            await _storageService.DeleteFileAsync(image.ImagePath);
            _context.ProductImages.Remove(image);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var image = await _context.ProductImages.FindAsync(imageId);

            image.Caption = (request.Caption != null) ? request.Caption : image.Caption;
            image.IsDefault = request.IsDefault;
            image.SortOrder = (request.SortOrder != 0) ? request.SortOrder : image.SortOrder;

            if (request.ImageFile != null)
            {
                image.ImagePath = await this.SaveFile(request.ImageFile);
                image.FileSize = request.ImageFile.Length;
            }
            await _context.SaveChangesAsync();
            return image.Id;
        }

        public async Task<List<ProductImage>> GetListImage(int productId)
        {
            var Imageviews = await _context.ProductImages.Where(i => i.ProductId == productId)
                                                .ToListAsync();

            return Imageviews;
        }

        public async Task<ProductViewModel> GetbyId(int productId, string languageId)
        {
            var p = await _context.Products.FindAsync(productId);
            var pt = await _context.ProductTranslations.FirstOrDefaultAsync(i => i.LanguageId == languageId && i.ProductId == productId);

            if (pt != null)
            {
                var result = new ProductViewModel()
                {
                    Id = p.Id,
                    Price = p.Price,
                    OriginalPrice = p.OriginalPrice,
                    Stock = p.Stock,
                    ViewCount = p.ViewCount,
                    DateCreated = p.DateCreated,
                    Name = pt.Name,
                    Description = pt.Description,
                    Details = pt.Details,
                    SeoDescription = pt.SeoDescription,
                    SeoTitle = pt.SeoTitle,
                    SeoAlias = pt.SeoAlias,
                    LanguageId = pt.LanguageId
                };

                return result;
            }
            return null;
        }

        public async Task<ProductImage> GetImageById(int imageId)
        {
            var result = await _context.ProductImages.FindAsync(imageId);

            return result;
        }

        public async Task<bool> SetCategories(int ProductId, List<ProductCategoryDto> request)
        {
            var add = request.Where(r => r.Selected == true).Select(r => r.Id).ToList();
            var removeall = await _context.ProductInCategories.Where(pt => pt.ProductId == ProductId).ToListAsync();
            var addall = add.Select(a => new ProductInCategory()
            {
                ProductId = ProductId,
                CategoryId = a
            });
            _context.ProductInCategories.RemoveRange(removeall);
            _context.ProductInCategories.AddRange(addall);

            var rs = _context.SaveChanges();
            return rs > 0;
        }
    }
}