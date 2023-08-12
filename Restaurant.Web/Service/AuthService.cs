using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;
using Restaurant.Web.Service.IService;
using Restaurant.Web.Utility;

namespace Restaurant.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        private const string authApiUrl = "/api/auth/";

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        
        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthApiBase + authApiUrl + "login"
            },withBearer:false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthApiBase + authApiUrl + "register"
            }, withBearer: false);
        }
    }
}
