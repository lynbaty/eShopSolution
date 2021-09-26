using eShopSolution.AdminWeb.Services.Client;
using eShopSolution.ViewModels.Categories;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.ProductsClient
{
    public class ProductClient : IProductClient
    {
        private readonly IClientService _clientService;

        public ProductClient(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(string uri)
        {
            var body = await _clientService.GetAsync(uri);
            var result = JsonConvert.DeserializeObject<PagedResult<ProductViewModel>>(body);
            return result;
        }

        public async Task<bool> Create(ProductCreateRequest request)
        {
            using var httpContent = new MultipartFormDataContent();
            if (request.ThumbailImage != null)
            {
                new StreamContent(request.ThumbailImage.OpenReadStream());
                httpContent.Add(new StreamContent(request.ThumbailImage.OpenReadStream()), "ThumbailImage", request.ThumbailImage.FileName);
            }

            httpContent.Add(new StringContent(request.Description.ToString(), System.Text.Encoding.UTF8), "Description");
            httpContent.Add(new StringContent(request.Name.ToString(), System.Text.Encoding.UTF8), "Name");
            httpContent.Add(new StringContent(request.Price.ToString(), System.Text.Encoding.UTF8), "Price");
            httpContent.Add(new StringContent(request.LanguageId.ToString(), System.Text.Encoding.UTF8), "LanguageId");
            httpContent.Add(new StringContent(request.Details.ToString(), System.Text.Encoding.UTF8), "Details");
            httpContent.Add(new StringContent(request.OriginalPrice.ToString(), System.Text.Encoding.UTF8), "OriginalPrice");
            httpContent.Add(new StringContent(request.SeoAlias.ToString(), System.Text.Encoding.UTF8), "SeoAlias");
            httpContent.Add(new StringContent(request.SeoDescription.ToString(), System.Text.Encoding.UTF8), "SeoDescription");
            httpContent.Add(new StringContent(request.SeoTitle.ToString(), System.Text.Encoding.UTF8), "SeoTitle");
            httpContent.Add(new StringContent(request.Stock.ToString(), System.Text.Encoding.UTF8), "Stock");

            var rs = await _clientService.PostAsync("/api/Products", httpContent);
            return rs;
        }

        public async Task<ProductViewModel> Details(int Id, string LanguagueId)
        {
            var body = await _clientService.GetAsync($"/{Id}/{LanguagueId}");
            var result = JsonConvert.DeserializeObject<ProductViewModel>(body);
            return result;
        }

        public async Task<bool> Edit(ProductUpdateRequest request)
        {
            using var httpContent = new MultipartFormDataContent();
            if (request.ThumbailImage != null)
            {
                new StreamContent(request.ThumbailImage.OpenReadStream());
                httpContent.Add(new StreamContent(request.ThumbailImage.OpenReadStream()), "ThumbailImage", request.ThumbailImage.FileName);
            }
            httpContent.Add(new StringContent(request.Description.ToString(), System.Text.Encoding.UTF8), "Description");
            httpContent.Add(new StringContent(request.Name.ToString(), System.Text.Encoding.UTF8), "Name");
            httpContent.Add(new StringContent(request.LanguageId.ToString(), System.Text.Encoding.UTF8), "LanguageId");
            httpContent.Add(new StringContent(request.Details.ToString(), System.Text.Encoding.UTF8), "Details");
            httpContent.Add(new StringContent(request.SeoAlias.ToString(), System.Text.Encoding.UTF8), "SeoAlias");
            httpContent.Add(new StringContent(request.SeoDescription.ToString(), System.Text.Encoding.UTF8), "SeoDescription");
            httpContent.Add(new StringContent(request.SeoTitle.ToString(), System.Text.Encoding.UTF8), "SeoTitle");
            httpContent.Add(new StringContent(request.Id.ToString(), System.Text.Encoding.UTF8), "Id");

            var rs = await _clientService.PutAsync("/api/Products", httpContent);
            return rs;
        }

        public async Task<bool> Delete(int Id)
        {
            var result = await _clientService.DeleteAsync($"/{Id}");
            return result;
        }

        public async Task<bool> SetCategory(int Id, List<ProductCategoryDto> request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _clientService.PostAsync($"/categories?productId={Id}", httpContent);
            return result;
        }
    }
}