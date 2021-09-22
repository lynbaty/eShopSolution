using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services
{
    public interface IUserClient
    {
        Task<string> Authencate(LoginRequestDto Request);

        Task<bool> Register(RegisterRequestDto Request);

        Task<PagedResult<UserViewModel>> GetAllPaging(UserRequestDto Request);

        Task<bool> Update(Guid Id, UserViewModel Request);

        Task<UserViewModel> GetbyId(Guid Id);

        Task<bool> Delete(Guid Id);
    }
}