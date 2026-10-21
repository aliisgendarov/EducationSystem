namespace Education.Application.DTOs.Group
{
    public class GroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IReadOnlyCollection<int> StudentIds { get; set; } = Array.Empty<int>();

        public IReadOnlyCollection<int> TeacherIds { get; set; } = Array.Empty<int>();
    }
}
