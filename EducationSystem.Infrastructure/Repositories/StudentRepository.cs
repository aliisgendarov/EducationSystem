using Dapper;
using Education.Application.Interfaces.Repositories;
using EducationSystem.Domain.Entities;
using System.Data;

namespace EducationSystem.Infrastructure.Repositories
{
    public sealed class StudentRepository : IStudentRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public StudentRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<Student> GetAll()
        {
            var students = _connection.Query<Student>(@"SELECT Id, Name, Surname, Age, GroupId
                                            FROM Students", transaction: _transaction).ToList();

            LoadGroups(students);
            return students;
        }

        public Student? GetById(int id)
        {
            var student = _connection.QuerySingleOrDefault<Student>(@"SELECT Id, Name, Surname, Age, GroupId
                                                        FROM Students
                                                        WHERE Id = @Id", new { Id = id }, transaction: _transaction);

            if (student is not null)
            {
                LoadGroups(new[] { student });
            }

            return student;
        }

        public void Add(Student entity)
        {
            entity.Id = _connection.QuerySingle<int>(
                """
                INSERT INTO Students (Name, Surname, Age, GroupId)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Surname, @Age, @GroupId)
                """, entity, transaction: _transaction);
        }

        public void Update(Student entity)
        {
            _connection.Execute(
                """
                UPDATE Students
                SET Name = @Name,
                    Surname = @Surname,
                    Age = @Age,
                    GroupId = @GroupId
                WHERE Id = @Id
                """, entity, transaction: _transaction);
        }

        public void Delete(Student entity)
        {
            _connection.Execute(@"DELETE FROM Students
                            WHERE Id = @Id", new { entity.Id }, transaction: _transaction);
        }

        private void LoadGroups(IReadOnlyCollection<Student> students)
        {
            var groupIds = students.Select(student => student.GroupId).Distinct().ToArray();
            if (groupIds.Length == 0)
            {
                return;
            }

            var groups = _connection.Query<Group>(
                "SELECT Id, Name FROM Groups WHERE Id IN @Ids",
                new { Ids = groupIds },
                transaction: _transaction)
                .ToDictionary(group => group.Id);

            foreach (var student in students)
            {
                if (groups.TryGetValue(student.GroupId, out var group))
                {
                    student.Group = group;
                }
            }
        }
    }
}
