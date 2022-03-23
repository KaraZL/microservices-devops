using AutoMapper;
using Courses.API.Data;
using Courses.API.Dtos;
using Courses.API.Grpc;
using Courses.API.MessageBus;
using Courses.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly ICourseDataClient _grpcClient;

        public CoursesController(ICoursesRepository repo, IMapper mapper, IMessageBusClient messageBusClient, ICourseDataClient grpcClient)
        {
            _repo = repo;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _grpcClient = grpcClient;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
        //Retirer ActionResult<IEnumerable<Course>> vers ActionResult
        //pour les tests unitaires, sinon impossible de cast en OkObjectResult
        //Du coup, on utilise ProducesResponseType pour indiquer le type de données

        public async Task<ActionResult> GetAllCourses()
        {
            var courses = await _repo.GetAllCourses();
            return Ok(courses);
        }

        [HttpGet("{id}", Name = "GetBasket")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]

        public async Task<ActionResult> GetCourseById(int id)
        {
            if (id < 0)
            {
                return BadRequest(id);
            }
            var course = await _repo.GetCourseById(id);
            return Ok(course);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]

        public async Task<ActionResult> CreateCourse([FromBody] Course course)
        {
            var result = await _repo.CreateCourse(course);

            return result is true ? Ok(course) : BadRequest();
        }

        //Envoi sur le service bus
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoursePublishedDto))]
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

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
        public ActionResult GetAllCoursesFromGrpcServer()
        {
            var courses = _grpcClient.ReturnAllCourses();

            return Ok(courses);
        }

        [HttpPost("[action]")]
        public ActionResult<Course> GetACourseByIdFromGrpcServer(int id)
        {
            var course = _grpcClient.GetCourseById(id);

            return Ok(course);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var result = await _repo.DeleteCourse(id);

            return result is true ? Ok(result) : BadRequest();
        }
    }
}
