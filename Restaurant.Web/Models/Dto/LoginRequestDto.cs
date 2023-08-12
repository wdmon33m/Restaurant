namespace Restaurant.Web.Models.Dto
{
    public class LoginRequestDto
    {
        public string UserName{ get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
