using Restaurant.Services.OrderAPI.Models.Dto;

namespace Restaurant.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductAsync(int productID);
    }
}
