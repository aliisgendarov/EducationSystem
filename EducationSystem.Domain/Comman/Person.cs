namespace EducationSystem.Domain.Comman
{
    public class Person
    {
        public Person(int ıd, string name, string surname, int age)
        {
            Id = ıd;
            Name = name;
            Surname = surname;
            Age = age;
        }

        public Person()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public string Surname { get; set; } = String.Empty;

        public int Age { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\nName: {Name}\nSurname: {Surname}\nAge: {Age}";
        }
    }
}
