using Education.Application.DTOs.TeacherDtos;
using Education.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _service;

        public TeachersController(ITeacherService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TeacherDto>> GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public ActionResult<TeacherDto> GetById(int id)
        {
            var result = _service.GetById(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public ActionResult<TeacherDto> Create(CreateTeacherDto dto)
        {
            var result = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TeacherDto> Update(int id, UpdateTeacherDto dto)
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

        [HttpPost("{teacherId:int}/groups/{groupId:int}")]
        public IActionResult AddToGroup(int teacherId, int groupId)
        {
            return _service.AddToGroup(teacherId, groupId) ? NoContent() : NotFound();
        }

        [HttpDelete("{teacherId:int}/groups/{groupId:int}")]
        public IActionResult RemoveFromGroup(int teacherId, int groupId)
        {
            return _service.RemoveFromGroup(teacherId, groupId) ? NoContent() : NotFound();
        }
    }
}