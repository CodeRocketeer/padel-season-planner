using Padel.Domain.Models;

namespace Padel.Tests.Models
{
    public class MatchModelTests
    {
        [Fact]
        public void Match_Should_Have_RequiredProperties()
        {
            // Arrange
            var match = new Match
            {
                Id = Guid.NewGuid(),
                SeasonId = Guid.NewGuid(),
                MatchDate = DateTime.Now,
                Team1Id = Guid.NewGuid(),
                Team2Id = Guid.NewGuid()
            };

            // Act & Assert
            Assert.NotEqual(Guid.Empty, match.Id);
            Assert.NotEqual(Guid.Empty, match.SeasonId);
            Assert.NotEqual(Guid.Empty, match.Team1Id);
            Assert.NotEqual(Guid.Empty, match.Team2Id);
            Assert.Equal(0, match.Teams.Count); // Default is an empty list
        }

        [Fact]
        public void Match_Should_Add_Teams()
        {
            // Arrange
            var match = new Match
            {
                Id = Guid.NewGuid(),
                SeasonId = Guid.NewGuid(),
                MatchDate = DateTime.Now,
                Team1Id = Guid.NewGuid(),
                Team2Id = Guid.NewGuid()
            };

            var team1 = new Team { Id = match.Team1Id };
            var team2 = new Team { Id = match.Team2Id };

            // Act
            match.Teams.Add(team1);
            match.Teams.Add(team2);

            // Assert
            Assert.Equal(2, match.Teams.Count);
            Assert.Contains(team1, match.Teams);
            Assert.Contains(team2, match.Teams);
        }


    }
}
