namespace Education.Application.DTOs.TeacherDtos
{
    public class TeacherDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public int Age { get; set; }

        public decimal Salary { get; set; }

        public IReadOnlyCollection<int> GroupIds { get; set; } = Array.Empty<int>();

        public IReadOnlyCollection<int> ManagerIds { get; set; } = Array.Empty<int>();
    }
}
