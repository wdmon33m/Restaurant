using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Services.AuthAPI.Models;
using Restaurant.Services.AuthAPI.Models.Dto;
using Restaurant.Services.AuthAPI.Service.IService;
using System.Net;

namespace Restaurant.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected APIResponse _response;


        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> Register([FromBody] RegistrationRequestDto model)
        {
            _response = await _authService.Register(model);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }

            _response.StatusCode = HttpStatusCode.Created;
            return _response;
        }


        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDto model)
        {
            _response = await _authService.Login(model);
            if (_response.ErrorMessages != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost("role")]
        public async Task<ActionResult<APIResponse>> CreateRole([FromBody] CreateRoleDto model)
        {
            _response = await _authService.CreateRole(model);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            _response.StatusCode = HttpStatusCode.Created;
            return _response;
        }
        [HttpPost("AssignRole")]
        public async Task<ActionResult<APIResponse>> AssignRole([FromBody] AssignRoleDto model)
        {
            _response = await _authService.AssignRole(model.Email,model.RoleName);
            if (!_response.ErrorMessages.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
    

}
