using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<string> Authencate(LoginRequestDto request);

        Task<bool> Register(RegisterRequestDto request);

        Task<PagedResult<UserViewModel>> GetUserAllPaging(UserRequestDto request);

        Task<bool> Update(Guid Id, UserViewModel request);

        Task<UserViewModel> GetbyId(Guid Id);

        Task<bool> Delete(Guid Id);

        Task<IList<string>> GetRolesbyId(Guid Id);

        Task<bool> AddRolesbyId(Guid Id, UserRolesDto roles);
    }
}