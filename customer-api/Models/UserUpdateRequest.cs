namespace CustomerApi.Models
{
    public class UserUpdateRequest : UserRequest
    {
        required public Guid Id { get; set; }
    }
}