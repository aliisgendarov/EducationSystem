using Education.Application.DTOs.Manager;
using Education.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagersController : ControllerBase
    {
        private readonly IManagerService _service;

        public ManagersController(IManagerService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ManagerDto>> GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public ActionResult<ManagerDto> GetById(int id)
        {
            var result = _service.GetById(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public ActionResult<ManagerDto> Create(CreateManagerDto dto)
        {
            var result = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ManagerDto> Update(int id, UpdateManagerDto dto)
        {
            dto.Id = id;
            var result = _service.Update(dto);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return _service.Delete(id) ? NoContent() : NotFound();
        }
    }
}