namespace CustomerApi.Models
{
    public class CustomerUpdateRequest : CustomerRequest
    {
        required public Guid Id { get; set; }
    }
}