namespace Padel.Domain.Models
{
    public class Team
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid SeasonId { get; private set; }

        // Store Player IDs as needed
        public Guid Player1Id { get; private set; }
        public Guid Player2Id { get; private set; }

        // Maintain the relationship with the actual Player objects (if needed)
        public IReadOnlyList<Player> Players => _players.AsReadOnly();
        private List<Player> _players = new();

        // Constructor to initialize both player IDs and players
        public Team(Guid id, Guid seasonId, Guid player1Id, Guid player2Id)
        {
            Id = id;
            SeasonId = seasonId;
            Player1Id = player1Id;
            Player2Id = player2Id;
        }

        // Optional: Add or associate players (if needed later)
        public void AssociatePlayers(Player player1, Player player2)
        {
            if (player1.Id != Player1Id || player2.Id != Player2Id)
            {
                throw new InvalidOperationException("Player IDs must match the actual player objects.");
            }

            _players.Clear();
            _players.Add(player1);
            _players.Add(player2);
        }

        // Method to update the team players (by IDs)
        public void UpdateTeam(Guid player1Id, Guid player2Id)
        {
            Player1Id = player1Id;
            Player2Id = player2Id;
        }
    }
}
