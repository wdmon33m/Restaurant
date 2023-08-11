using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Services.CouponAPI.Data;
using Restaurant.Services.CouponAPI.Models;
using Restaurant.Services.CouponAPI.Models.Dto;

namespace Restaurant.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
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
                    coupon = _mapper.Map<Coupon>(couponDto);
                    _db.Coupons.Add(coupon);
                    _db.SaveChanges();
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
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
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
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.CouponID == id);
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();
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
