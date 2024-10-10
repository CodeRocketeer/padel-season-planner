using Padel.Domain.Models;


namespace Padel.Application.Models
{
    public class Team
    {
        public Guid Id { get; set; }

        public required Guid MatchId { get; init; }

        public required Guid Player1Id { get; init; }

        public required Guid Player2Id { get; init; }
    }
}