namespace Restaurant.Services.ShoppingCartAPI.Models.Dto
{
    public class CreateCartDto
    {
        public CreateCartHeaderDto CartHeader { get; set; }
        public IEnumerable<CreateCartDetailsDto>? CartDetails { get; set; }
    }
}
