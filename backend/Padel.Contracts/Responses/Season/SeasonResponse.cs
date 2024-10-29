using PadelContracts.Responses.Match;

namespace Padel.Contracts.Responses.Season;

public class SeasonResponse
{
    public required int Id { get; init; }
    public int AmountOfMatches { get; init; }
    public DateTime StartDate { get; init; }
    public string? Title { get; init; }
    public DayOfWeek DayOfWeek { get; init; }

   
}
