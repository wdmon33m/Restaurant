using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;
using Restaurant.Web.Service.IService;
using Restaurant.Web.Utility;

namespace Restaurant.Web.Service
{
    public class RoleService : IRoleService
    {
        private readonly IBaseService _baseService;
        private const string authApiUrl = "/api/role/";

        public RoleService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(AssignRoleDto assignRoleDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = assignRoleDto,
                Url = SD.AuthApiBase + authApiUrl + "assign"
            });
        }

        public async Task<ResponseDto?> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = createRoleDto,
                Url = SD.AuthApiBase + authApiUrl
            });
        }
        public async Task<ResponseDto?> RemoveRoleAsync(RemoveRoleDto removeRoleDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Data = removeRoleDto,
                Url = SD.AuthApiBase + authApiUrl
            });
        }

        public async Task<ResponseDto?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthApiBase + authApiUrl
            });
        }

        public async Task<ResponseDto?> GetByRoleNameAsync(string roleName)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthApiBase + authApiUrl + "GetByName/" + roleName
            });
        }

        public async Task<ResponseDto?> GetAsync(string id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthApiBase + authApiUrl + id
            });
        }

        public async Task<ResponseDto?> RemoveRoleAsync(string id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.AuthApiBase + authApiUrl + id
            });
        }

        public async Task<ResponseDto?> RemoveByRoleNameAsync(string roleName)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthApiBase + authApiUrl + "romovebyname/" + roleName
            });
        }
    }
}
