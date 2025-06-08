namespace CustomerApi.Models;
public class CustomerResponse
{
    required public Guid Id { get; set; }
    required public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    required public string LastName { get; set; }
    required public string Email { get; set; }
    required public string PhoneNumber { get; set; }
}