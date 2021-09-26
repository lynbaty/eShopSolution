using eShopSolution.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.LanguageClient
{
    public interface ILanguageClient
    {
        Task<List<LanguageDto>> GetAll();
    }
}