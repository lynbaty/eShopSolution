using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.Client
{
    public class ClientService : IClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ClientService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public HttpClient GetClient()
        {
            var token = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            return client;
        }

        public async Task<string> GetAsync(string uri)
        {
            var client = GetClient();
            var response = await client.GetAsync(uri);
            var body = await response.Content.ReadAsStringAsync();
            return body;
        }

        public async Task<bool> PostAsync(string uri, HttpContent content)
        {
            var client = GetClient();
            var response = await client.PostAsync(uri, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(string uri, HttpContent content)
        {
            var client = GetClient();
            var response = await client.PutAsync(uri, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string uri)
        {
            var client = GetClient();
            var response = await client.DeleteAsync(uri);
            return response.IsSuccessStatusCode;
        }
    }
}