using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Series.API.Data;
using Series.API.Dto;
using Series.API.Models;

namespace Series.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesRepo _repo;
        private readonly IMapper _mapper;

        public SeriesController(ISeriesRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("[action]", Name = "GetAllSeries")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SeriesModel>))]
        public IActionResult GetAll()
        {
            var list = _repo.GetAllSeries();

            if (list is null || !list.Any())
            {
                return NotFound();
            }

            return Ok(list);
        }

        [HttpPost("[action]", Name = "PostSeries")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateSeriesDto))]
        public IActionResult PostSeries(SeriesModel model)
        {
            var dto = _mapper.Map<CreateSeriesDto>(model);
            _repo.CreateSeries(dto);

            return Ok(dto);
        }

        [HttpGet("[action]", Name = "GetSeriesById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateSeriesDto))]
        public IActionResult GetById(string id)
        {
            var series = _repo.GetSeriesById(id);

            if(series is null)
            {
                return NotFound();
            }

            return Ok(series);
        }
    }
}
