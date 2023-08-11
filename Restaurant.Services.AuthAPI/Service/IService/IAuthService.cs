using Restaurant.Services.AuthAPI.Models;
using Restaurant.Services.AuthAPI.Models.Dto;

namespace Restaurant.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<APIResponse> Register(RegistrationRequestDto registrationRequestDto);
        Task<APIResponse> Login(LoginRequestDto loginRequestDto);
        
    }
}
