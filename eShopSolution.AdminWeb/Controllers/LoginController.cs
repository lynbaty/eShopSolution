using eShopSolution.AdminWeb.Services;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserClient _userClient;
        private readonly IConfiguration _configuration;

        public LoginController(IUserClient userClient, IConfiguration configuration)
        {
            _userClient = userClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return View();
            var token = await _userClient.Authencate(request);
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Lỗi get Tokken");
                return View();
            }
            if (token == "incorrect")
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
                return View();
            }
            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("DefaultLanguageId", _configuration["DefaultLanguageId"]);
            var userPrincipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                userPrincipal,
                                authProperties
                                );

            return RedirectToAction("Index", "User");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true; //Check cho nay

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}