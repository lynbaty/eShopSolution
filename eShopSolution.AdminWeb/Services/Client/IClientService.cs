using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.Client
{
    public interface IClientService
    {
        Task<string> GetAsync(string uri);

        Task<bool> PostAsync(string uri, HttpContent content);

        Task<bool> PutAsync(string uri, HttpContent content);

        Task<bool> DeleteAsync(string uri);
    }
}