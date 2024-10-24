

using Padel.Application.Models;
using Padel.Contracts.Requests.Season;
using Padel.Contracts.Responses.Participants;
using Padel.Contracts.Responses.Season;
using PadelContracts.Requests.Participant;
using PadelContracts.Responses.Match;
using PadelContracts.Responses.Team;

namespace Padel.Api.Mapping;

public static class ContractMapping
{
    public static Season MapToSeason(this CreateSeasonRequest request)
    {
        return new Season
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            AmountOfMatches = request.AmountOfMatches,
            StartDate = request.StartDate,
            DayOfWeek = request.DayOfWeek
        };

    }

    public static Season MapToSeason(this UpdateSeasonRequest request, Guid id)
    {
        return new Season
        {
            Id = id,
            Title = request.Title,
            AmountOfMatches = request.AmountOfMatches,
            StartDate = request.StartDate,
            DayOfWeek = request.DayOfWeek
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
            UserParticipates = season.UserParticipates,
            Matches = season.Matches?.Select(match => match.MapToSimplifiedResponse()).ToList() // Use simplified response
        };
    }

    public static SeasonsResponse MapToResponse(this IEnumerable<Season> seasons)
    {
        return new SeasonsResponse
        {
            Items = seasons.Select(MapToResponse)
        };
    }


    public static Participant MapToParticipant(this CreateParticipantRequest request, Guid? userId, Guid seasonId)
    {
        return new Participant
        {
            Id = Guid.NewGuid(),
            UserId = userId!.Value,
            SeasonId = seasonId,
            Name = request.Name,
            Gender = request.Gender,
        };

    }

    public static ParticipantsResponse MapToResponse(this IEnumerable<Participant> participants)
    {
        return new ParticipantsResponse
        {
            Items = participants.Select(MapToResponse)
        };
    }

    public static ParticipantResponse MapToResponse(this Participant participant)
    {
        return new ParticipantResponse
        {
            Id = participant.Id,
            Name = participant.Name,
            Gender = participant.Gender,
            UserId = participant.UserId,
            SeasonId = participant.SeasonId

        };
    }

    public static GetAllParticipantsOptions MapToOptions(this GetAllParticipantsRequest request)
    {
        return new GetAllParticipantsOptions
        {
            SeasonId = request.SeasonId,
            UserId = request.UserId
          
        };
    }

    public static MatchResponse MapToResponse(this Match match)
    {
        return new MatchResponse
        {
            Id = match.Id,
            MatchDate = match.MatchDate,
            SeasonId = match.SeasonId,
            Team1 = match.Team1.MapToResponse(), // Full team response
            Team2 = match.Team2.MapToResponse(), // Full team response
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
            SeasonId = team.SeasonId,
            Participant1 = team.Participant1?.MapToResponse(),
            Participant2 = team.Participant2?.MapToResponse()
        };
    }

    public static MatchTeamResponse MapToMatchResponse(this Guid teamId)
    {
        return new MatchTeamResponse
        {
            Id = teamId
        };
    }


    public static SimplifiedMatchResponse MapToSimplifiedResponse(this Match match)
    {
        return new SimplifiedMatchResponse
        {
            Id = match.Id,
            MatchDate = match.MatchDate,
            SeasonId = match.SeasonId,
            Team1 = match.Team1.Id.MapToMatchResponse(), // Use MatchTeamResponse
            Team2 = match.Team2.Id.MapToMatchResponse(), // Use MatchTeamResponse
        };
    }








}
