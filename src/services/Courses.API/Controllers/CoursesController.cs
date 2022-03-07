using Courses.API.Data;
using Courses.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Courses.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesRepository _repo;

        public CoursesController(ICoursesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetBasket")]

        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            var course = await _repo.GetCourseById(id);
            return Ok(course);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var courses = await _repo.GetAllCourses();
            return Ok(courses);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse([FromBody] Course course)
        {
            var result = await _repo.CreateCourse(course);

            return result is true ? Ok(course) : BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var result = await _repo.DeleteCourse(id);

            return result is true ? Ok(result) : BadRequest();
        }
    }
}
