using eShopSolution.AdminWeb.Services.CategoryClient;
using eShopSolution.AdminWeb.Services.ProductsClient;
using eShopSolution.ViewModels.Categories;
using eShopSolution.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.OData.Query.SemanticAst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminWeb.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductClient _productClient;
        private readonly ICategoryClient _categoryClient;

        public ProductController(IProductClient productClient, ICategoryClient categoryClient)
        {
            _productClient = productClient;
            _categoryClient = categoryClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string CategoryId, string Keyword, int PageSize = 10, int PageIndex = 1)
        {
            var LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var cate = await _categoryClient.GetAll(LanguageId);
            ViewBag.Categories = cate.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            string uri = $"/api/Products?Keyword={Keyword}&pageSize={PageSize}&pageIndex={PageIndex}&LanguageId={LanguageId}&CategoryId={CategoryId}";
            var rs = await _productClient.GetAllPaging(uri);
            if (TempData["product"] != null) ViewBag.product = TempData["product"];
            return View(rs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            request.LanguageId = LanguageId;

            var rs = await _productClient.Create(request);
            if (!rs)
            {
                ModelState.AddModelError("", "Yêu cầu tạo mới thất bại");
                return View(request);
            }
            TempData["product"] = "Tạo mới sản phẩm thành công";
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var rs = await _productClient.Details(Id, LanguageId);
            return View(rs);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var rs = await _productClient.Details(Id, LanguageId);

            var request = new ProductUpdateRequest()
            {
                Id = Id,
                Name = rs.Name,
                Description = rs.Description,
                SeoAlias = rs.SeoAlias,
                SeoTitle = rs.SeoTitle,
                SeoDescription = rs.SeoDescription,
                Details = rs.Details,
                LanguageId = rs.LanguageId
            };
            return View(request);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(ProductUpdateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var rs = await _productClient.Edit(request);

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var rs = await _productClient.Details(Id, LanguageId);
            return View(rs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id, int x = 0)
        {
            var rs = await _productClient.Delete(Id);
            if (rs)
            {
                TempData["product"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index", "Product");
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> SetCategory(int Id)
        {
            var LanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var all = await _categoryClient.GetAll(LanguageId);
            var has = await _categoryClient.GetbyId(Id, LanguageId);
            var hass = has.Select(x => x.Name).ToList();
            var sele = new List<ProductCategoryDto>();
            foreach (var item in all)
            {
                sele.Add(new ProductCategoryDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Selected = hass.Contains(item.Name)
                });
            }
            var request = new CategoryModel() { categories = sele };

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> SetCategory(int Id, CategoryModel request)
        {
            var result = await _productClient.SetCategory(Id, request.categories);
            if (result) return RedirectToAction("Index", "Product");
            return BadRequest();
        }
    }
}