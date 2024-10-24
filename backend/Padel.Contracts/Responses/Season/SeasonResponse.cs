using PadelContracts.Responses.Match;

namespace Padel.Contracts.Responses.Season;

public class SeasonResponse
{
    public required Guid Id { get; init; }
    public int AmountOfMatches { get; init; }
    public DateTime StartDate { get; init; }

    public bool? UserParticipates { get; init; }
    public string? Title { get; init; }
    public int DayOfWeek { get; init; }

    public List<SimplifiedMatchResponse>? Matches { get; init; } // Nullable list of matches
}
