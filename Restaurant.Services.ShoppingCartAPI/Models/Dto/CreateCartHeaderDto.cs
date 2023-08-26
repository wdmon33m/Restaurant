namespace Restaurant.Services.ShoppingCartAPI.Models.Dto
{
    public class CreateCartHeaderDto
    {
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
