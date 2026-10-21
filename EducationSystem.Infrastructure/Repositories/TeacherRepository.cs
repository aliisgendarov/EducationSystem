using Dapper;
using Education.Application.Interfaces.Repositories;
using EducationSystem.Domain.Entities;
using System.Data;

namespace EducationSystem.Infrastructure.Repositories
{
    public sealed class TeacherRepository : ITeacherRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public TeacherRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<Teacher> GetAll()
        {
            var teachers = _connection.Query<Teacher>(@"SELECT Id, Name, Surname, Age, Salary
                                            FROM Teachers", transaction: _transaction).ToList();

            LoadGroups(teachers);
            LoadManagers(teachers);
            return teachers;
        }

        public Teacher? GetById(int id)
        {
            var teacher = _connection.QuerySingleOrDefault<Teacher>(@"SELECT Id, Name, Surname, Age, Salary
                                                        FROM Teachers
                                                        WHERE Id = @Id", new { Id = id }, transaction: _transaction);

            if (teacher is not null)
            {
                LoadGroups(new[] { teacher });
                LoadManagers(new[] { teacher });
            }

            return teacher;
        }

        public void Add(Teacher entity)
        {
            entity.Id = _connection.QuerySingle<int>(
                """
                INSERT INTO Teachers (Name, Surname, Age, Salary)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Surname, @Age, @Salary)
                """, entity, transaction: _transaction);
        }

        public void Update(Teacher entity)
        {
            _connection.Execute(
                """
                UPDATE Teachers
                SET Name = @Name,
                    Surname = @Surname,
                    Age = @Age,
                    Salary = @Salary
                WHERE Id = @Id
                """, entity, transaction: _transaction);
        }

        public void Delete(Teacher entity)
        {
            _connection.Execute(@"DELETE FROM Teachers
                            WHERE Id = @Id", new { entity.Id }, transaction: _transaction);
        }

        private void LoadGroups(IReadOnlyCollection<Teacher> teachers)
        {
            var teacherIds = teachers.Select(teacher => teacher.Id).ToArray();
            if (teacherIds.Length == 0)
            {
                return;
            }

            var groups = _connection.Query<TeacherGroupRow>(
                @"SELECT tg.TeacherId, g.Id, g.Name
                  FROM TeacherGroups tg
                  INNER JOIN Groups g ON g.Id = tg.GroupId
                  WHERE tg.TeacherId IN @Ids",
                new { Ids = teacherIds },
                transaction: _transaction)
                .ToLookup(row => row.TeacherId);

            foreach (var teacher in teachers)
            {
                teacher.Groups = groups[teacher.Id]
                    .Select(row =>
                    {
                        var group = new Group
                        {
                            Id = row.Id,
                            Name = row.Name
                        };
                        group.Teachers = new List<Teacher> { teacher };
                        return group;
                    })
                    .ToList();
            }
        }

        private void LoadManagers(IReadOnlyCollection<Teacher> teachers)
        {
            var teacherIds = teachers.Select(teacher => teacher.Id).ToArray();
            if (teacherIds.Length == 0)
            {
                return;
            }

            var managers = _connection.Query<Manager>(
                "SELECT Id, Name, Surname, Age, Salary, TeacherId FROM Managers WHERE TeacherId IN @Ids",
                new { Ids = teacherIds },
                transaction: _transaction)
                .ToLookup(manager => manager.TeacherId);

            foreach (var teacher in teachers)
            {
                teacher.Managers = managers[teacher.Id]
                    .Select(manager =>
                    {
                        manager.Teacher = teacher;
                        return manager;
                    })
                    .ToList();
            }
        }

        private sealed class TeacherGroupRow
        {
            public int TeacherId { get; set; }

            public int Id { get; set; }

            public string Name { get; set; } = string.Empty;
        }
    }
}
