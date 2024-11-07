using Padel.Contracts.Requests.Season;
using Padel.Contracts.Responses.Season;
using Padel.Domain.Models;
using Padel.Shared.Options;
using PadelContracts.Requests.Player;
using PadelContracts.Responses.Match;
using PadelContracts.Responses.Player;
using PadelContracts.Responses.Season;
using PadelContracts.Responses.Team;

namespace Padel.Api.Mapping;

public static class ContractMapping
{
    public static Season MapToSeason(this CreateSeasonRequest request)
    {
        return new Season(request.AmountOfMatches, request.StartDate, request.Title, request.DayOfWeek);


    }

    public static Season MapToSeason(this UpdateSeasonRequest request, int id)
    {
        return new Season(request.AmountOfMatches, request.StartDate, request.Title, request.DayOfWeek)
        {
            Id = id
        };
    }

    public static SeasonResponse MapToResponse(this Season season)
    {
        return new SeasonResponse
        {
            Id = season.Id,
            AmountOfMatches = season.AmountOfMatches,
            StartDate = season.StartDate,
            Title = season.Title,
            DayOfWeek = season.DayOfWeek,
        };
    }

    public static SeasonScheduleResponse MapToSeasonScheduleResponse(this Season season)
    {
        return new SeasonScheduleResponse
        {
            Id = season.Id,
            AmountOfMatches = season.AmountOfMatches,
            StartDate = season.StartDate,
            Title = season.Title,
            DayOfWeek = season.DayOfWeek,
            Matches = season.Matches.Select(MapToResponse).ToList()
        };
    }

    public static SeasonsResponse MapToResponse(this IEnumerable<Season> seasons)
    {
        return new SeasonsResponse
        {
            Items = seasons.Select(MapToResponse)
        };
    }

    public static Player MapToPlayer(this CreatePlayerRequest request, Guid userId)
    {
        return new Player(request.Gender, request.Name, userId);

    }

    public static GetAllPlayersOptions MapToOptions(this GetAllPlayersRequest request)
    {
        return new GetAllPlayersOptions
        {
            SeasonId = request.SeasonId,
        };
    }

    public static PlayerResponse MapToResponse(this Player player)
    {
        return new PlayerResponse
        {
            UserId = player.UserId,
            Name = player.Name,
            Gender = player.Gender,
        };
    }

    public static PlayersResponse MapToResponse(this IEnumerable<Player> players)
    {
        return new PlayersResponse
        {
            Items = players.Select(MapToResponse)
        };
    }

    public static MatchResponse MapToResponse(this Match match)
    {
        return new MatchResponse
        {
            Id = match.Id,
            MatchDate = match.MatchDate,
            Team1 = match.Team1.MapToResponse(),
            Team2 = match.Team2.MapToResponse()
        };
    }


    public static MatchesResponse MapToResponse(this IEnumerable<Match> matches)
    {
        return new MatchesResponse
        {
            Items = matches.Select(MapToResponse)
        };
    }

    public static TeamResponse MapToResponse(this Team team)
    {
        return new TeamResponse
        {
            Id = team.Id,
            Player1 = team.Player1.MapToResponse(),
            Player2 = team.Player2.MapToResponse()

        };
    }

}
