using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Services.AuthAPI.Data;
using Restaurant.Services.AuthAPI.Models;
using Restaurant.Services.AuthAPI.Models.Dto;
using Restaurant.Services.AuthAPI.Service.IService;

namespace Restaurant.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        protected APIResponse _response;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _response = new ();
        }
        public async Task<APIResponse> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u =>
                            u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || !isPasswordValid)
            {
                _response.ErrorMessages = new List<string>() { "Username or password is incorrect" };
                return _response;
            }

            //if user was found, Generate GWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            _response.Result = loginResponseDto;
            return _response;
        }

        public async Task<APIResponse> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user,registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    user.Id = _db.ApplicationUsers.First(u => u.UserName.ToLower() == user.UserName.ToLower()).Id;

                    UserDto userDto = new() 
                    {
                        Email = user.Email,
                        ID = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber
                    };
                    _response.Result = userDto;
                    return _response;
                }
                else
                {
                    _response.ErrorMessages = new List<string> { result.Errors.First().Description };
                }
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
    }
}
