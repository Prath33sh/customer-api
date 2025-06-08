using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerApi.Data.Entities;
public class Customer : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [MaxLength(50)]
    required public string FirstName { get; set; }
    [MaxLength(50)]
    public string? MiddleName { get; set; }
    [MaxLength(50)]
    required public string LastName { get; set; }
    [MaxLength(100)]
    required public string Email { get; set; }
    [MaxLength(15)]
    required public string PhoneNumber { get; set; }
    public bool IsDeleted { get; set; } = false;  // Soft delete flag
} 