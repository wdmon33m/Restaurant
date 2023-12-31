﻿using Restaurant.Web.Models;
using Restaurant.Web.Models.Dto;

namespace Restaurant.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrderAsync(CartDto cartDto);
        Task<ResponseDto?> CreateStripeSessionAsync(StripeRequestDto stripeRequestDto);
        Task<ResponseDto?> ValidateStripeSession(int orderHeaderId);
        Task<ResponseDto?> GetAllOrdersAsync(string? userId);
        Task<ResponseDto?> GetOrderAsync(int orderId);
        Task<ResponseDto?> UpdateOrderStatusAsync(int orderId,string newStatus);
    }
}
