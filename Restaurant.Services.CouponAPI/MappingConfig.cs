using AutoMapper;
using Restaurant.Services.CouponAPI.Models;
using Restaurant.Services.CouponAPI.Models.Dto;

namespace Restaurant.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegesterMaps()
        {
            var mapingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon,CouponDto>().ReverseMap();
            });
            return mapingConfig;
        }
    }
}
