using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models;

public class CustomerRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    required public string FirstName { get; set; }
    [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters.")]
    public virtual string? MiddleName { get; set; }
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    required public string LastName { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    required public string Email { get; set; }
    [MinLength(10, ErrorMessage = "Phone number must be at least 10 characters long.")]
    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
    required public string PhoneNumber { get; set; } // accept single string for now. can be made composite later
}