using AutoMapper;
using Courses.API.Dtos;
using Courses.API.Models;
using Courses.API.Protos;

namespace Courses.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CoursePublishedDto>();

            //From grpc proto message to Course
            CreateMap<GrpcCourseModel, Course>();
        }
    }
}
