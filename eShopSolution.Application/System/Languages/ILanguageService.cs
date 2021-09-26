using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Languages
{
    public interface ILanguageService
    {
        Task<List<LanguageDto>> GetAll();
    }
}