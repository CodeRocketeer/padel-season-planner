using Padel.Domain.Models;
namespace Padel.Tests.Models
{
    public class TeamModelTests
    {
        [Fact]
        public void Team_Should_Have_RequiredProperties()
        {
            // Arrange
            var team = new Team
            {
                Id = Guid.NewGuid(),
                SeasonId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };

            // Act & Assert
            Assert.NotEqual(Guid.Empty, team.Id);
            Assert.NotEqual(Guid.Empty, team.SeasonId);
            Assert.NotEqual(Guid.Empty, team.Player1Id);
            Assert.NotEqual(Guid.Empty, team.Player2Id);
        }

        [Fact]
        public void Team_Should_AddPlayers()
        {
            // Arrange
            var team = new Team
            {
                Id = Guid.NewGuid(),
                SeasonId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };

            var player1 = new Player{ Id = team.Player1Id, Name = "Player 1", Sex = "M", SeasonId = team.SeasonId, UserId = Guid.NewGuid() };
            var player2 = new Player { Id = team.Player2Id, Name = "Player 2", Sex = "F", SeasonId = team.SeasonId, UserId = Guid.NewGuid() };

            // Act
            team.Players.Add(player1);
            team.Players.Add(player2);

            // Assert
            Assert.Equal(2, team.Players.Count);
            Assert.Contains(player1, team.Players);
            Assert.Contains(player2, team.Players);
        }


    }
}
