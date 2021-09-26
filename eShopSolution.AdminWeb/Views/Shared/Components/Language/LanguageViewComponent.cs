using eShopSolution.AdminWeb.Services.LanguageClient;
using eShopSolution.ViewModels.System.Languages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Views.Shared.Components.Language
{
    public class Language : ViewComponent
    {
        private readonly ILanguageClient _languageClient;

        public Language(ILanguageClient languageClient)
        {
            _languageClient = languageClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rs = await _languageClient.GetAll();

            var x = new LanguageModel()
            {
                Languages = rs,
                CurrentLanguageId = HttpContext.Session.GetString("DefaultLanguageId")
            };

            return View<LanguageModel>(x);
        }
    }
}