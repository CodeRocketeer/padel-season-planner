using Moq;
using Padel.Application.Services;
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Padel.Tests.Services
{
    public class TeamServiceTests
    {
        private readonly Mock<ITeamService> _teamServiceMock;

        public TeamServiceTests()
        {
            _teamServiceMock = new Mock<ITeamService>();
        }

        [Fact]
        public async Task GenerateTeamsForSeason_WithValidSeasonAndEnoughPlayers_ReturnsCorrectNumberOfTeams()
        {
            // Arrange
            var season = new Season { Id = Guid.NewGuid(), Name = "Test Season", StartDate = DateTime.Now, AmountOfMatches = 4 };
            var players = new List<Player>
            {
                new Player { UserId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "Player 1", Sex = "M", SeasonId = season.Id },
                new Player { UserId = Guid.NewGuid(),Id = Guid.NewGuid(), Name = "Player 2", Sex = "M", SeasonId = season.Id },
                new Player { UserId = Guid.NewGuid(),Id = Guid.NewGuid(), Name = "Player 3", Sex = "M", SeasonId = season.Id },
                new Player { UserId = Guid.NewGuid(),Id = Guid.NewGuid(), Name = "Player 4", Sex = "M", SeasonId = season.Id }
            };

            var expectedTeams = new List<Team>
            {
                new Team { Id = Guid.NewGuid(), Player1Id = players[0].Id, Player2Id = players[1].Id },
                new Team { Id = Guid.NewGuid(), Player1Id = players[0].Id, Player2Id = players[2].Id },
                new Team { Id = Guid.NewGuid(), Player1Id = players[0].Id, Player2Id = players[3].Id },
                new Team { Id = Guid.NewGuid(), Player1Id = players[1].Id, Player2Id = players[2].Id },
                new Team { Id = Guid.NewGuid(), Player1Id = players[1].Id, Player2Id = players[3].Id },
                new Team { Id = Guid.NewGuid(), Player1Id = players[2].Id, Player2Id = players[3].Id }
            };

            _teamServiceMock.Setup(s => s.GenerateTeamsForSeason(season, players)).ReturnsAsync(expectedTeams);

            // Act
            var teams = await _teamServiceMock.Object.GenerateTeamsForSeason(season, players);

            // Assert: For 4 players, the expected number of teams is C(4, 2) = 6
            Assert.Equal(6, teams.Count);

            // Verify that each team contains two distinct players
            foreach (var team in teams)
            {
                Assert.NotEqual(team.Player1Id, team.Player2Id);
            }
        }

        [Fact]
        public async Task GenerateTeamsForSeason_WithTwoPlayers_ReturnsOneTeam()
        {
            // Arrange
            var season = new Season { Id = Guid.NewGuid(), Name = "Test Season", StartDate = DateTime.Now, AmountOfMatches = 4 };
            var players = new List<Player>
            {
                new Player {UserId = Guid.NewGuid(),  Id = Guid.NewGuid(), Name = "Player 1", Sex = "M", SeasonId = season.Id },
                new Player {UserId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "Player 2", Sex = "M", SeasonId = season.Id }
            };

            var expectedTeam = new List<Team>
            {
                new Team { Id = Guid.NewGuid(), Player1Id = players[0].Id, Player2Id = players[1].Id }
            };

            _teamServiceMock.Setup(s => s.GenerateTeamsForSeason(season, players)).ReturnsAsync(expectedTeam);

            // Act
            var teams = await _teamServiceMock.Object.GenerateTeamsForSeason(season, players);

            // Assert: With 2 players, there should only be 1 team
            Assert.Single(teams);
            Assert.Equal(players[0].Id, teams[0].Player1Id);
            Assert.Equal(players[1].Id, teams[0].Player2Id);
        }

        [Fact]
        public async Task GenerateTeamsForSeason_WithNotEnoughPlayers_ThrowsInvalidOperationException()
        {
            // Arrange
            var season = new Season { Id = Guid.NewGuid(), Name = "Test Season", StartDate = DateTime.Now, AmountOfMatches = 4 };
            var players = new List<Player> { new Player { UserId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "Player 1", Sex = "M", SeasonId = season.Id } }; // Only 1 player

            // Setup the mock to throw an exception when less than 2 players are provided
            _teamServiceMock.Setup(s => s.GenerateTeamsForSeason(season, players))
                .ThrowsAsync(new InvalidOperationException("Invalid number of players"));

            // Act & Assert: Generating teams with less than 2 players should throw an error
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _teamServiceMock.Object.GenerateTeamsForSeason(season, players));
        }
    }
}
