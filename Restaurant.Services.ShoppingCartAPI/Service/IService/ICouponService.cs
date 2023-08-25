using Restaurant.Services.ShoppingCartAPI.Models.Dto;

namespace Restaurant.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponAsync(string couponCode);
    }
}
