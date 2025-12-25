namespace StudentManagement.Application.DTOs
{
    public class ClassResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
