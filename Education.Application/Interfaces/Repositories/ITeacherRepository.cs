using EducationSystem.Domain.Entities;

namespace Education.Application.Interfaces.Repositories
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        void AddGroup(int teacherId, int groupId);

        void RemoveGroup(int teacherId, int groupId);
    }
}