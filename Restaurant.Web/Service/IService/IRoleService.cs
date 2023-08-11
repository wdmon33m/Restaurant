using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;

namespace Restaurant.Web.Service.IService
{
    public interface IRoleService
    {
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetByRoleNameAsync(string roleName);
        Task<ResponseDto?> GetAsync(string id);
        Task<ResponseDto?> AssignRoleAsync(AssignRoleDto assignRoleDto);
        Task<ResponseDto?> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<ResponseDto?> RemoveRoleAsync(string id);
        Task<ResponseDto?> RemoveByRoleNameAsync(string roleName);
    }
}
