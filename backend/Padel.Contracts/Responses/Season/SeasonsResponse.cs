namespace Padel.Contracts.Responses.Season;

public class SeasonsResponse
{
    public required IEnumerable<SeasonResponse> Items { get; init; } = Enumerable.Empty<SeasonResponse>();
}
