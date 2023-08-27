using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;
using Restaurant.Web.Service.IService;
using Restaurant.Web.Utility;

namespace Restaurant.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.RemoveCartAsync(cartDetailsId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response?.ErrorMessages.First();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response?.ErrorMessages.First();
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto? response = await _cartService.RemoveCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon removed successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response?.ErrorMessages.First();
            }

            return View();
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }
        [Authorize]
        public async Task<IActionResult> Confirmation(int orderId)
        {
            ResponseDto? response = await _orderService.ValidateStripeSession(orderId);
            if (response != null && response.IsSuccess)
            {
                OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
                if (orderHeaderDto.Status == SD.Status_Approved)
                {
                    return View(orderId);
                }
            }
            //redirect to error page based on status
            return View(orderId);
        }
        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("CheckOut")]
        public async Task<IActionResult> CheckOut(CartDto cartDto)
        {
            CartDto updatedCart = new();
            updatedCart = await LoadCartDtoBasedOnLoggedInUser();
            updatedCart.CartHeader.Phone = cartDto.CartHeader?.Phone;
            updatedCart.CartHeader.Email = cartDto.CartHeader?.Email;
            updatedCart.CartHeader.FirstName = cartDto.CartHeader?.FirstName;
            updatedCart.CartHeader.LastName = cartDto.CartHeader?.LastName;

            var response = await _orderService.CreateOrderAsync(updatedCart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if (response != null && response.IsSuccess)
            {
                // get stripe session and redirect to stripe to place order
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CancelUrl = domain + "cart/checkout",
                    OrderHeader = orderHeaderDto
                };

                var stripeResponse = await _orderService.CreateStripeSessionAsync(stripeRequestDto);
                StripeRequestDto stripeRequestDtoResult = JsonConvert.DeserializeObject<StripeRequestDto>
                                                          (Convert.ToString(stripeResponse.Result));

                Response.Headers.Add("Location", stripeRequestDtoResult.StripeSessionUrl);
                return new StatusCodeResult(303);
            }
            return View(updatedCart);
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
             var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCartByUserId(userId);

            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            else
            {
                TempData["error"] = response?.ErrorMessages.First();
            }
            return new CartDto();
        }
    }
}
