using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;

namespace Restaurant.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
    }
}
