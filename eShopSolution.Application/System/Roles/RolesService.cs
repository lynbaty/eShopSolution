using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<AppRole> _rolesManager;

        public RolesService(RoleManager<AppRole> rolesManager)
        {
            _rolesManager = rolesManager;
        }

        public async Task<ApiResult<bool>> Create(RoleCreateDto request)
        {
            var role = await GetbyName(request.Name);
            if (role.Result != null) return new ApiResult<bool>(false, "Tên Roles không đúng");

            var newrole = new AppRole()
            {
                Name = request.Name,
                Description = request.Description
            };

            var result = await _rolesManager.CreateAsync(newrole);
            return new ApiResult<bool>(result.Succeeded);
        }

        public async Task<ApiResult<bool>> Delete(string Name)
        {
            var role = await GetbyName(Name);
            if (role.Result == null) return new ApiResult<bool>(false, "Tên Roles không đúng");

            var result = await _rolesManager.DeleteAsync(role.Result);
            return new ApiResult<bool>(result.Succeeded);
        }

        public async Task<ApiResult<bool>> Edit(string Name, RoleEditDto request)
        {
            var role = await GetbyName(Name);
            if (role.Result == null) return new ApiResult<bool>(false, "Không được phép thay đổi Tên phân quyền");

            role.Result.Description = request.Description;

            var result = await _rolesManager.UpdateAsync(role.Result);
            return new ApiResult<bool>(result.Succeeded);
        }

        public ApiResult<PagedResult<AppRole>> GetAllPaging(RoleGetAllDto request)
        {
            var roles = _rolesManager.Roles;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                roles = roles.Where(r => r.Name.Contains(request.Keyword) || r.Description.Contains(request.Keyword));
            }
            var total = roles.Count();
            var listroles = roles.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize).ToList();
            var result = new PagedResult<AppRole>()
            {
                TotalRecords = total,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                items = listroles
            };
            return new ApiResult<PagedResult<AppRole>>(result);
        }

        public async Task<ApiResult<AppRole>> GetbyName(string Name)
        {
            var role = await _rolesManager.FindByNameAsync(Name);
            if (role == null) return new ApiResult<AppRole>(null, "Không tìm được RoleName");
            return new ApiResult<AppRole>(role);
        }
    }
}