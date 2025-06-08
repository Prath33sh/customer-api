namespace CustomerApi.Models
{
    public class UserRequest
    {
        required public string FirstName { get; set; }
        required public string LastName { get; set; }
        public string? MiddleName { get; set; }
        required public string Email { get; set; }
        required public string PhoneNumber { get; set; }
    }
}   