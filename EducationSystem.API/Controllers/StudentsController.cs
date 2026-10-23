using Education.Application.DTOs.Student;
using Education.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StudentDto>> GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public ActionResult<StudentDto> GetById(int id)
        {
            var result = _service.GetById(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public ActionResult<StudentDto> Create(CreateStudentDto dto)
        {
            var result = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<StudentDto> Update(int id, UpdateStudentDto dto)
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
