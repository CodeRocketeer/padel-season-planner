namespace Padel.Contracts.Requests.Team
{
    public class TeamCreateRequest
    {


        public Guid SeasonId { get; init; }
        public Guid Player1Id { get; init; }
        public Guid Player2Id { get; init; }


    }
}
