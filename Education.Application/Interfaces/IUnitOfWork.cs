using Education.Application.Interfaces.Repositories;

namespace Education.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository StudentRepository { get; }

        IGroupRepository GroupRepository { get; }

        ITeacherRepository TeacherRepository { get; }

        IManagerRepository ManagerRepository { get; }

        void Commit();

        void Rollback();
    }
}
