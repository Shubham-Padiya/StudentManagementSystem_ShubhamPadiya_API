namespace StudentManagement.Application.DTOs
{
    public class StudentGetDTO
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; } = "FirstName";
        public bool SortDesc { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
