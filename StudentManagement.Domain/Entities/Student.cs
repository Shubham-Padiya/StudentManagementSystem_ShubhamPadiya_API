using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Domain.Entities;

public class Student
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public string EmailId { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
}
