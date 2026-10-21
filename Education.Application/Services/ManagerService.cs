using Education.Application.DTOs.Manager;
using Education.Application.Interfaces;
using Education.Application.Interfaces.Services;
using EducationSystem.Domain.Entities;
using FluentValidation;

namespace Education.Application.Services
{
    public sealed class ManagerService : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateManagerDto> _createValidator;
        private readonly IValidator<UpdateManagerDto> _updateValidator;

        public ManagerService(
            IUnitOfWork unitOfWork,
            IValidator<CreateManagerDto> createValidator,
            IValidator<UpdateManagerDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public IEnumerable<ManagerDto> GetAll()
        {
            return _unitOfWork.ManagerRepository.GetAll().Select(MapToDto).ToList();
        }

        public ManagerDto? GetById(int id)
        {
            return _unitOfWork.ManagerRepository.GetById(id) is { } manager
                ? MapToDto(manager)
                : null;
        }

        public ManagerDto Create(CreateManagerDto dto)
        {
            Validate(_createValidator, dto);

            var manager = new Manager
            {
                Name = dto.Name.Trim(),
                Surname = dto.Surname.Trim(),
                Age = dto.Age,
                Salary = dto.Salary,
                TeacherId = dto.TeacherId
            };

            _unitOfWork.ManagerRepository.Add(manager);
            _unitOfWork.Commit();

            return MapToDto(manager);
        }

        public ManagerDto? Update(UpdateManagerDto dto)
        {
            Validate(_updateValidator, dto);

            var manager = _unitOfWork.ManagerRepository.GetById(dto.Id);
            if (manager is null)
            {
                return null;
            }

            manager.Name = dto.Name.Trim();
            manager.Surname = dto.Surname.Trim();
            manager.Age = dto.Age;
            manager.Salary = dto.Salary;
            manager.TeacherId = dto.TeacherId;

            _unitOfWork.ManagerRepository.Update(manager);
            _unitOfWork.Commit();

            return MapToDto(manager);
        }

        public bool Delete(int id)
        {
            var manager = _unitOfWork.ManagerRepository.GetById(id);
            if (manager is null)
            {
                return false;
            }

            _unitOfWork.ManagerRepository.Delete(manager);
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

        private static ManagerDto MapToDto(Manager manager)
        {
            return new ManagerDto
            {
                Id = manager.Id,
                Name = manager.Name,
                Surname = manager.Surname,
                Age = manager.Age,
                Salary = manager.Salary,
                TeacherId = manager.TeacherId
            };
        }
    }
}
