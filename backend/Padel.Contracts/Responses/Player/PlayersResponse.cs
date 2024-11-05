using PadelContracts.Responses.Match;

namespace PadelContracts.Responses.Player;

public class PlayersResponse
{
    public required IEnumerable<PlayerResponse> Items { get; init; } = Enumerable.Empty<PlayerResponse>();
}
