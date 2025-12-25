namespace StudentManagement.Application.DTOs
{
    public class StudentUpdateDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public List<Guid> ClassIds { get; set; } = new();
    }
}
