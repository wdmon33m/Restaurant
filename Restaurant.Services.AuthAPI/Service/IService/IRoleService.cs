using Microsoft.AspNetCore.Identity;
using Restaurant.Services.AuthAPI.Models;
using Restaurant.Services.AuthAPI.Models.Dto;

namespace Restaurant.Services.AuthAPI.Service.IService
{
    public interface IRoleService
    {
        Task<APIResponse> GetAllAsync();
        Task<APIResponse> GetByRoleNameAsync(string roleName);
        Task<APIResponse> GetByIdAsync(string id);
        Task<APIResponse> AssignRole(string email, string roleName);
        Task<APIResponse> CreateRole(CreateRoleDto roleDto);
        Task<APIResponse> RemoveRoleByName(string roleName);
        Task<APIResponse> RemoveRoleById(string roleId);
    }
}
