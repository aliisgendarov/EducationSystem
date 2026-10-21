using Education.Application.DTOs.Manager;

namespace Education.Application.Interfaces.Services
{
    public interface IManagerService
    {
        IEnumerable<ManagerDto> GetAll();

        ManagerDto? GetById(int id);

        ManagerDto Create(CreateManagerDto dto);

        ManagerDto? Update(UpdateManagerDto dto);

        bool Delete(int id);
    }
}