using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Services.AuthAPI.Data;
using Restaurant.Services.AuthAPI.Models;
using Restaurant.Services.AuthAPI.Models.Dto;
using Restaurant.Services.AuthAPI.Service.IService;
using System.Collections.Immutable;

namespace Restaurant.Services.AuthAPI.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _db;
        protected APIResponse _apiResponse;

        public RoleService(RoleManager<ApplicationRole> roleManager, AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _apiResponse = new();
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
                        _apiResponse.Result = "Role : " + roleName + " added to " + email + " Successufully";
                        return _apiResponse;
                    }
                    else
                    {
                        _apiResponse.ErrorMessages = new List<string> { "Role " + roleName + " is not exist" };
                        return _apiResponse;
                    }
                }
                else
                {
                    _apiResponse.ErrorMessages = new List<string> { "User is not exist!" };
                    return _apiResponse;
                }
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }

            return _apiResponse;
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
                    _apiResponse.Result = roleDto;
                    return _apiResponse;
                }
                else
                {
                    _apiResponse.ErrorMessages = new List<string> { result.Errors.First().Description };
                }
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }

            return _apiResponse;
        }
        public async Task<APIResponse> RemoveRoleByName(string roleName)
        {
            try
            {
                var role = await _db.ApplicationRoles.FirstOrDefaultAsync(u => u.Name.ToLower() == roleName.ToLower());
                if (role == null)
                {
                    _apiResponse.ErrorMessages = new List<string>() { "Role " + roleName + " is not exist!" };
                    return _apiResponse;
                }

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    _apiResponse.Result = "Role has been removed successfully";
                    return _apiResponse;
                }
                else
                {
                    _apiResponse.ErrorMessages = new List<string> { result.Errors.First().Description };
                }
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }

            return _apiResponse;
        }

        public async Task<APIResponse> RemoveRoleById(string roleId)
        {
            try
            {
                var role = _db.ApplicationRoles.First(u => u.Id.ToLower() == roleId.ToLower());
                if (role == null)
                {
                    _apiResponse.ErrorMessages = new List<string>() { "Role " + roleId + " is not exist!" };
                    return _apiResponse;
                }

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    _apiResponse.Result = "Role has been removed successfully";
                    return _apiResponse;
                }
                else
                {
                    _apiResponse.ErrorMessages = new List<string> { result.Errors.First().Description };
                }
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }

            return _apiResponse;
        }

        public async Task<APIResponse> GetAllAsync()
        {
            try
            {
                _apiResponse.Result = await _db.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;
        }

        public async Task<APIResponse> GetByRoleNameAsync(string roleName)
        {
            try
            {
                _apiResponse.Result = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (_apiResponse.Result == null)
                {
                    _apiResponse.ErrorMessages = new() { "Role " + roleName + " is not exist!" };
                    return _apiResponse;
                }
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;
        }

        public async Task<APIResponse> GetByIdAsync(string id)
        {
            try
            {
                _apiResponse.Result = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id);
                if (_apiResponse.Result == null)
                {
                    _apiResponse.ErrorMessages = new() { "Role " + id + " is not exist!" };
                    return _apiResponse;
                }
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;
        }
    }
}
