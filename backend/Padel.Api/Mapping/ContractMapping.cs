using Padel.Contracts.Requests.Season;
using Padel.Contracts.Responses.Season;
using Padel.Domain.Models;
using PadelContracts.Requests.Player;

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




}
