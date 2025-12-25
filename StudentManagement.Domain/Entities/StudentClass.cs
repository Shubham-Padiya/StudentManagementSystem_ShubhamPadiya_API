namespace StudentManagement.Domain.Entities;

public class StudentClass
{
    public Guid StudentId { get; set; }
    public virtual Student Student { get; set; } = null!;

    public Guid ClassId { get; set; }
    public virtual Class Class { get; set; } = null!;
}
