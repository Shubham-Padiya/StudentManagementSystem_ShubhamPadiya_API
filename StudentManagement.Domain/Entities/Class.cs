using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Domain.Entities;

public class Class
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [MaxLength(100)]
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
}
