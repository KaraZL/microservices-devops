using AutoMapper;
using Courses.API.Data;
using Courses.API.Dtos;
using Courses.API.Grpc;
using Courses.API.MessageBus;
using Courses.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICoursesRepository repo, 
            IMapper mapper, 
            IMessageBusClient messageBusClient, 
            ICourseDataClient grpcClient,
            ILogger<CoursesController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _grpcClient = grpcClient;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
        //Retirer ActionResult<IEnumerable<Course>> vers ActionResult
        //pour les tests unitaires, sinon impossible de cast en OkObjectResult
        //Du coup, on utilise ProducesResponseType pour indiquer le type de données

        public async Task<ActionResult> GetAllCourses()
        {
            var courses = await _repo.GetAllCourses();
            _logger.LogInformation("--> Read : GetAllCourses");
            return Ok(courses);
        }

        [HttpGet("{id}", Name = "GetBasket")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]

        public async Task<ActionResult> GetCourseById(int id)
        {
            if (id < 0)
            {
                _logger.LogError("--> Read : GetCourseById - Id lower than 0");
                return BadRequest(id);
            }
            var course = await _repo.GetCourseById(id);
            _logger.LogInformation("--> Read : GetCourseById");
            return Ok(course);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]

        public async Task<ActionResult> CreateCourse([FromBody] Course course)
        {
            var result = await _repo.CreateCourse(course);
            if(result is false)
            {
                _logger.LogError("--> Create : CreateCourse - returned false");
                return BadRequest();
            }
            _logger.LogInformation("--> Create : CreateCourse");
            return Ok(course);
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
                _logger.LogInformation("--> RabbitMQ Sender : SendCourseToBus - Message Sent");
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> RabbitMQ Sender : SendCourseToBus - Message Couldn't be sent : {ex.Message}");
            }
            
            return Ok(message);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
        public ActionResult GetAllCoursesFromGrpcServer()
        {
            var courses = _grpcClient.ReturnAllCourses();
            _logger.LogInformation("--> Grpc Sender : GetAllCoursesFromGrpcServer");

            return Ok(courses);
        }

        [HttpPost("[action]")]
        public ActionResult<Course> GetACourseByIdFromGrpcServer(int id)
        {
            var course = _grpcClient.GetCourseById(id);
            _logger.LogInformation("--> Grpc Sender : GetACourseByIdFromGrpcServer");
            return Ok(course);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var result = await _repo.DeleteCourse(id);
            
            if(result is false)
            {
                _logger.LogError("--> Delete : DeleteCourse - Returned False");
                return BadRequest();
            }

            _logger.LogInformation("--> Delete : DeleteCourse");
            return Ok(result);
        }
    }
}
