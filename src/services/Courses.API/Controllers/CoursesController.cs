using AutoMapper;
using Courses.API.Data;
using Courses.API.Dtos;
using Courses.API.MessageBus;
using Courses.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public CoursesController(ICoursesRepository repo, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _repo = repo;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
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

        //Envoi sur le service bus
        [HttpPost("[action]")]
        public async Task<ActionResult> SendCourseToBus([FromBody] CoursePublishedDto message)
        {
            try
            {
                message.Event = "Courses_Published";
                _messageBusClient.PublishNewCourse(message);
                Console.WriteLine("--> Message sent to RabbitMQ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't send message : {ex.Message}");
            }
            
            return Ok(message);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var result = await _repo.DeleteCourse(id);

            return result is true ? Ok(result) : BadRequest();
        }
    }
}
