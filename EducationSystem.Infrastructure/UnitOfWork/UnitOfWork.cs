using Education.Application.Interfaces;
using Education.Application.Interfaces.Repositories;
using EducationSystem.Infrastructure.Data;
using EducationSystem.Infrastructure.Repositories;
using System.Data;

namespace EducationSystem.Infrastructure.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private bool _completed;
        private bool _disposed;

        public UnitOfWork(IDbConnectionFactory connectionFactory)
        {
            ArgumentNullException.ThrowIfNull(connectionFactory);

            _connection = connectionFactory.CreateConnection();
            _connection.Open();

            try
            {
                _transaction = _connection.BeginTransaction();

                StudentRepository = new StudentRepository(_connection, _transaction);
                GroupRepository = new GroupRepository(_connection, _transaction);
                TeacherRepository = new TeacherRepository(_connection, _transaction);
                ManagerRepository = new ManagerRepository(_connection, _transaction);
            }
            catch
            {
                _connection.Dispose();
                throw;
            }
        }

        public IStudentRepository StudentRepository { get; }

        public IGroupRepository GroupRepository { get; }

        public ITeacherRepository TeacherRepository { get; }

        public IManagerRepository ManagerRepository { get; }

        public void Commit()
        {
            EnsureActive();

            _transaction.Commit();
            _completed = true;
        }

        public void Rollback()
        {
            EnsureActive();

            _transaction.Rollback();
            _completed = true;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (!_completed)
            {
                _transaction.Rollback();
            }

            _transaction.Dispose();
            _connection.Dispose();
            _disposed = true;
        }

        private void EnsureActive()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_completed)
            {
                throw new InvalidOperationException("The unit of work transaction has already completed.");
            }
        }
    }
}
