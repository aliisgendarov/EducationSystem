using Education.Application.DTOs.TeacherDtos;
using Education.Application.Interfaces;
using Education.Application.Interfaces.Services;
using EducationSystem.Domain.Entities;
using FluentValidation;

namespace Education.Application.Services
{
    public sealed class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTeacherDto> _createValidator;
        private readonly IValidator<UpdateTeacherDto> _updateValidator;

        public TeacherService(
            IUnitOfWork unitOfWork,
            IValidator<CreateTeacherDto> createValidator,
            IValidator<UpdateTeacherDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public IEnumerable<TeacherDto> GetAll()
        {
            return _unitOfWork.TeacherRepository
                .GetAll()
                .Select(MapToDto)
                .ToList();
        }

        public TeacherDto? GetById(int id)
        {
            return _unitOfWork.TeacherRepository
                .GetById(id) is { } teacher
                ? MapToDto(teacher)
                : null;
        }

        public TeacherDto Create(CreateTeacherDto dto)
        {
            Validate(_createValidator, dto);

            var teacher = new Teacher
            {
                Name = dto.Name.Trim(),
                Surname = dto.Surname.Trim(),
                Age = dto.Age,
                Salary = dto.Salary
            };

            _unitOfWork.TeacherRepository.Add(teacher);
            _unitOfWork.Commit();

            return MapToDto(teacher);
        }

        public TeacherDto? Update(UpdateTeacherDto dto)
        {
            Validate(_updateValidator, dto);

            var teacher = _unitOfWork.TeacherRepository.GetById(dto.Id);
            if (teacher is null)
            {
                return null;
            }

            teacher.Name = dto.Name.Trim();
            teacher.Surname = dto.Surname.Trim();
            teacher.Age = dto.Age;
            teacher.Salary = dto.Salary;

            _unitOfWork.TeacherRepository.Update(teacher);
            _unitOfWork.Commit();

            return MapToDto(teacher);
        }

        public bool Delete(int id)
        {
            var teacher = _unitOfWork.TeacherRepository.GetById(id);
            if (teacher is null)
            {
                return false;
            }

            _unitOfWork.TeacherRepository.Delete(teacher);
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

        private static TeacherDto MapToDto(Teacher teacher)
        {
            return new TeacherDto
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Surname = teacher.Surname,
                Age = teacher.Age,
                Salary = teacher.Salary,
                GroupIds = teacher.Groups.Select(group => group.Id).ToArray(),
                ManagerIds = teacher.Managers.Select(manager => manager.Id).ToArray()
            };
        }
    }
}
