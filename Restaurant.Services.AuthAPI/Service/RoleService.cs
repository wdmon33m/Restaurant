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

        public async Task<APIResponse> AssignRole(AssignRoleDto assignRoleDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u =>
                            u.Email.ToLower() == assignRoleDto.Email.ToLower());
            try
            {
                if (user != null)
                {
                    if (_roleManager.RoleExistsAsync(assignRoleDto.RoleName).GetAwaiter().GetResult())
                    {
                        var result = await _userManager.AddToRoleAsync(user, assignRoleDto.RoleName);
                        if (result != null && result.Succeeded)
                        {
                            _apiResponse.Result = "Role : " + assignRoleDto.RoleName + " added to " + assignRoleDto.Email + " Successufully";
                            return _apiResponse;
                        }
                        else
                        {
                            _apiResponse.ErrorMessages = new List<string> { result.Errors.First().Description };
                        }
                    }
                    else
                    {
                        _apiResponse.ErrorMessages = new List<string> { "Role " + assignRoleDto.RoleName + " is not exist" };
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

        public async Task<APIResponse> RemoveRoleAsync(string filter, bool isRemoveById = true)
        {
            try
            {
                ApplicationRole role;

                
                if (isRemoveById)
                {
                    role = _db.ApplicationRoles.First(u => u.Id.ToLower() == filter.ToLower());
                }
                else
                {
                    role = _db.ApplicationRoles.First(u => u.Name.ToLower() == filter.ToLower());
                }

                if (role == null)
                {
                    _apiResponse.ErrorMessages = new List<string>() { "Role " + filter + " is not exist!" };
                    return _apiResponse;
                }

                if (role.Name.Equals("ADMIN") || role.Name.Equals("CUSTOMER"))
                {
                    _apiResponse.ErrorMessages = new() { "You can not remove this role" };
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
