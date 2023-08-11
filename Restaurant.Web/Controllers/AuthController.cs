using Microsoft.AspNetCore.Mvc;
using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;
using Restaurant.Web.Service.IService;

namespace Restaurant.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IRoleService _roleService;

        public AuthController(IAuthService authService, IRoleService roleService)
        {
            _authService = authService;
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            if (ModelState.IsValid)
            {
                ResponseDto result = await _authService.LoginAsync(obj);
                if (result != null && result.IsSuccess)
                {
                    _roleService.AssignRoleAsync(new AssignRoleDto() { RoleName = "CUSTOMER" });
                    TempData["success"] = "User created successfully";
                    return RedirectToAction(nameof(Login));
                }
                TempData["error"] = result.ErrorMessages.First();
            }

            return View(obj);
        }




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            if (ModelState.IsValid)
            {
                ResponseDto result = await _authService.RegisterAsync(obj);
                if (result != null && result.IsSuccess)
                {
                    _roleService.AssignRoleAsync(new AssignRoleDto() { RoleName = "CUSTOMER" });
                    TempData["success"] = "User created successfully";
                    return RedirectToAction(nameof(Login));
                }
                TempData["error"] = result.ErrorMessages.First();
            }

            return View(obj);
        }
        //public IActionResult SignInUser(LoginResponseDto model)
        //{
        //    //var handler = new JwtSecurityTokenHandler();
        //}
        
        public IActionResult Logout()
        {
            return View();
        }
    }
}
