using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;

namespace Restaurant.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductAsync(int id);
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
