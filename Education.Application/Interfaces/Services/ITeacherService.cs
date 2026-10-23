using Education.Application.DTOs.TeacherDtos;

namespace Education.Application.Interfaces.Services
{
    public interface ITeacherService
    {
        IEnumerable<TeacherDto> GetAll();

        TeacherDto? GetById(int id);

        TeacherDto Create(CreateTeacherDto dto);

        TeacherDto? Update(UpdateTeacherDto dto);

        bool Delete(int id);

        bool AddToGroup(int teacherId, int groupId);

        bool RemoveFromGroup(int teacherId, int groupId);
    }
}