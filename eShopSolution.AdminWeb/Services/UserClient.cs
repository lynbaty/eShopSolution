using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using eShopSolution.ViewModels.System.Users;
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

namespace eShopSolution.AdminWeb.Services
{
    public class UserClient : IUserClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> Authencate(LoginRequestDto Request)
        {
            var Json = JsonConvert.SerializeObject(Request);
            var httpContent = new StringContent(Json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var response = await client.PostAsync("/api/Users/authenticate", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<PagedResult<UserViewModel>> GetAllPaging(UserRequestDto Request)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/Users?Keyword={Request.Keyword}&pageSize={Request.pageSize}&pageIndex={Request.pageIndex}");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(body);
            return result;
        }

        public async Task<bool> Register(RegisterRequestDto Request)
        {
            var Json = JsonConvert.SerializeObject(Request);
            var httpContent = new StringContent(Json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var response = await client.PostAsync("/api/Users", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(Guid Id, UserViewModel Request)
        {
            var Json = JsonConvert.SerializeObject(Request);
            var httpContent = new StringContent(Json, Encoding.UTF8, "application/json");

            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"/api/Users/{Request.Id}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<UserViewModel> GetbyId(Guid Id)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/Users/{Id}");

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserViewModel>(body);

            return result;
        }

        public async Task<bool> Delete(Guid Id)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"/api/Users/{Id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IList<string>> GetRolesbyId(Guid Id)
        {
            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/Users/{Id}/Roles");

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IList<string>>(body);

            return result;
        }

        public async Task<bool> AddRolesbyId(Guid Id, UserRolesDto roles)
        {
            var Json = JsonConvert.SerializeObject(roles);
            var httpContent = new StringContent(Json, Encoding.UTF8, "application/json");

            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"/api/Users/{Id}/Roles", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}