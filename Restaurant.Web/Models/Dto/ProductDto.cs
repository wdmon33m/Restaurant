﻿using Restaurant.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Web.Models.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? Image { get; set; }
        [Range(1,100)]
        public int Count { get; set; } = 1;

    }
}
