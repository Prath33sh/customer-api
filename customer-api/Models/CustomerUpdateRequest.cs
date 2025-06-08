using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models;

public class CustomerUpdateRequest : CustomerRequest
{
    [Required(ErrorMessage = "Customer Id is required.")]
    required public Guid Id { get; set; }
}
