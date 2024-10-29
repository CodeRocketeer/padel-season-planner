namespace PadelContracts.Requests.Participant
{
    public class GetAllParticipantsRequest
    {
        public Guid? UserId { get; init; }
        public  int? SeasonId { get; init; }
    }
}
