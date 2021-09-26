using eShopSolution.AdminWeb.Services.Client;
using eShopSolution.ViewModels.Categories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.CategoryClient
{
    public class CategoryClient : ICategoryClient
    {
        private readonly IClientService _clientService;

        public CategoryClient(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<List<CategoryViewDto>> GetAll(string LanguageId)
        {
            string uri = $"/api/Categories?LanguageId={LanguageId}";
            var respond = await _clientService.GetAsync(uri);
            var result = JsonConvert.DeserializeObject<List<CategoryViewDto>>(respond);

            return result;
        }

        public async Task<List<CategoryViewDto>> GetbyId(int Id, string LanguageId)
        {
            string uri = $"/api/Categories/{Id}/{LanguageId}";
            var respond = await _clientService.GetAsync(uri);
            var result = JsonConvert.DeserializeObject<List<CategoryViewDto>>(respond);

            return result;
        }

        public async Task<bool> Delete(int Id)
        {
            string uri = $"/api/Categories/{Id}";
            var respond = await _clientService.DeleteAsync(uri);
            return respond;
        }

        public async Task<bool> Create(CategoryCreateDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            string uri = "/api/Categories";

            var respond = await _clientService.PostAsync(uri, httpContent);
            return respond;
        }

        public async Task<bool> Edit(int Id, CategoryEditDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            string uri = $"/api/Categories/{Id}";

            var respond = await _clientService.PutAsync(uri, httpContent);
            return respond;
        }

        public async Task<CategoryViewDto> Get(int Id, string LanguageId)
        {
            string uri = $"/api/Categories/{Id}?LanguageId={LanguageId}";
            var respond = await _clientService.GetAsync(uri);

            var result = JsonConvert.DeserializeObject<CategoryViewDto>(respond);
            return result;
        }
    }
}