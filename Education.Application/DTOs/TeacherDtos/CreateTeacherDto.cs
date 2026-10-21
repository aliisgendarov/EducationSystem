namespace Education.Application.DTOs.TeacherDtos
{
    public class CreateTeacherDto
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public int Age { get; set; }

        public decimal Salary { get; set; }
    }
}
