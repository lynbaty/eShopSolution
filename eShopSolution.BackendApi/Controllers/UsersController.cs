using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.Products;
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
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var resultToken = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(resultToken)) return BadRequest("incorrect");
            return Ok(resultToken);
        }

        [HttpPost()]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _userService.Register(request);
            if (!result) return BadRequest("Register is unsuccessful");
            return Ok();
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllPaging([FromQuery] UserRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _userService.GetUserAllPaging(request);
            return Ok(result);
        }

        //http://localhost/api/users/{id} PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _userService.Update(id, request);
            if (!result) return BadRequest();
            return Ok();
        }

        //localhost/api/users/{id} GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId([FromRoute] Guid id)
        {
            var result = await _userService.GetbyId(id);
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _userService.Delete(id);
            if (!result) return BadRequest();
            return Ok(result);
        }
    }
}