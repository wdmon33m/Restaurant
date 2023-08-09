namespace Restaurant.Services.CouponAPI.Models.Dto
{
    public class CouponDto
    {
        public int CouponID { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public string MinAmount { get; set; }

    }
}
