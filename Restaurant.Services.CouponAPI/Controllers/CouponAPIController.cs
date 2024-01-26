using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Restaurant.Services.CouponAPI.Data;
using Restaurant.Services.CouponAPI.Models;
using Restaurant.Services.CouponAPI.Models.Dto;

namespace Restaurant.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }
        
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> list = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(list);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.CouponID == id);
                if (coupon == null)
                {
                    _response.ErrorMessages = new() { "No coupon found" };
                }
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{couponCode}")]
        public ResponseDto GetbyCouponCode(string couponCode)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.CouponCode.ToLower() == couponCode.ToLower());
                if (coupon == null)
                {
                    _response.ErrorMessages = new() { "No coupon found" };
                }
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _db.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == couponDto.CouponCode.ToLower());
                if (coupon != null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Coupon code is already exist!" };
                    return _response;
                }
                if (couponDto.DiscountAmount > couponDto.MinAmount)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Minimum Amount less than Discount Amount" };
                } 
                else
                {
                    couponDto.CouponCode = couponDto.CouponCode.ToUpper();
                    coupon = _mapper.Map<Coupon>(couponDto);
                    _db.Coupons.Add(coupon);
                    _db.SaveChanges();

                    var options = new Stripe.CouponCreateOptions
                    {
                        AmountOff = (long)(couponDto.DiscountAmount * 100),
                        Name = couponDto.CouponCode,
                        Currency = "usd",
                        Id = couponDto.CouponCode
                    };
                    var service = new Stripe.CouponService();
                    service.Create(options);

                    _response.Result = couponDto;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                couponDto.CouponCode = couponDto.CouponCode.ToUpper();
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(coupon);
                _db.SaveChanges();
                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.CouponID == id);
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();


                var service = new Stripe.CouponService();
                service.DeleteAsync(coupon.CouponCode);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }
    }
}
