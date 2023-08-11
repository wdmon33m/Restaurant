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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        protected APIResponse _response;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager,
                           RoleManager<ApplicationRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _response = new ();
        }

        public async Task<APIResponse> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u =>
                            u.Email.ToLower() == email.ToLower());
            try
            {
                if (user != null)
                {
                    if (_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                        _response.Result = "Role : " + roleName + " added to " + email + " Successufully";
                        return _response;
                    }
                    else
                    {
                        _response.ErrorMessages = new List<string> {"Role " + roleName + " is not exist" };
                        return _response;
                    }
                }
                else
                {
                    _response.ErrorMessages = new List<string> { "User is not exist!" };
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            
            return _response;
        }

        public async Task<APIResponse> CreateRole(CreateRoleDto CreateRoleDto)
        {
            ApplicationRole role = new()
            {
                Name = CreateRoleDto.Name,
                NormalizedName = CreateRoleDto.Name.ToUpper()
            };

            try
            {
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    role.Id = _db.ApplicationRoles.First(u => u.Name == role.Name).Id;

                    RoleDto roleDto = new()
                    {
                        Id = role.Id,
                        Name = role.Name,
                        NormalizedName = role.NormalizedName,
                    };
                    _response.Result = roleDto;
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
                    user.Id = _db.ApplicationUsers.First(u => u.UserName == user.UserName).Id;

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
