using Padel.Contracts.Responses.Player;

namespace Padel.Contracts.Responses.Team
{


    public class TeamResponse
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Guid Player1Id { get; set; }

        public Guid Player2Id { get; set; }
    }




}
