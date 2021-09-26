using eShopSolution.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.CategoryClient
{
    public interface ICategoryClient
    {
        Task<List<CategoryViewDto>> GetAll(string LanguageId);

        Task<List<CategoryViewDto>> GetbyId(int Id, string LanguageId);

        Task<bool> Delete(int Id);

        Task<bool> Create(CategoryCreateDto request);

        Task<bool> Edit(int Id, CategoryEditDto request);

        Task<CategoryViewDto> Get(int Id, string LanguageId);
    }
}