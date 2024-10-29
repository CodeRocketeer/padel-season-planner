namespace Padel.Contracts.Responses.Participants;

public class ParticipantsResponse
{
    public required IEnumerable<ParticipantResponse> Items { get; init; } = Enumerable.Empty<ParticipantResponse>();
}
