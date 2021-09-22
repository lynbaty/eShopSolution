using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(IConfiguration config, RoleManager<AppRole> roleManager,
                           SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<string> Authencate(LoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.Remember, true);
            if (!result.Succeeded) return null;

            var roles = _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role,string.Join(";",roles)),
                new Claim(ClaimTypes.Name,request.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<PagedResult<UserViewModel>> GetUserAllPaging(UserRequestDto request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(u => u.UserName.Contains(request.Keyword) || u.PhoneNumber.Contains(request.Keyword));
            }

            int totalitems = query.Count();
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                            .Select(i => new UserViewModel()
                            {
                                Id = i.Id,
                                FirstName = i.FirstName,
                                LastName = i.LastName,
                                UserName = i.UserName,
                                PhoneNumber = i.PhoneNumber,
                                Email = i.Email
                            })
                            .ToListAsync();
            var pageResult = new PagedResult<UserViewModel>
            {
                TotalRecords = totalitems,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                items = data
            };
            return pageResult;
        }

        public async Task<bool> Register(RegisterRequestDto request)
        {
            var user = new AppUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Dob = DateTime.Now,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return false;

            return true;
        }

        public async Task<bool> Update(Guid Id, UserViewModel request)
        {
            if (await _userManager.Users.AnyAsync(u => u.Id != request.Id && u.Email == request.Email))
                return false;
            var user = await _userManager.FindByIdAsync(Id.ToString());

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UserName = request.UserName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return true;
            return false;
        }

        public async Task<UserViewModel> GetbyId(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return null;
            }

            var result = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return result;
        }

        public async Task<bool> Delete(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return false;
            }
            var result = _userManager.DeleteAsync(user);

            return result.Result.Succeeded;
        }
    }
}