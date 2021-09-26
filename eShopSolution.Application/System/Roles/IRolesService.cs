using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public interface IRolesService
    {
        Task<ApiResult<AppRole>> GetbyName(string Name);

        ApiResult<PagedResult<AppRole>> GetAllPaging(RoleGetAllDto request);

        Task<ApiResult<bool>> Create(RoleCreateDto request);

        Task<ApiResult<bool>> Delete(string Name);

        Task<ApiResult<bool>> Edit(string Name, RoleEditDto request);
    }
}