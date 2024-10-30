using Padel.Infrastructure.Entities;
using Padel.Domain.Models;

namespace Padel.Application.Mappers;

public static class MatchMapper
{
    // Convert Match model to MatchEntity
    public static MatchEntity ToEntity(this Match match)
    {
        if (match == null)
            throw new ArgumentNullException(nameof(match));

        return new MatchEntity
        {
            Id = match.Id != 0 ? match.Id : 0,
            SeasonId = match.SeasonId,
            MatchDate = match.MatchDate
            // Teams can be handled separately if needed
        };
    }

    // Convert MatchEntity back to Match model
    public static Match FromEntity(this MatchEntity matchEntity, Team team1, Team team2)
    {
        if (matchEntity == null)
            throw new ArgumentNullException(nameof(matchEntity));

        if (team1 == null || team2 == null)
            throw new ArgumentNullException("Teams cannot be null");

        return new Match(team1, team2) // Corrected to use team2
        {
            Id = matchEntity.Id,
            SeasonId = matchEntity.SeasonId,
            MatchDate = matchEntity.MatchDate
        };
    }
}