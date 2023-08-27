using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;
using Restaurant.Web.Service.IService;
using Restaurant.Web.Utility;

namespace Restaurant.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
        private const string cartApiUrl = "/api/order/";

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateOrderAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.OrderApiBase + cartApiUrl + "CreateOrder"
            });
        }

        public async Task<ResponseDto?> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrderApiBase + cartApiUrl + "CreateStripeSession"
            });
        }

        public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = orderHeaderId,
                Url = SD.OrderApiBase + cartApiUrl + "ValidateStripeSession"
            });
        }

        public async Task<ResponseDto?> GetAllOrdersAsync(string? userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderApiBase + cartApiUrl + "GetOrders/" + userId
            });
        }

        public async Task<ResponseDto?> GetOrderAsync(int orderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderApiBase + cartApiUrl + "GetOrder/" + orderId
            });
        }

        public async Task<ResponseDto?> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PATCH,
                Data = newStatus,
                Url = SD.OrderApiBase + cartApiUrl + "UpdateOrderStatus/" + orderId
            });
        }
    }
}
