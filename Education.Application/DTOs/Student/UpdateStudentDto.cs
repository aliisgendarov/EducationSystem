namespace Education.Application.DTOs.Student
{
    public class UpdateStudentDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public int Age { get; set; }

        public int GroupId { get; set; }
    }
}
