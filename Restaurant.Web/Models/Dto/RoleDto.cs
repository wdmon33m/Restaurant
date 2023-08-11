namespace Restaurant.Web.Models.Dto
{
    public class RoleDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string NormalizedName { get; set; } 
    }
}
