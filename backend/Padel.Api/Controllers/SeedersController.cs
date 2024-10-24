using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Padel.Api.Auth;
using Padel.Api.Mapping;
using Padel.Application.Models;
using Padel.Application.Services.Interfaces;
using Padel.Contracts.Requests.Season;
using PadelContracts.Requests.Seeder;


namespace Padel.Api.Controllers;

[ApiController]
public class SeedersController : ControllerBase
{
    private readonly ISeederService _seederService;

    public SeedersController(ISeederService seederService)
    {
        _seederService = seederService;
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPost(ApiEndpoints.Seeders.SeedParticipants)]
    public async Task<IActionResult> SeedParticipants([FromBody] SeedParticipantsRequest request, CancellationToken token)
    {
        // Create dummy participants
        var result =await _seederService.CreateManyParticipantsAsync(request.Count, request.SeasonId, token);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }

}


