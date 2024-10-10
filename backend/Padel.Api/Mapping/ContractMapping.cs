using Padel.Application.Models;
using Padel.Contracts.Requests.Player;
using Padel.Contracts.Requests.Team;
using Padel.Contracts.Responses.Player;
using Padel.Contracts.Responses.Team;
using Padel.Domain.Models;

namespace Padel.Api.Mapping
{
    public static class ContractMapping
    {

        public static Team MapToTeam(this TeamCreateRequest request)
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

        public static Team MapToTeam(this TeamUpdateRequest request, Guid id)
        {
            return new Team
            {
                Id = id,
                MatchId = request.MatchId,
                Player1Id = request.Player1Id,
                Player2Id = request.Player2Id,
            };
        }

        // Player mapping

        public static Player MapToPlayer(this PlayerCreateRequest request)
        {
            return new Player
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Sex = request.Sex
            };
        }

        public static PlayerResponse MapToResponse(this Player player)
        {
            return new PlayerResponse
            {
                Id = player.Id,
                Name = player.Name,
                Sex = player.Sex
            };
        }

        public static PlayersResponse MapToResponse(this IEnumerable<Player> players)
        {
            return new PlayersResponse
            {
                Items = players.Select(MapToResponse).ToList()
            };
        }

        public static Player MapToPlayer(this PlayerUpdateRequest request, Guid id)
        {
            return new Player
            {
                Id = id,
                Name = request.Name,
                Sex = request.Sex
            };
        }


    }
}
