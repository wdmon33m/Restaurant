﻿using AutoMapper;
using Restaurant.Services.ShoppingCartAPI.Models;
using Restaurant.Services.ShoppingCartAPI.Models.Dto;

namespace Restaurant.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader,CartHeaderDto>().ReverseMap();
                config.CreateMap<CartHeader,CreateCartHeaderDto>().ReverseMap();

                config.CreateMap<CartDetails,CartDetailsDto>().ReverseMap();
                config.CreateMap<CartDetails, CreateCartDetailsDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
