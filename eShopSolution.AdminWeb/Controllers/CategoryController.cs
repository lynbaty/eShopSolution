using eShopSolution.AdminWeb.Services.CategoryClient;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModels.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryClient _categoryClient;

        public CategoryController(ICategoryClient categoryClient)
        {
            _categoryClient = categoryClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var result = await _categoryClient.GetAll(LanguageId);

            if (result == null) return BadRequest();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            string LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var result = await _categoryClient.Get(Id, LanguageId);

            if (result == null) return BadRequest();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            string LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var result = await _categoryClient.Get(Id, LanguageId);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id, int x = 0)
        {
            var result = await _categoryClient.Delete(Id);
            if (result) TempData["Category"] = "Đã xóa thành công danh mục";
            return RedirectToAction("Index", "Category");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto request)
        {
            if (!ModelState.IsValid) return View(request);
            string LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            request.LanguageId = LanguageId;
            var result = await _categoryClient.Create(request);
            if (!result)
            {
                ModelState.AddModelError("", "Lỗi tạo danh mục");
                return View(request);
            }
            TempData["Category"] = $"Đã tạo danh mục {request.Name} thành công";
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var values = from Status e in Enum.GetValues(typeof(Status))
                         select new
                         {
                             Value = Convert.ToInt32(e),
                             Text = e.ToString()
                         };
            ViewBag.Enum = new SelectList(values, "Value", "Text");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            string LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var result = await _categoryClient.Get(Id, LanguageId);
            var request = new CategoryEditDto()
            {
                Name = result.Name,
                Status = result.Status,
                SortOrder = result.SortOrder,
                LanguageId = LanguageId
            };
            var values = from Status e in Enum.GetValues(typeof(Status))
                         select new
                         {
                             Value = Convert.ToInt32(e),
                             Text = e.ToString()
                         };
            ViewBag.EnumEdit = new SelectList(values, "Value", "Text", result.Status);
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, CategoryEditDto request)
        {
            if (!ModelState.IsValid) return View(request);
            var result = await _categoryClient.Edit(Id, request);
            if (!result)
            {
                ModelState.AddModelError("", "Thay đổi thông tin danh mục không thành công");
                return View();
            }
            TempData["Category"] = $"Đã thay đổi danh mục {request.Name} thành công";
            return RedirectToAction("Index", "Category");
        }
    }
}