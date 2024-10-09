using Padel.Application.Models;
using Padel.Contracts.Requests.Player;
using Padel.Contracts.Requests.Team;

namespace Padel.Api.Mapping
{
    public static class ContractMapping
    {
        public static Team MapToTeam(this CreateTeamRequest request)
        {
            return new Team
            {
                Id = Guid.NewGuid(),
                MatchId = request.MatchId,
                Player1Id = request.Player1Id,
                Player2Id = request.Player2Id,
            };
        }

        public static TeamResponse MapToResponse(this Team team)
        {
            return new TeamResponse
            {
                Id = team.Id,
                MatchId = team.MatchId,
                Player1Id = team.Player1Id,
                Player2Id = team.Player2Id,
            };
        }

        public static TeamsResponse MapToResponse(this IEnumerable<Team> teams)
        {
            return new TeamsResponse
            {
                Items = teams.Select(MapToResponse).ToList()
            };
        }

        public static Team MapToTeam(this UpdateTeamRequest request, Guid id)
        {
            return new Team
            {
                Id = id,
                MatchId = request.MatchId,
                Player1Id = request.Player1Id,
                Player2Id = request.Player2Id,
            };
        }
    }
}
