using EducationSystem.Domain.Comman;

namespace EducationSystem.Domain.Entities
{
    public class Teacher : Person
    {
        public Teacher(int id, string name, string surname, int age, decimal salary, ICollection<Group> groups, ICollection<Manager> managers)
            : base(id, name, surname, age)
        {
            Salary = salary;
            Groups = groups;
            Managers = managers;
        }

        public Teacher()
        {

        }

        public decimal Salary { get; set; }

        public ICollection<Group> Groups { get; set; } = new List<Group>();

        public ICollection<Manager> Managers { get; set; } = new List<Manager>();

        public override string ToString()
        {
            return $"{base.ToString()}\nSalary: {Salary}";
        }
    }
}
