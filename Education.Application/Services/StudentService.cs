using Education.Application.DTOs.Student;
using Education.Application.Interfaces;
using Education.Application.Interfaces.Services;
using EducationSystem.Domain.Entities;
using FluentValidation;

namespace Education.Application.Services
{
    public sealed class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateStudentDto> _createValidator;
        private readonly IValidator<UpdateStudentDto> _updateValidator;

        public StudentService(
            IUnitOfWork unitOfWork,
            IValidator<CreateStudentDto> createValidator,
            IValidator<UpdateStudentDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public IEnumerable<StudentDto> GetAll()
        {
            return _unitOfWork.StudentRepository.GetAll().Select(MapToDto).ToList();
        }

        public StudentDto? GetById(int id)
        {
            return _unitOfWork.StudentRepository.GetById(id) is { } student
                ? MapToDto(student)
                : null;
        }

        public StudentDto Create(CreateStudentDto dto)
        {
            Validate(_createValidator, dto);

            var student = new Student
            {
                Name = dto.Name.Trim(),
                Surname = dto.Surname.Trim(),
                Age = dto.Age,
                GroupId = dto.GroupId
            };

            _unitOfWork.StudentRepository.Add(student);
            _unitOfWork.Commit();

            return MapToDto(student);
        }

        public StudentDto? Update(UpdateStudentDto dto)
        {
            Validate(_updateValidator, dto);

            var student = _unitOfWork.StudentRepository.GetById(dto.Id);
            if (student is null)
            {
                return null;
            }

            student.Name = dto.Name.Trim();
            student.Surname = dto.Surname.Trim();
            student.Age = dto.Age;
            student.GroupId = dto.GroupId;

            _unitOfWork.StudentRepository.Update(student);
            _unitOfWork.Commit();

            return MapToDto(student);
        }

        public bool Delete(int id)
        {
            var student = _unitOfWork.StudentRepository.GetById(id);
            if (student is null)
            {
                return false;
            }

            _unitOfWork.StudentRepository.Delete(student);
            _unitOfWork.Commit();

            return true;
        }

        private static void Validate<T>(IValidator<T> validator, T dto)
        {
            var result = validator.Validate(dto);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

        private static StudentDto MapToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                Age = student.Age,
                GroupId = student.GroupId
            };
        }
    }
}
