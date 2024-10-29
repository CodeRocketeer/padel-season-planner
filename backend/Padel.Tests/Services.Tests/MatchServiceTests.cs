using FluentAssertions;
using Padel.Application.Models;
using Padel.Application.Services;

namespace Padel.Tests.Services.Tests
{
    public class MatchServiceTests
    {

        private readonly MatchService _matchService;
        private readonly TeamService _teamService;

        public MatchServiceTests()
        {
            _matchService = new MatchService();
            _teamService = new TeamService();
        }

        [Fact]
        public async Task GenerateAllTeamCombinations_ValidPlayers_ReturnsAllCombinations()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player(Gender.Male, "Player 1", Guid.NewGuid()),
                new Player(Gender.Female, "Player 2", Guid.NewGuid()),
                new Player(Gender.Male, "Player 3", Guid.NewGuid()),
                new Player(Gender.Female, "Player 4", Guid.NewGuid())
            };

            // Act
            var teams = await _teamService.GenerateAllTeamCombinations(players);
            var matches = await _matchService.GenerateAllMatchCombinations(teams.ToList());

            // Assert
            teams.Should().HaveCount(6);
            matches.Should().HaveCount(3);
        }

        [Fact]
        public async Task GenerateAllMatchCombinations_EmptyTeams_ThrowsArgumentException()
        {
            // Arrange
            var teams = new List<Team>();

            // Act
            await FluentActions
              .Invoking(() => _matchService.GenerateAllMatchCombinations(teams))
              .Should()
              .ThrowAsync<ArgumentException>()
              .WithMessage("At least two teams are required to form matches.");
        }

        [Fact]
        public async Task GenerateAllMatchCombinations_OneTeam_ThrowsArgumentException()
        {
            // Arrange
            var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
            var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());

            var teams = new List<Team>
            {
                new Team(player1, player2)
            };

            // Act
            await FluentActions
               .Invoking(() => _matchService.GenerateAllMatchCombinations(teams))
               .Should()
               .ThrowAsync<ArgumentException>()
               .WithMessage("At least two teams are required to form matches.");
        }

        [Fact]
        public async Task GenerateAllMatchCombinations_CommonPlayers_CreatesNoMatches()
        {
            // Arrange
            var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
            var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
            var commonPlayer = new Player(Gender.Male, "Common Player", Guid.NewGuid());

            var teams = new List<Team>
            {
                new Team(player1, commonPlayer),
                new Team(player2, commonPlayer)
            };

            // Act
            var matches = await _matchService.GenerateAllMatchCombinations(teams);

            // Assert
            matches.Should().BeEmpty(); // No matches should be created due to common players
        }

      






    }
}
