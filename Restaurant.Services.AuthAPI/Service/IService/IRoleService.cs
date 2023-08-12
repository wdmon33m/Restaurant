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
        Task<APIResponse> AssignRole(AssignRoleDto assignRoleDto);
        Task<APIResponse> CreateRole(CreateRoleDto roleDto);
        Task<APIResponse> RemoveRoleAsync(string filter, bool isRemoveById = true);
    }
}
