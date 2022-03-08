using AutoMapper;
using Courses.API.Dtos;
using Courses.API.Models;

namespace Courses.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CoursePublishedDto>();
        }
    }
}
