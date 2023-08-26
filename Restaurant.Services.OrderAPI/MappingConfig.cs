using AutoMapper;
using Restaurant.Services.OrderAPI.Models;
using Restaurant.Services.OrderAPI.Models.Dto;

namespace Restaurant.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeader,OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetails,OrderDetailsDto>().ReverseMap();

                config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal))
                .ReverseMap();

                config.CreateMap<OrderDetailsDto, CartDetailsDto>();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

            });
            return mappingConfig;
        }
    }
}
