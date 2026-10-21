using Education.Application.DTOs.Group;
using Education.Application.Interfaces;
using Education.Application.Interfaces.Services;
using EducationSystem.Domain.Entities;
using FluentValidation;

namespace Education.Application.Services
{
    public sealed class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateGroupDto> _createValidator;
        private readonly IValidator<UpdateGroupDto> _updateValidator;

        public GroupService(
            IUnitOfWork unitOfWork,
            IValidator<CreateGroupDto> createValidator,
            IValidator<UpdateGroupDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public IEnumerable<GroupDto> GetAll()
        {
            return _unitOfWork.GroupRepository.GetAll().Select(MapToDto).ToList();
        }

        public GroupDto? GetById(int id)
        {
            return _unitOfWork.GroupRepository.GetById(id) is { } group
                ? MapToDto(group)
                : null;
        }

        public GroupDto Create(CreateGroupDto dto)
        {
            Validate(_createValidator, dto);

            var group = new Group
            {
                Name = dto.Name.Trim()
            };

            _unitOfWork.GroupRepository.Add(group);
            _unitOfWork.Commit();

            return MapToDto(group);
        }

        public GroupDto? Update(UpdateGroupDto dto)
        {
            Validate(_updateValidator, dto);

            var group = _unitOfWork.GroupRepository.GetById(dto.Id);
            if (group is null)
            {
                return null;
            }

            group.Name = dto.Name.Trim();

            _unitOfWork.GroupRepository.Update(group);
            _unitOfWork.Commit();

            return MapToDto(group);
        }

        public bool Delete(int id)
        {
            var group = _unitOfWork.GroupRepository.GetById(id);
            if (group is null)
            {
                return false;
            }

            _unitOfWork.GroupRepository.Delete(group);
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

        private static GroupDto MapToDto(Group group)
        {
            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                StudentIds = group.Students.Select(student => student.Id).ToArray(),
                TeacherIds = group.Teachers.Select(teacher => teacher.Id).ToArray()
            };
        }
    }
}
