using Padel.Contracts.Requests.Match;
using Padel.Contracts.Requests.Player;
using Padel.Contracts.Requests.Season;
using Padel.Contracts.Requests.Team;
using Padel.Contracts.Responses.Match;
using Padel.Contracts.Responses.Player;
using Padel.Contracts.Responses.Season;
using Padel.Contracts.Responses.Team;
using Padel.Domain.Models;

namespace Padel.Api.Mapping
{
    public static class ContractMapping
    {

        public static Team MapToTeam(this TeamCreateRequest request)
        {
            return new Team { Id = Guid.NewGuid(), SeasonId = request.SeasonId, Player1Id = request.Player1Id, Player2Id = request.Player2Id };
        }

        public static TeamResponse MapToResponse(this Team team)
        {
            return new TeamResponse
            {
                Id = team.Id,
                SeasonId = team.SeasonId,
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
            return new Team { Id = id, SeasonId = request.SeasonId, Player1Id = request.Player1Id, Player2Id = request.Player2Id };
        }

        // Player mapping

        public static Player MapToPlayer(this PlayerCreateRequest request)
        {

            return new Player { Id = Guid.NewGuid(), UserId = request.UserId, Name = request.Name, Sex = request.Sex, SeasonId = request.SeasonId };

        }

        public static PlayerResponse MapToResponse(this Player player)
        {
            return new PlayerResponse
            {
                Id = player.Id,
                Name = player.Name,
                Sex = player.Sex,
                UserId = player.UserId,
                SeasonId = player.SeasonId
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
            return new Player { Id = id, UserId = request.UserId, Name = request.Name, Sex = request.Sex, SeasonId = request.SeasonId };

        }

        // Match mapping

        public static Match MapToMatch(this MatchCreateRequest request)
        {

            return new Match { Id = Guid.NewGuid(), SeasonId = request.SeasonId, MatchDate = request.MatchDate, Team1Id = request.Team1Id, Team2Id = request.Team2Id };

        }

        public static MatchResponse MapToResponse(this Match match)
        {
            return new MatchResponse
            {
                Id = match.Id,
                SeasonId = match.SeasonId,
                MatchDate = match.MatchDate,
                Team1Id = match.Team1Id,
                Team2Id = match.Team2Id,
                Teams = match.Teams.Select(MapToResponse).ToList()
                
            };
        }

        public static MatchesResponse MapToResponse(this IEnumerable<Match> matches)
        {
            return new MatchesResponse
            {
                Items = matches.Select(MapToResponse).ToList()
            };
        }

        public static Match MapToMatch(this MatchUpdateRequest request, Guid id)
        {
            return new Match { Id = id, SeasonId = request.SeasonId, MatchDate = request.MatchDate, Team1Id = request.Team1Id, Team2Id = request.Team2Id };

        }

        // Season mapping

        public static Season MapToSeason(this SeasonCreateRequest request)
        {

            return new Season
            {
                Id = Guid.NewGuid(),
                StartDate = request.StartDate,
                AmountOfMatches = request.AmountOfMatches,
                DayOfWeek = request.DayOfWeek,
                Name = request.Name

            };


        }

        public static SeasonResponse MapToResponse(this Season season)
        {
            return new SeasonResponse
            {
                Id = season.Id,
                StartDate = season.StartDate,
                AmountOfMatches = season.AmountOfMatches,
                DayOfWeek = season.DayOfWeek,
                EndDate = season.EndDate,
                Name = season.Name,
                Matches = season.Matches.Select(MapToResponse).ToList()

            };
        }

        public static SeasonsResponse MapToResponse(this IEnumerable<Season> seasons)
        {
            return new SeasonsResponse
            {
                Items = seasons.Select(MapToResponse).ToList()
            };
        }

        public static Season MapToSeason(this SeasonUpdateRequest request, Guid id)
        {
            return new Season
            {
                Id = id,
                StartDate = request.StartDate,
                AmountOfMatches = request.AmountOfMatches,
                DayOfWeek = request.DayOfWeek,
                Name = request.Name
            };

        }















    }
}
