namespace StudentManagement.Application.DTOs
{
    public class StudentResponseDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string EmailId { get; set; } = null!;
        public List<string> Classes { get; set; } = new();
    }
}
