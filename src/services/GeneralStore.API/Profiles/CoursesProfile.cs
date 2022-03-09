using AutoMapper;
using GeneralStore.API.Dtos;
using GeneralStore.Models;

namespace GeneralStore.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            //On ignore Id car il sera automatiquement ajouté par la DB
            CreateMap<CoursePublishedDto, Course>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
