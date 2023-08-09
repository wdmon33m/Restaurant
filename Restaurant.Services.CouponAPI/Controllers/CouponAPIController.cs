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

        public CouponAPIController(AppDbContext db)
        {
            _db = db;
            _response = new();
        }
        
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> list = _db.Coupons.ToList();
                _response.Result = list;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
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
                _response.Result = coupon;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
