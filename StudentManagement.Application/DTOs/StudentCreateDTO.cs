namespace StudentManagement.Application.DTOs
{
    public class StudentCreateDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string EmailId { get; set; } = null!;
        public List<Guid> ClassIds { get; set; } = new();
    }
}
