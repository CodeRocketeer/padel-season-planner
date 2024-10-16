using Microsoft.AspNetCore.Mvc;
using Padel.Api.Mapping;
using Padel.Application.Services;
using Padel.Contracts.Requests.Season;

namespace Padel.Api.Controllers
{

    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private readonly ISeasonService _seasonService;

        public SeasonsController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }


        [HttpPost(ApiEndpoints.Seasons.Create)]
        public async Task<IActionResult> Create([FromBody] SeasonCreateRequest request, CancellationToken token)
        {
            var season = request.MapToSeason();

            season.StartDate = DateTime.Now;
            season.AmountOfMatches = 20;
            season.DayOfWeek = 2;


            var result = await _seasonService.CreateAsync(season, token);
            return CreatedAtAction(nameof(Get), new { id = season.Id }, season.MapToResponse());
        }

        [HttpGet(ApiEndpoints.Seasons.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var season = await _seasonService.GetByIdAsync(id, token);

            if (season is null) return NotFound();
            var response = season.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Seasons.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var seasons = await _seasonService.GetAllAsync(token);
            var moviesResponse = seasons.MapToResponse();
            return Ok(moviesResponse);
        }

        [HttpPut(ApiEndpoints.Seasons.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SeasonUpdateRequest request, CancellationToken token)
        {
            var season = request.MapToSeason(id);
            var updatedSeason = await _seasonService.UpdateAsync(season, token);

            if (updatedSeason is null) return NotFound();
            return Ok(season.MapToResponse());
        }

        [HttpDelete(ApiEndpoints.Seasons.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _seasonService.DeleteByIdAsync(id, token);

            if (!deleted) return NotFound();

            return Ok();

        }

        [HttpPost(ApiEndpoints.Seasons.ConfirmSeason)]
        public async Task<IActionResult> ConfirmSeason([FromRoute] Guid id, CancellationToken token)
        {
            var confirmed = await _seasonService.PopulateSeasonAsync(id, token);

            if (!confirmed) return NotFound();
            return Ok();
        }






    }
}
