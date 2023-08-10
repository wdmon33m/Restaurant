namespace Restaurant.Services.AuthAPI.Models.Dto
{
    public class RegistrationRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
