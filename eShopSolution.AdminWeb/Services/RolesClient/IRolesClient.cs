using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Services.RolesClient
{
    public interface IRolesClient
    {
        Task<bool> Create(RoleCreateDto request);

        Task<PagedResult<RoleViewDto>> GetAllPaging(RoleGetAllDto request);

        Task<ApiResult<bool>> Edit(string name, RoleEditDto Request);

        Task<RoleViewDto> GetbyId(string name);

        Task<bool> Delete(string name);
    }
}