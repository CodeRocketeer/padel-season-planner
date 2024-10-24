namespace Padel.Contracts.Responses.Participants;

public class ParticipantResponse

{
    public Guid Id { get; init; }              // Participant ID
    public Guid UserId { get; init; }          // Associated User ID (if applicable)
    public required string Name { get; init; }          // Participant's full name
    public required string Gender { get; init; }        // Participant's gender

    public Guid SeasonId { get; init; }        // Season ID

}
