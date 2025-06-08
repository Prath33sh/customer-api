using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerApi.Models;

public class CustomerUpdateRequest : CustomerRequest
{
    [Required(ErrorMessage = "Customer Id is required.")]
    [JsonPropertyName("id")]
    required public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Middle name is required. If not applicable, set it to null.")]
    public override string? MiddleName { get; set; }
}
