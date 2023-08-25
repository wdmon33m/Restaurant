using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;
using Restaurant.Web.Service.IService;
using Restaurant.Web.Utility;

namespace Restaurant.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        private const string cartApiUrl = "/api/cart/";

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        
        public async Task<ResponseDto?> GetCartByUserId(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartApiBase + cartApiUrl + "GetCart/" + userId
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartApiBase + cartApiUrl + "CartUpsert"
            });
        }

        public async Task<ResponseDto?> RemoveCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                Url = SD.ShoppingCartApiBase + cartApiUrl + "RemoveCart"
            });
        }

        public async Task<ResponseDto?> ApplyCoupon(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartApiBase + cartApiUrl + "ApplyCoupon"
            });
        }
    }
}
