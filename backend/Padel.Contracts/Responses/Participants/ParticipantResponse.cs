namespace Padel.Contracts.Responses.Participants;

public class ParticipantResponse

{
    public int Id { get; init; }              // Participant ID
    public Guid UserId { get; init; }          // Associated User ID (if applicable)
    public required string Name { get; init; }          // Participant's full name
    public required string Gender { get; init; }        // Participant's gender

    public int SeasonId { get; init; }        // Season ID

}
