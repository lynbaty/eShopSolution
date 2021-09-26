using AutoMapper;
using eShopSolution.Application.System.Roles;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;

        public RolesController(IRolesService rolesService, IMapper mapper)
        {
            _rolesService = rolesService;
            _mapper = mapper;
        }

        //api/roles
        [HttpGet]
        public IActionResult GetAllPaging([FromQuery] RoleGetAllDto request)
        {
            var call = _rolesService.GetAllPaging(request);
            if (call.Result == null) return BadRequest(call.Messenger);

            var x = call.Result.items.Select(i => _mapper.Map<RoleViewDto>(i)).ToList();
            var result = new PagedResult<RoleViewDto>()
            {
                items = x,
                PageIndex = call.Result.PageIndex,
                PageSize = call.Result.PageSize,
                TotalRecords = call.Result.TotalRecords
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto request)
        {
            var call = await _rolesService.Create(request);
            if (!call.Result) return BadRequest(call.Messenger);

            return Ok();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetbyName(string name)
        {
            var call = await _rolesService.GetbyName(name);
            if (call.Result == null) return BadRequest(call.Messenger);

            var result = _mapper.Map<RoleViewDto>(call.Result);
            return Ok(result);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var call = await _rolesService.Delete(name);
            if (!call.Result) return BadRequest(call.Messenger);

            return Ok();
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Edit(string name, RoleEditDto request)
        {
            var call = await _rolesService.Edit(name, request);
            if (!call.Result) return BadRequest(call.Messenger);

            return Ok();
        }
    }
}