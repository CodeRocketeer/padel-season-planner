using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Padel.Api.Auth;
using Padel.Api.Mapping;
using Padel.Application.Services.Interfaces;
using PadelContracts.Requests.Participant;

namespace Padel.Api.Controllers
{
    public class ParticipantsController : ControllerBase
    {

        private readonly IParticipantService _participantService;

        public ParticipantsController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        [Authorize]
        [HttpPut(ApiEndpoints.Seasons.Participate)]
        public async Task<IActionResult> ParticipateInSeason([FromBody] CreateParticipantRequest request, [FromRoute] Guid seasonId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var participant = request.MapToParticipant(userId, seasonId);

            try
            {
                var result = await _participantService.ParticipateInSeasonAsync(participant, token);
                return result ? Ok() : NotFound(); // This line can also be simplified depending on your use case.
            }
            catch (DirectoryNotFoundException ex)
            {
                // Catch the exception and return a 404 Not Found with the message
                return NotFound(new { ex.Message });
            }

        }


        [Authorize]
        [HttpDelete(ApiEndpoints.Seasons.Leave)]
        public async Task<IActionResult> LeaveSeason([FromRoute] Guid seasonId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            try
            {
                var result = await _participantService.LeaveSeasonAsync(seasonId, userId!.Value, token);
                return result ? Ok() : NotFound();
            }
            catch (DirectoryNotFoundException ex)
            {
                // Catch the exception and return a 404 Not Found with the message
                return NotFound(new { ex.Message });
            }
        }

        [HttpGet(ApiEndpoints.Participants.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllParticipantsRequest request, CancellationToken token)
        {


            var options = request.MapToOptions();
               
            var participants = await _participantService.GetAllAsync(options, token);

            var participantsResponse = participants.MapToResponse();
            return Ok(participantsResponse);
        }


    }
}
