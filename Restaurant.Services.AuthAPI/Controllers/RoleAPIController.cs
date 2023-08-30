using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Services.AuthAPI.Models;
using Restaurant.Services.AuthAPI.Models.Dto;
using Restaurant.Services.AuthAPI.Service.IService;
using System.Net;

namespace Restaurant.Services.AuthAPI.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleAPIController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleAPIController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {
            var _response = await _roleService.GetAllAsync();
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("GetByName/{roleName}")]
        public async Task<ActionResult<APIResponse>> GetByName(string roleName)
        {
            var _response = await _roleService.GetByRoleNameAsync(roleName);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse>> GetById(string id)
        {
            var _response = await _roleService.GetByIdAsync(id);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }
        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateRole([FromBody] CreateRoleDto model)
        {
            var _response = await _roleService.CreateRole(model);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            _response.StatusCode = HttpStatusCode.Created;
            return _response;
        }
        [HttpPost("assign")]
        public async Task<ActionResult<APIResponse>> AssignRole([FromBody] AssignRoleDto model)
        {

            var _response = await _roleService.AssignRole(model);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpDelete("romovebyname/{roleName}")]
        public async Task<ActionResult<APIResponse>> RemoveRoleByName(string roleName)
        {
            var _response = await _roleService.RemoveRoleAsync(roleName,false);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            _response.StatusCode = HttpStatusCode.NoContent;
            return _response;
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> RemoveRoleByID(string id)
        {
            var _response = await _roleService.RemoveRoleAsync(id);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            _response.StatusCode = HttpStatusCode.NoContent;
            return _response;
        }
    }
}
