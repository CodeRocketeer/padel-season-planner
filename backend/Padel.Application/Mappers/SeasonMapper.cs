using Padel.Application.Database.Entities;
using Padel.Domain.Models;

namespace Padel.Application.Mappers;

public static class SeasonMapper
{
    public static SeasonEntity ToEntity(this Season season)
    {
        if (season == null)
        {
            throw new ArgumentNullException(nameof(season), "Season cannot be null.");
        }

        return new SeasonEntity
        {
            Id = season.Id != 0 ? season.Id : 0,
            AmountOfMatches = season.AmountOfMatches,
            DayOfWeek = season.DayOfWeek,
            StartDate = DateTime.SpecifyKind(season.StartDate, DateTimeKind.Utc), // Ensure UTC kind
            Title = season.Title
        };
    }

    public static Season FromEntity(this SeasonEntity seasonEntity)
    {
        if (seasonEntity == null)
        {
            throw new ArgumentNullException(nameof(seasonEntity), "Season entity cannot be null.");
        }

        return new Season(seasonEntity.AmountOfMatches, seasonEntity.StartDate, seasonEntity.Title, seasonEntity.DayOfWeek)
        {
            Id = seasonEntity.Id,
                        

        };
    }
}