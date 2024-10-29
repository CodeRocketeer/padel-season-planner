using Padel.Application.Database.Entities;

namespace Padel.Application.Models
{
    public class Team
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        // Constructor allowing optional participants
        public Team(Player player1, Player player2)
        {
            Player1 = player1 ?? throw new ArgumentNullException(nameof(player1));
            Player2 = player2 ?? throw new ArgumentNullException(nameof(player2));

            if (player1.UserId == player2.UserId)
            {
                throw new ArgumentException("Player1 and Player2 cannot have the same user ID.", nameof(player1));
            }
        }

        // Method to convert a Team model to a TeamEntity
        public static TeamEntity ToEntity(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            return new TeamEntity
            {
                MatchId = team.MatchId,
                Id = team.Id != 0 ? team.Id : 0,
                Players = new List<PlayerEntity>
                {
                    new PlayerEntity { UserId = team.Player1.UserId, Name = team.Player1.Name, Gender = team.Player1.Gender },
                    new PlayerEntity { UserId = team.Player2.UserId, Name = team.Player2.Name, Gender = team.Player2.Gender }
                }
            };
        }

        // Method to convert a TeamEntity to a Team model
        public static Team FromEntity(TeamEntity teamEntity)
        {
            if (teamEntity == null)
                throw new ArgumentNullException(nameof(teamEntity));

            var players = teamEntity.Players?.ToList() ?? new List<PlayerEntity>();
            if (players.Count != 2)
            {
                throw new ArgumentException("Team must have exactly two players.", nameof(teamEntity.Players));
            }

            var player1 = new Player(players[0].Gender, players[0].Name, players[0].UserId) { Id = players[0].Id };
            var player2 = new Player(players[1].Gender, players[1].Name, players[1].UserId) { Id = players[1].Id };

            return new Team(player1, player2)
            {
                Id = teamEntity.Id,
                MatchId = teamEntity.MatchId
            };
        }

    }

}
