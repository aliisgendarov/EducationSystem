using Dapper;
using Education.Application.Interfaces.Repositories;
using EducationSystem.Domain.Entities;
using System.Data;

namespace EducationSystem.Infrastructure.Repositories
{
    public sealed class ManagerRepository : IManagerRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public ManagerRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<Manager> GetAll()
        {
            var managers = _connection.Query<Manager>(@"SELECT Id, Name, Surname, Age, Salary, TeacherId
                                            FROM Managers", transaction: _transaction).ToList();

            LoadTeachers(managers);
            return managers;
        }

        public Manager? GetById(int id)
        {
            var manager = _connection.QuerySingleOrDefault<Manager>(@"SELECT Id, Name, Surname, Age, Salary, TeacherId
                                                            FROM Managers WHERE Id = @Id", new { Id = id }, transaction: _transaction);

            if (manager is not null)
            {
                LoadTeachers(new[] { manager });
            }

            return manager;
        }

        public void Add(Manager entity)
        {
            entity.Id = _connection.QuerySingle<int>(
                """
                INSERT INTO Managers (Name, Surname, Age, Salary, TeacherId)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Surname, @Age, @Salary, @TeacherId)
                """, entity, transaction: _transaction);
        }

        public void Update(Manager entity)
        {
            _connection.Execute(
                """
                UPDATE Managers
                SET Name = @Name,
                    Surname = @Surname,
                    Age = @Age,
                    Salary = @Salary,
                    TeacherId = @TeacherId
                WHERE Id = @Id
                """, entity, transaction: _transaction);
        }

        public void Delete(Manager entity)
        {
            _connection.Execute(@"DELETE FROM Managers
                                WHERE Id = @Id", new { entity.Id }, transaction: _transaction);
        }

        private void LoadTeachers(IReadOnlyCollection<Manager> managers)
        {
            var teacherIds = managers.Select(manager => manager.TeacherId).Distinct().ToArray();
            if (teacherIds.Length == 0)
            {
                return;
            }

            var teachers = _connection.Query<Teacher>(
                "SELECT Id, Name, Surname, Age, Salary FROM Teachers WHERE Id IN @Ids",
                new { Ids = teacherIds },
                transaction: _transaction)
                .ToDictionary(teacher => teacher.Id);

            foreach (var manager in managers)
            {
                if (teachers.TryGetValue(manager.TeacherId, out var teacher))
                {
                    manager.Teacher = teacher;
                }
            }
        }
    }
}
