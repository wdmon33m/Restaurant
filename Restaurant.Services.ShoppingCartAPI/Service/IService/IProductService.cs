using Restaurant.Services.ShoppingCartAPI.Models.Dto;

namespace Restaurant.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductAsync(int productID);
    }
}
