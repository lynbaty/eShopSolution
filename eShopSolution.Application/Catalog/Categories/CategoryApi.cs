using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Common;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Categories
{
    public class CategoryApi : ICategoryApi
    {
        private readonly eShopDbContext _context;

        public CategoryApi(eShopDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(CategoryCreateDto request)
        {
            var cate = new Category()
            {
                SortOrder = request.SortOrder,
                IsShowOnHome = request.IsShowOnHome,
                ParentId = request.ParentId,
                Status = request.Status,
                CategoryTranslations = new List<CategoryTranslation>
                {
                    new CategoryTranslation()
                    {
                        Name = request.Name,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId,
                        SeoAlias = request.SeoAlias
                    }
                }
            };
            _context.Categories.Add(cate);
            var rs = await _context.SaveChangesAsync();
            return rs > 0;
        }

        public async Task<bool> Delete(int Id)
        {
            var cate = await _context.Categories.FindAsync(Id);
            _context.Categories.Remove(cate);
            var catet = _context.CategoryTranslations.Where(x => x.CategoryId == Id);
            if (catet.Count() > 0)
            {
                _context.CategoryTranslations.RemoveRange(catet);
            }
            var rs = await _context.SaveChangesAsync();
            return rs > 0;
        }

        public async Task<CategoryViewDto> Details(int Id, string LanguageId)
        {
            var i = await (from c in _context.Categories
                           join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                           where ct.LanguageId == LanguageId && c.Id == Id
                           select new { c, ct }).FirstOrDefaultAsync();
            if (i != null)
            {
                var rs = new CategoryViewDto()
                {
                    Id = i.c.Id,
                    Name = i.ct.Name,
                    SortOrder = i.c.SortOrder,
                    Status = i.c.Status
                };
                return rs;
            }
            return null;
        }

        public async Task<bool> Edit(int Id, CategoryEditDto request)
        {
            var cate = await _context.Categories.FindAsync(Id);
            if (cate == null) return false;
            cate.IsShowOnHome = request.IsShowOnHome;
            cate.ParentId = request.ParentId;
            cate.SortOrder = request.SortOrder;
            cate.Status = request.Status;
            _context.Categories.Update(cate);
            var catetr = _context.CategoryTranslations.FirstOrDefault(ct => ct.LanguageId == request.LanguageId && ct.CategoryId == Id);
            if (catetr != null)
            {
                catetr.Name = request.Name;
                catetr.SeoAlias = request.SeoAlias;
                catetr.SeoTitle = request.SeoTitle;
                catetr.SeoDescription = request.SeoDescription;
                _context.CategoryTranslations.Update(catetr);
            }
            var rs = await _context.SaveChangesAsync();
            return rs > 0;
        }

        public async Task<List<CategoryViewDto>> GetAll(string LanguageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == LanguageId
                        select new { c, ct };

            var rs = await query.Select(i => new CategoryViewDto()
            {
                Id = i.c.Id,
                Name = i.ct.Name,
                SortOrder = i.c.SortOrder,
                Status = i.c.Status
            }).ToListAsync();

            return rs;
        }

        public async Task<List<CategoryViewDto>> GetbyId(int Id, string LanguageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                        where ct.LanguageId == LanguageId
                        select new { c, ct, pic };

            var rs = await query.Where(q => q.pic.ProductId == Id).Select(i => new CategoryViewDto()
            {
                Id = i.c.Id,
                Name = i.ct.Name,
                SortOrder = i.c.SortOrder,
                Status = i.c.Status
            }).ToListAsync();
            return rs;
        }
    }
}