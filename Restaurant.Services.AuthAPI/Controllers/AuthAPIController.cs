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
            if (model == null)
            {
                _response.ErrorMessages = new() { "Please enter user name and passowrd" };
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }

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
    }
}
