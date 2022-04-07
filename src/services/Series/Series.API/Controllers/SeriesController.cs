using MediatR;
using Microsoft.AspNetCore.Mvc;
using Series.Application.Commands;
using Series.Application.Queries;
using Series.Domain.Entites;

namespace Series.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SeriesController> _logger;

        public SeriesController(IMediator mediator, ILogger<SeriesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("[action]", Name = "GetAllSeries")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SeriesModel>))]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllSeriesQuery();
            var list = await _mediator.Send(query);

            if (list is null || !list.Any())
            {
                _logger.LogError("--> Query : GetAllSeries - Null or Empty");
                return NotFound();
            }

            _logger.LogInformation("--> Query : GetAllSeries");

            return Ok(list);
        }

        [HttpPost("[action]", Name = "PostSeries")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SeriesModel))]
        public async Task<IActionResult> PostSeries(SeriesModel model)
        {
            model.Id = $"series:{Guid.NewGuid()}";
            var command = new CreateSeriesCommand(model);
            var result = await _mediator.Send(command);

            _logger.LogInformation("--> Command : PostSeries");

            return Ok(result);
        }

        [HttpGet("[action]", Name = "GetSeriesById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SeriesModel))]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetSeriesByIdQuery(id);
            var series = await _mediator.Send(query);

            if(series is null)
            {
                _logger.LogError("--> Query : GetById : Null");
                return NotFound();
            }

            _logger.LogInformation("--> Query : GetById");
            return Ok(series);
        }
    }
}
