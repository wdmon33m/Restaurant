using System.ComponentModel.DataAnnotations;

namespace Restaurant.Web.Models.Dto
{
    public class CouponDto
    {
        public int CouponID { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public double MinAmount { get; set; }

    }
}
