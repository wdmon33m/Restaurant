using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;
using Restaurant.Web.Service;
using Restaurant.Web.Service.IService;

namespace Restaurant.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> RoleIndex()
        {
            List<RoleDto>? list = new();
            ResponseDto? response = await _roleService.GetAllAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RoleDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.ErrorMessages;
            }

            return View(list);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDto obj)
        {
            if (ModelState.IsValid)
            {
                ResponseDto result = await _roleService.CreateRoleAsync(obj);
                if (result != null && result.IsSuccess)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(RoleIndex));
                }
                TempData["error"] = result.ErrorMessages.First();
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(RoleDto roleDto)
        {
            ResponseDto? response = await _roleService.RemoveRoleAsync(roleDto.Id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Role deleted successfully";
                return RedirectToAction(nameof(RoleIndex));
            }
            else
            {
                TempData["error"] = response?.ErrorMessages.First();
                return RedirectToAction(nameof(RoleIndex));
            }

            return View(roleDto);
        }
        [HttpGet]
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            ResponseDto? response = await _roleService.GetAsync(roleId);

            if (response != null && response.IsSuccess)
            {
                RoleDto? roleDto = JsonConvert.DeserializeObject<RoleDto>(Convert.ToString(response.Result));
                return View(roleDto);
            }
            else
            {
                TempData["error"] = response?.ErrorMessages;
            }
            return NotFound();
        }
    }
}
