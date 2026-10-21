using EducationSystem.Domain.Comman;

namespace EducationSystem.Domain.Entities
{
    public class Manager : Person
    {
        public Manager(int id, string name, string surname, int age, decimal salary, int teacherId, Teacher teacher)
            : base(id, name, surname, age)
        {
            Salary = salary;
            TeacherId = teacherId;
            Teacher = teacher;
        }

        public Manager()
        {

        }

        public decimal Salary { get; set; }

        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; } = null!;

        public override string ToString()
        {
            return $"{base.ToString()}\nSalary: {Salary}";
        }
    }
}
