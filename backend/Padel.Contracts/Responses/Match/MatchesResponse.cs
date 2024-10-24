namespace PadelContracts.Responses.Match
{
    public class MatchesResponse
    {
        public required IEnumerable<MatchResponse> Items { get; init; } = Enumerable.Empty<MatchResponse>();

    }
}


