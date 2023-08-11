using Microsoft.AspNetCore.Identity;

namespace Restaurant.Services.AuthAPI.Models
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsActive { get; set; } = true;
    }
}
