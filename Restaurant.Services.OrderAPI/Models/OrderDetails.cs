﻿using Restaurant.Services.OrderAPI.Models.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? Product { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }

    }
}
