using Education.Application.DTOs.Student;

namespace Education.Application.Interfaces.Services
{
    public interface IStudentService
    {
        IEnumerable<StudentDto> GetAll();

        StudentDto? GetById(int id);

        StudentDto Create(CreateStudentDto dto);

        StudentDto? Update(UpdateStudentDto dto);

        bool Delete(int id);
    }
}
