using eShopSolution.AdminWeb.Services;
using eShopSolution.ViewModels.Products;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserClient _userClient;
        private readonly IConfiguration _configuration;

        public UserController(IUserClient userClient, IConfiguration configuration)
        {
            _userClient = userClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string keyword, int pageSize = 5, int pageIndex = 1)
        {
            var request = new UserRequestDto()
            {
                Keyword = keyword,
                pageIndex = pageIndex,
                pageSize = pageSize,
            };

            var result = await _userClient.GetAllPaging(request);
            if (TempData["success"] != null) ViewBag.successalert = TempData["success"];
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            if (!ModelState.IsValid) return View();

            var rs = await _userClient.Register(request);
            if (rs) return View(request);
            TempData["success"] = "Đăng kí tài khoản mới thành công";
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var rs = await _userClient.GetbyId(Id);
            if (rs == null) return BadRequest();
            return View(rs);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, UserViewModel request)
        {
            var rs = await _userClient.Update(Id, request);
            if (!rs) return View(request);
            TempData["success"] = "Cập nhập thông tin thành công";
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var rs = await _userClient.GetbyId(Id);
            if (rs == null) return BadRequest();
            return View(rs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserViewModel request)
        {
            var rs = await _userClient.Delete(request.Id);
            if (!rs) return BadRequest();
            TempData["success"] = "Xóa tài khoản người dùng thành công";
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var rs = await _userClient.GetbyId(Id);
            if (rs == null) return BadRequest();
            return View(rs);
        }
    }
}