using eShopSolution.Application.Catalog.Categories;
using eShopSolution.ViewModels.Categories;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryApi _categoryApi;

        public CategoriesController(ICategoryApi categoryApi)
        {
            _categoryApi = categoryApi;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string LanguageId)
        {
            var rs = await _categoryApi.GetAll(LanguageId);
            return Ok(rs);
        }

        [HttpGet("{Id}/{LanguageId}")]
        public async Task<IActionResult> GetbyId([FromRoute] int Id, string LanguageId)
        {
            var rs = await _categoryApi.GetbyId(Id, LanguageId);
            return Ok(rs);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var rs = await _categoryApi.Delete(Id);
            if (rs) return Ok(rs);
            return BadRequest();
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Details(int Id, string LanguageId)
        {
            var rs = await _categoryApi.Details(Id, LanguageId);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto request)
        {
            var rs = await _categoryApi.Create(request);
            if (rs) return Ok(rs);
            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Edit(int Id, CategoryEditDto request)
        {
            var rs = await _categoryApi.Edit(Id, request);
            if (rs) return Ok(rs);
            return BadRequest();
        }
    }
}