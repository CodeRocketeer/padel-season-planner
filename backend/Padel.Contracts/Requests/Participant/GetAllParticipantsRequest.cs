namespace PadelContracts.Requests.Participant
{
    public class GetAllParticipantsRequest
    {
        public Guid? UserId { get; init; }
        public  Guid? SeasonId { get; init; }
    }
}
