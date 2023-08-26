using Restaurant.Services.OrderAPI.Utility;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Services.OrderAPI.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discound { get; set; }
        public double OrderTotal { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public string? Status { get; set; } = SD.Status_Pending;
        public string? PaymentIntenId { get; set; }
        public string? StripeSessionId { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set;}

    }
}
