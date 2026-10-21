using Education.Application.DTOs.Group;

namespace Education.Application.Interfaces.Services
{
    public interface IGroupService
    {
        IEnumerable<GroupDto> GetAll();

        GroupDto? GetById(int id);

        GroupDto Create(CreateGroupDto dto);

        GroupDto? Update(UpdateGroupDto dto);

        bool Delete(int id);
    }
}
