using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Services.ShoppingCartAPI.Data;
using Restaurant.Services.ShoppingCartAPI.Models;
using Restaurant.Services.ShoppingCartAPI.Models.Dto;
using Restaurant.Services.ShoppingCartAPI.Service.IService;
using System.Reflection.PortableExecutable;

namespace Restaurant.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        public CartAPIController(IMapper mapper, AppDbContext db, IProductService productService, ICouponService couponService)
        {
            _response = new();
            _mapper = mapper;
            _db = db;
            _productService = productService;
            _couponService = couponService;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CreateCartDto cartDto)
        {
            try
            {
                int productId = cartDto.CartDetails.FirstOrDefault().ProductId;
                var product = await _productService.GetProductAsync(productId);
                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new() { "The Product is not exist!" };
                    return _response;
                }

                var couponCode = cartDto.CartHeader.CouponCode;
                var coupon = await _couponService.GetCouponAsync(couponCode);
                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new() { "The coupon is not exist!" };
                    return _response;
                }

                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    cartDetails.CartHeaderId = cartHeader.CartHeaderId;
                    await _db.CartDetails.AddAsync(cartDetails);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if detail has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails

                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                        cartDetails.CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        await _db.CartDetails.AddAsync(cartDetails);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cartDetails
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;

                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                        cartDetails.CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDetails.CartDetailsId = cartDetailsFromDb.CartDetailsId;

                        _db.CartDetails.Update(cartDetails);
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }


        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(u => u.CartDetailsId == cartDetailsId);
                int totalCountOfCartItems = _db.CartDetails.Count(u => u.CartHeaderId == cartDetails.CartHeaderId);
                _db.CartDetails.Remove(cartDetails);

                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _db.SaveChangesAsync();
                _response.Result = "Cart has been removed successuflly";
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId))
                };
                if (cart.CartHeader == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new() { "No items was found" };
                    return _response;
                }
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(
                    _db.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> productDtos = await _productService.GetAllProductsAsync();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                //apply coupon if any
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCouponAsync(cart.CartHeader.CouponCode);
                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discound = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CreateCartHeaderDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.UserId);
                cartHeaderFromDb.CouponCode = cartDto.CouponCode.ToUpper();

                var coupon = await _couponService.GetCouponAsync(cartHeaderFromDb.CouponCode);
                if (coupon.CouponCode != null)
                {
                    _db.CartHeaders.Update(cartHeaderFromDb);
                    await _db.SaveChangesAsync();

                    _response.Result = "Coupon has been applied successfully";
                    return _response;
                }
                _response.ErrorMessages = new() { "Coupon is not exist!" };
                _response.IsSuccess = false;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpDelete("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CreateCartHeaderDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.UserId);
                cartHeaderFromDb.CouponCode = "";
                _db.CartHeaders.Update(cartHeaderFromDb);
                await _db.SaveChangesAsync();

                _response.Result = "Coupon has been removed successfully";
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
