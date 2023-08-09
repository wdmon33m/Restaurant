using System.ComponentModel.DataAnnotations;

namespace Restaurant.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Required]
        public int CouponID { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        [Required]
        public double MinAmount { get; set; }

    }
}
