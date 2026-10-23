using Dapper;
using Education.Application.Interfaces.Repositories;
using EducationSystem.Domain.Entities;
using System.Data;

namespace EducationSystem.Infrastructure.Repositories
{
    public sealed class GroupRepository : IGroupRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public GroupRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<Group> GetAll()
        {
            var groups = _connection.Query<Group>(@"SELECT Id, Name
                                        FROM Groups", transaction: _transaction).ToList();

            LoadStudents(groups);
            LoadTeachers(groups);
            return groups;
        }

        public Group? GetById(int id)
        {
            var group = _connection.QuerySingleOrDefault<Group>(@"SELECT Id, Name
                                                        FROM Groups
                                                        WHERE Id = @Id", new { Id = id }, transaction: _transaction);

            if (group is not null)
            {
                LoadStudents(new[] { group });
                LoadTeachers(new[] { group });
            }

            return group;
        }

        public void Add(Group entity)
        {
            entity.Id = _connection.QuerySingle<int>(
                """
                INSERT INTO Groups (Name)
                OUTPUT INSERTED.Id
                VALUES (@Name)
                """, entity, transaction: _transaction);
        }

        public void Update(Group entity)
        {
            _connection.Execute(
                """
                UPDATE Groups
                SET Name = @Name
                WHERE Id = @Id
                """, entity, transaction: _transaction);
        }

        public void Delete(Group entity)
        {
            _connection.Execute(@"DELETE FROM TeacherGroups WHERE GroupId = @Id",
                new { entity.Id }, transaction: _transaction);

            _connection.Execute(@"DELETE FROM Groups
                            WHERE Id = @Id", new { entity.Id }, transaction: _transaction);
        }

        private void LoadStudents(IReadOnlyCollection<Group> groups)
        {
            var groupIds = groups.Select(group => group.Id).ToArray();
            if (groupIds.Length == 0)
            {
                return;
            }

            var students = _connection.Query<Student>(
                "SELECT Id, Name, Surname, Age, GroupId FROM Students WHERE GroupId IN @Ids",
                new { Ids = groupIds },
                transaction: _transaction)
                .ToLookup(student => student.GroupId);

            foreach (var group in groups)
            {
                group.Students = students[group.Id].ToList();
            }
        }

        private void LoadTeachers(IReadOnlyCollection<Group> groups)
        {
            var groupIds = groups.Select(group => group.Id).ToArray();
            if (groupIds.Length == 0)
            {
                return;
            }

            var teachers = _connection.Query<TeacherGroupRow>(
                @"SELECT t.Id, t.Name, t.Surname, t.Age, t.Salary, tg.GroupId
                  FROM Teachers t
                  INNER JOIN TeacherGroups tg ON tg.TeacherId = t.Id
                  WHERE tg.GroupId IN @Ids",
                new { Ids = groupIds },
                transaction: _transaction)
                .ToLookup(row => row.GroupId);

            foreach (var group in groups)
            {
                group.Teachers = teachers[group.Id]
                    .Select(row =>
                    {
                        var teacher = new Teacher
                        {
                            Id = row.Id,
                            Name = row.Name,
                            Surname = row.Surname,
                            Age = row.Age,
                            Salary = row.Salary
                        };
                        teacher.Groups = new List<Group> { group };
                        return teacher;
                    })
                    .ToList();
            }
        }

        private sealed class TeacherGroupRow
        {
            public int Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public string Surname { get; set; } = string.Empty;

            public int Age { get; set; }

            public decimal Salary { get; set; }

            public int GroupId { get; set; }
        }
    }
}