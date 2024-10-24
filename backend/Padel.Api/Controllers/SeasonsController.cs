using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Padel.Api.Auth;
using Padel.Api.Mapping;
using Padel.Application.Services.Interfaces;
using Padel.Contracts.Requests.Season;



namespace Padel.Api.Controllers;


[ApiController]
public class SeasonsController : ControllerBase
{
    private readonly ISeasonService _seasonService;

    public SeasonsController(ISeasonService seasonService)
    {
        _seasonService = seasonService;
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPost(ApiEndpoints.Seasons.Create)]
    public async Task<IActionResult> Create([FromBody] CreateSeasonRequest request,
        CancellationToken token)
    {
        var season = request.MapToSeason();
        await _seasonService.CreateAsync(season, token);
        var SeasonResponse = season.MapToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = season.Id }, SeasonResponse);
    }


    [HttpGet(ApiEndpoints.Seasons.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var season = Guid.TryParse(idOrSlug, out var id)
            ? await _seasonService.GetByIdAsync(id, userId, token)
            : await _seasonService.GetBySlugAsync(idOrSlug, userId, token);
        if (season is null)
        {
            return NotFound();
        }

        var response = season.MapToResponse();
        return Ok(response);
    }


    [HttpGet(ApiEndpoints.Seasons.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var seasons = await _seasonService.GetAllAsync(userId, token);

        var seasonsResponse = seasons.MapToResponse();
        return Ok(seasonsResponse);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPut(ApiEndpoints.Seasons.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateSeasonRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var season = request.MapToSeason(id);
        var updatedSeason = await _seasonService.UpdateAsync(season, userId, token);
        if (updatedSeason is null)
        {
            return NotFound();
        }

        var response = updatedSeason.MapToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Seasons.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id,
        CancellationToken token)
    {
        var deleted = await _seasonService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPut(ApiEndpoints.Seasons.Confirm)]
    public async Task<IActionResult> Confirm([FromRoute] Guid id, CancellationToken token)
    {
        try
        {
            var userId = HttpContext.GetUserId();
            var confirmed = await _seasonService.ConfirmSeasonAsync(id, userId,token);
            return confirmed ? Ok() : NotFound();
        }
        catch (DirectoryNotFoundException ex)
        {
            // Catch the exception and return a 404 Not Found with the message
            return NotFound(new { ex.Message });
        }
    }
}
