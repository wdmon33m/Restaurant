using Newtonsoft.Json;
using Restaurant.Services.ShoppingCartAPI.Models.Dto;
using Restaurant.Services.ShoppingCartAPI.Service.IService;

namespace Restaurant.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDto>(); 
        }

        public async Task<ProductDto> GetProductAsync(int productID)
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product/" + productID);
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));
            }
            return new ProductDto();
        }
    }
}
