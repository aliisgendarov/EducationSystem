namespace Education.Application.DTOs.Manager
{
    public class CreateManagerDto
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public int Age { get; set; }

        public decimal Salary { get; set; }

        public int TeacherId { get; set; }
    }
}
