using Microsoft.AspNetCore.Identity;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u =>
                            u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || !isPasswordValid)
            {
                return new LoginResponseDto() { User = null, Token = ""};
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
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
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
                    user.Id = _db.ApplicationUsers.First(u => u.UserName == user.UserName).Id;

                    UserDto userDto = new() 
                    {
                        Email = user.Email,
                        ID = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Error Encountered";
        }
    }
}
