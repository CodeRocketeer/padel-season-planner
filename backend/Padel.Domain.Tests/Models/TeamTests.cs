using Padel.Domain.Models;

namespace Padel.Domain.Tests.Models
{
    public class TeamTests
    {
        // Test 1: Constructor should create a valid Team
        [Fact]
        public void Constructor_ShouldCreateValidTeam()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var player1Id = Guid.NewGuid();
            var player2Id = Guid.NewGuid();

            // Act
            var team = new Team(Guid.NewGuid(), matchId, player1Id, player2Id);

            // Assert
            Assert.NotNull(team);
            Assert.NotEqual(Guid.Empty, team.Id);
            Assert.Equal(matchId, team.MatchId);
            Assert.Equal(player1Id, team.Player1Id);
            Assert.Equal(player2Id, team.Player2Id);
        }


        // Test 3: Constructor should throw exception for empty Match ID
        [Fact]
        public void Constructor_ShouldThrowException_WhenMatchIdIsEmpty()
        {
            // Arrange
            var player1Id = Guid.NewGuid();
            var player2Id = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Team(Guid.NewGuid(), Guid.Empty, player1Id, player2Id));
            Assert.Equal("Match ID must not be empty.", exception.Message);
        }

        // Test 4: Constructor should throw exception for duplicate Player IDs
        [Fact]
        public void Constructor_ShouldThrowException_WhenPlayerIdsAreTheSame()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var playerId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new Team(Guid.NewGuid(), matchId, playerId, playerId));
            Assert.Equal("Player 1 and Player 2 cannot be the same.", exception.Message);
        }

        // Test 5: UpdateTeam should change Player IDs
        [Fact]
        public void UpdateTeam_ShouldUpdatePlayerIdsSuccessfully()
        {
            // Arrange
            var team = new Team(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var newPlayer1Id = Guid.NewGuid();
            var newPlayer2Id = Guid.NewGuid();

            // Act
            team.UpdateTeam(newPlayer1Id, newPlayer2Id);

            // Assert
            Assert.Equal(newPlayer1Id, team.Player1Id);
            Assert.Equal(newPlayer2Id, team.Player2Id);
        }

        // Test 6: UpdateTeam should throw exception for duplicate Player IDs
        [Fact]
        public void UpdateTeam_ShouldThrowException_WhenPlayerIdsAreTheSame()
        {
            // Arrange
            var team = new Team(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var playerId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => team.UpdateTeam(playerId, playerId));
            Assert.Equal("Player 1 and Player 2 cannot be the same.", exception.Message);
        }
    }
}
