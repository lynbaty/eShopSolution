using eShopSolution.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface ICategoryApi
    {
        Task<List<CategoryViewDto>> GetAll(string LanguageId);

        Task<List<CategoryViewDto>> GetbyId(int Id, string LanguageId);

        Task<bool> Delete(int Id);

        Task<bool> Create(CategoryCreateDto request);

        Task<bool> Edit(int Id, CategoryEditDto request);

        Task<CategoryViewDto> Details(int Id, string LanguageId);
    }
}