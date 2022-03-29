using AutoMapper;
using Series.API.Dto;
using Series.API.Models;

namespace Series.API.Profiles
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            //En ignorant, il prendra la valeur par defaut de CreateSeriesDto qui est "Guid.NewGuid...etc"
            CreateMap<SeriesModel, CreateSeriesDto>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
