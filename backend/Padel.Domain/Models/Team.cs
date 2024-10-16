namespace Padel.Domain.Models
{
    public partial class Team
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }

        public Guid Player1Id { get; set; }

        public Guid Player2Id { get; set; }

        public List<Player> Players { get; set; } = new();

    }
}

