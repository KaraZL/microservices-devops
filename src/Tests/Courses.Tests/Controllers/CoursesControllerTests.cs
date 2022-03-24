using AutoMapper;
using Courses.API.Controllers;
using Courses.API.Data;
using Courses.API.Dtos;
using Courses.API.Grpc;
using Courses.API.MessageBus;
using Courses.API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Courses.Tests.Controllers
{
    //Unit Test
    public class CoursesControllerTests
    {
        private readonly CoursesController _sut; //system under test
        private readonly Mock<ICoursesRepository> _repo = new Mock<ICoursesRepository>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IMessageBusClient> _messageBus = new Mock<IMessageBusClient>();
        private readonly Mock<ICourseDataClient> _dataClient = new Mock<ICourseDataClient>();


        public CoursesControllerTests()
        {
            _sut = new CoursesController(
                _repo.Object, 
                _mapper.Object, 
                _messageBus.Object, 
                _dataClient.Object);
        }

        [Fact]
        public async Task GetAllCourses_ShouldReturnsAllCourses()
        {
            //Arrange
            IEnumerable<Course> courses = new List<Course>()
            {
                new Course()
                {
                    Id = 1,
                    Name = "Zafina",
                    Duration = 20,
                    Price = 69.99
                },
                new Course()
                {
                    Id = 2,
                    Name = "Kazuya",
                    Duration = 15,
                    Price = 89.99
                },
            };

            _repo.Setup(x => x.GetAllCourses()).ReturnsAsync(courses);

            //Act

            //On transforme en OkObjectResult pour pouvoir récupérer les données dans Value
            //Car on retourne Ok(data) dans le controller
            var call = await _sut.GetAllCourses() as OkObjectResult;

            //Assert
            //On le cast car on ne connait pas les données dans Value pour le moment
            IEnumerable<Course> result = call.Value as IEnumerable<Course>;

            result.Should().NotBeEmpty();
            result.Count().Should().Be(courses.Count());
            result.Should().Contain(courses);

        }

        [Fact]
        public async Task GetCourseById_ShouldReturnsCourse()
        {
            //Arrange
            var rand = new Random();
            var id = rand.Next(0,100); //sent in parameter

            var course = new Course
            {
                Id = id,
                Name = "Zafina",
                Duration = 20,
                Price = 69.99
            };

            _repo.Setup(x => x.GetCourseById(id)).ReturnsAsync(course);

            //Act
            var call = await _sut.GetCourseById(id) as OkObjectResult;

            //Assert
            var result = call?.Value as Course;
            var statusCode = call?.StatusCode; //200 OK

            result.Should().NotBeNull();
            result.Should().Be(course);
            statusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetCourseById_ShouldBeValueNull_WhenIdIsNegative()
        {
            //Arrange
            var rand = new Random();
            var id = rand.Next(-100, -1); //sent in parameter

            var course = new Course
            {
                Id = id,
                Name = "Zafina",
                Duration = 20,
                Price = 69.99
            };

            _repo.Setup(x => x.GetCourseById(id)).ReturnsAsync(() => null);

            //Act
            var call = await _sut.GetCourseById(id) as OkObjectResult;

            //Assert
            var result = call?.Value as Course;
            var statusCode = call?.StatusCode; //Inutile car BedRequest Renvoie Null

            result.Should().BeNull();
        }

        [Fact]
        public async Task SendCourseToBus_ShouldReturnsMessageDto()
        {
            //Arrange

            var message = new CoursePublishedDto
            {
                Id = 1,
                Name = "Zafina is the best",
                Event = "Courses_Published"
            };

            _messageBus.Setup(x => x.PublishNewCourse(message));

            //Act

            var call = await _sut.SendCourseToBus(message) as OkObjectResult;

            //Assert
            var result = call.Value as CoursePublishedDto;
            var statusCode = call.StatusCode;

            call.Should().NotBeNull();
            result.Should().Be(message);
            statusCode.Should().Be(200);
        }

    }
}
