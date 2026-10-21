namespace EducationSystem.Domain.Entities
{
    public class Group
    {
        public Group(int ıd, string name, ICollection<Student> students, ICollection<Teacher> teachers)
        {
            Id = ıd;
            Name = name;
            Students = students;
            Teachers = teachers;
        }

        public Group()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public ICollection<Student> Students { get; set; } = new List<Student>();

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

        public override string ToString()
        {
            return $"Id: {Id}\nName: {Name}";
        }
    }
}
