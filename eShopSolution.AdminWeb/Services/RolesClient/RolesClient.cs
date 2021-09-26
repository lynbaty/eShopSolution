using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.RolesClient
{
    public class RolesClient : IRolesClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public RolesClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> Create(RoleCreateDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = GetClient();
            var response = await client.PostAsync("/api/Roles", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(string name)
        {
            var client = GetClient();
            var response = await client.DeleteAsync($"/api/Roles/{name}");

            return response.IsSuccessStatusCode;
        }

        public async Task<ApiResult<bool>> Edit(string name, RoleEditDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = GetClient();
            var response = await client.PutAsync($"/api/Roles/{name}", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return new ApiResult<bool>(false, body);
            return new ApiResult<bool>(true);
        }

        public async Task<PagedResult<RoleViewDto>> GetAllPaging(RoleGetAllDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = GetClient();
            var response = await client.GetAsync($"/api/Roles?Keyword={request.Keyword}&pageSize={request.pageSize}&pageIndex={request.pageIndex}");

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedResult<RoleViewDto>>(body);
            return result;
        }

        public async Task<RoleViewDto> GetbyId(string name)
        {
            var client = GetClient();
            var response = await client.GetAsync($"/api/Roles/{name}");

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RoleViewDto>(body);
            return result;
        }

        public HttpClient GetClient()
        {
            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            return client;
        }
    }
}