using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerApi.Models;

public class CustomerUpdateRequest : CustomerRequest
{
    [JsonIgnore] // This will be set from path
    public Guid? Id { get; set; }

    [Required(AllowEmptyStrings = true, ErrorMessage = "Middle name is required. If not applicable, set it to empty string.")]
    public override string? MiddleName { get; set; }
}
