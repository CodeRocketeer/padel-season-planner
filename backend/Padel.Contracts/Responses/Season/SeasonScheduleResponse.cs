using PadelContracts.Responses.Match;

namespace PadelContracts.Responses.Season
{
    public class SeasonScheduleResponse
    {
        public required int Id { get; init; }
        public int AmountOfMatches { get; init; }
        public DateTime StartDate { get; init; }
        public string? Title { get; init; }
        public DayOfWeek DayOfWeek { get; init; }
        public List<MatchResponse> Matches { get; init; } = new List<MatchResponse>();
    }
}
