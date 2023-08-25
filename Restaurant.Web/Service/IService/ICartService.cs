using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;

namespace Restaurant.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserId(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCoupon(CartDto cartDto);
    }
}
