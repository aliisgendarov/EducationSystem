using EducationSystem.Domain.Comman;
using System.Text.RegularExpressions;

namespace EducationSystem.Domain.Entities
{
    public class Student : Person
    {
        public Student(int id, string name, string surname, int age, int groupId, Group group)
            : base(id, name, surname, age)
        {
            GroupId = groupId;
            Group = group;
        }

        public Student()
        {

        }

        public int GroupId { get; set; }

        public Group Group { get; set; } = new Group();

        public override string ToString()
        {
            return $"{base.ToString()}\nGroupId: {GroupId}";
        }
    }
}
