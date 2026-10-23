using Education.Application.DTOs.Group;
using Education.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _service;

        public GroupsController(IGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GroupDto>> GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public ActionResult<GroupDto> GetById(int id)
        {
            var result = _service.GetById(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public ActionResult<GroupDto> Create(CreateGroupDto dto)
        {
            var result = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<GroupDto> Update(int id, UpdateGroupDto dto)
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