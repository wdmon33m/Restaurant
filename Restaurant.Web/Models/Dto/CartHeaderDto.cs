﻿using System.ComponentModel.DataAnnotations;

namespace Restaurant.Web.Models.Dto
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discound { get; set; }
        public double CartTotal { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set;}
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
    }
}
