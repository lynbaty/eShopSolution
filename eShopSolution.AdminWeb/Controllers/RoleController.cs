using eShopSolution.AdminWeb.Services.RolesClient;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRolesClient _roleClient;

        public RoleController(IRolesClient roleClient)
        {
            _roleClient = roleClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int PageIndex = 1, int PageSize = 2)
        {
            var request = new RoleGetAllDto()
            {
                Keyword = keyword,
                pageIndex = PageIndex,
                pageSize = PageSize,
            };
            var result = await _roleClient.GetAllPaging(request);
            if (TempData["role"] != null) ViewBag.Role = TempData["role"];
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            await _roleClient.Create(request);
            TempData["role"] = $"Thêm role {request.Name} thành công";
            return RedirectToAction("Index", "Role");
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Details(string name)
        {
            var result = await _roleClient.GetbyId(name);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string name)
        {
            var result = await _roleClient.GetbyId(name);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name, int a = 0)
        {
            await _roleClient.Delete(name);
            TempData["role"] = $"Xóa role {name} thành công";
            return RedirectToAction("Index", "Role");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string name)
        {
            var result = await _roleClient.GetbyId(name);
            var x = new RoleEditDto()
            {
                Name = result.Name,
                Description = result.Description
            };
            return View(x);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditDto request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _roleClient.Edit(request.Name, request);
            if (!result.Result)
            {
                ModelState.AddModelError("", result.Messenger);
                return View(request);
            }
            TempData["role"] = $"Cập nhập role {request.Name} thành công";
            return RedirectToAction("Index", "Role");
        }
    }
}