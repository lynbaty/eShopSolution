using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromForm] LoginRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var resultToken = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(resultToken)) return BadRequest("Username or password incorrect");
            return Ok(new { token = resultToken });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _userService.Register(request);
            if (!result) return BadRequest("Register is unsuccessful");
            return Ok();
        }
    }
}