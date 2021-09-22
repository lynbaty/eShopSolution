using AutoMapper;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Products;
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
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        private IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var x = await _productService.GetAllPaging(request);
            return Ok(x);
        }

        // /product/public-paging
        [HttpGet("/Category")]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery] GetPublicProductPagingRequest request)
        {
            var x = await _productService.GetAllByCategoryId(request);
            return Ok(x);
        }

        [HttpGet("/{productId}/{languageId}")]
        public async Task<IActionResult> GetbyId([FromRoute] int productId, [FromRoute] string languageId)
        {
            var result = await _productService.GetbyId(productId, languageId);

            await _productService.UpdateViewCount(productId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _productService.Create(request);

            var product = await _productService.GetbyId(result, request.LanguageId);

            return CreatedAtAction(nameof(GetbyId), new { productId = result, languageId = request.LanguageId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _productService.Update(request);

            var product = await _productService.GetbyId(result, request.LanguageId);

            return CreatedAtAction(nameof(GetbyId), new { productId = result, languageId = request.LanguageId }, product);
        }

        [HttpDelete("/{productId}")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            var result = await _productService.Delete(productId);

            return Ok($"Đã xóa {result} tham chiếu trong Database");
        }

        [HttpPatch("/{productId}/price")]
        public async Task<IActionResult> UpdatePrice([FromRoute] int productId, [FromBody] decimal price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _productService.UpdatePrice(productId, price);

            if (result) return Ok("Đã Update");
            return BadRequest();
        }

        [HttpPatch("/{productId}/stock")]
        public async Task<IActionResult> UpdateStock([FromRoute] int productId, [FromBody] int Quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _productService.UpdateStock(productId, Quantity);

            if (result) return Ok("Đã Update");
            return BadRequest();
        }

        [HttpGet("/{productId}/images")]
        public async Task<IActionResult> GetListImage([FromRoute] int productId)
        {
            List<ProductImage> x = await _productService.GetListImage(productId);

            if (x.Count == 0)
            {
                return BadRequest();
            }
            List<ProductImageViewModel> result = x.Select(m => _mapper.Map<ProductImageViewModel>(m)).ToList();

            return Ok(result);
        }

        [HttpPost("/{productId}/images")]
        public async Task<IActionResult> AddImages([FromRoute] int productId, [FromBody] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var imageid = await _productService.AddImage(productId, request);

            var x = await _productService.GetImageById(imageid);

            if (x != null)
            {
                ProductImageViewModel result = _mapper.Map<ProductImageViewModel>(x);
                return CreatedAtAction(nameof(GetImageById), new { imageId = imageid, productId = productId }, result);
            }
            return Ok(imageid);
        }

        [HttpGet("/{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById([FromRoute] int imageId)
        {
            var x = await _productService.GetImageById(imageId);
            if (x == null)
            {
                return BadRequest();
            }
            ProductImageViewModel result = _mapper.Map<ProductImageViewModel>(x);

            return Ok(result);
        }

        [HttpDelete("/{productId}/image/{imageId}")]
        public async Task<IActionResult> Remove([FromRoute] int imageId)
        {
            var result = await _productService.Remove(imageId);

            return Ok($"Đã xóa {result} tham chiếu trong Database");
        }

        [HttpPatch("/{productId}/image/{imageId}")]
        public async Task<IActionResult> UpdateImage([FromRoute] int productId, [FromRoute] int imageId, [FromBody] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _productService.UpdateImage(imageId, request);

            return CreatedAtAction(nameof(GetImageById), new { imageId = result, productId = productId }, result);
        }
    }
}