using eShopSolution.Application.Catalog.Products;
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
    public class ProductController : ControllerBase
    {
        private IPublicProductService _pulicProductService;
        public ProductController(IPublicProductService pulicProductService){
            _pulicProductService = pulicProductService;            
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var x = await _pulicProductService.GetAll();
            return Ok(x);
        }
    }
}
