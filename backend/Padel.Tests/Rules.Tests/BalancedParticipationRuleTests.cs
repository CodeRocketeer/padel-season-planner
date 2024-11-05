using Padel.Application.Rules;
using Padel.Domain.Models;

namespace Padel.Tests.Rules
{
    public class BalancedParticipationRuleTests
    {
        private BalancedParticipationRule _rule;

        public BalancedParticipationRuleTests()
        {
            _rule = new BalancedParticipationRule();
        }

        [Fact]
        public void Validate_Should_ReturnZero_When_MatchSetIsEmpty()
        {
            // Arrange
            var matchSet = new List<Match>();
            var players = new List<Player>();

            // Act
            int result = _rule.Validate(matchSet, players);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Validate_Should_ReturnZero_When_AllPlayersParticipateEqually()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player(Gender.Male, "Player 1", Guid.NewGuid()),
                new Player(Gender.Male, "Player 2", Guid.NewGuid()),
                new Player(Gender.Female, "Player 3", Guid.NewGuid()),
                new Player(Gender.Female, "Player 4", Guid.NewGuid())
            };

            var teams = new List<Team>
            {
                new Team(players[0], players[1]) { Id = 1 },
                new Team(players[2], players[3]) { Id = 2 }
            };

            var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1]),
                new Match(teams[0], teams[1])
            };

            // Act
            int result = _rule.Validate(matchSet, players);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Validate_Should_ReturnOneFaultPoint_When_OnePlayerParticipatesUnequally()
        {
            var players = new List<Player>
            {
                new Player(Gender.Male, "Player 1", Guid.NewGuid()),
                new Player(Gender.Male, "Player 2", Guid.NewGuid()),
                new Player(Gender.Female, "Player 3", Guid.NewGuid()),
                new Player(Gender.Female, "Player 4", Guid.NewGuid()),
                new Player(Gender.Female, "Player 5", Guid.NewGuid()),
                new Player(Gender.Male, "Player 6", Guid.NewGuid()),
                new Player(Gender.Male, "Player 7", Guid.NewGuid()),

            };

            var teams = new List<Team>
            {
                new Team(players[0], players[1]) { Id = 1 },
                new Team(players[2], players[3]) { Id = 2 },
                new Team(players[4], players[5]) { Id = 3 },
                new Team(players[6], players[0]) { Id = 4 }
            };

            var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1]),
                new Match(teams[2], teams[3])
            };

            // Act
            int result = _rule.Validate(matchSet, players);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Validate_Should_ReturnTwoFaultPoints_When_TwoPlayersParticipateUnequally()
        {
            var players = new List<Player> {
                new Player(Gender.Male, "Player 1", Guid.NewGuid()),
                new Player(Gender.Male, "Player 2", Guid.NewGuid()),
                new Player(Gender.Female, "Player 3", Guid.NewGuid()),
                new Player(Gender.Female, "Player 4", Guid.NewGuid()),
                new Player(Gender.Female, "Player 5", Guid.NewGuid()),
                new Player(Gender.Male, "Player 6", Guid.NewGuid()),
                new Player(Gender.Male, "Player 7", Guid.NewGuid()),

            };

            var teams = new List<Team> {

                new Team(players[0], players[1]) { Id = 1 },
                new Team(players[2], players[3]) { Id = 2 },
                new Team(players[4], players[5]) { Id = 3 },
                new Team(players[1], players[0]) { Id = 4 }
            };

            var matchSet = new List<Match> {
                new Match(teams[0], teams[1]),
                new Match(teams[2], teams[3])
            };

            // Act
            int result = _rule.Validate(matchSet, players);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Validate_Should_ReturnThreeFaultsPoints_When_OnePlayerParticipatesTwoTimes_AndOtherPlayerParticipatesThreeTimes()
        {
            var players = new List<Player>
            {
                new Player(Gender.Male, "Player 1", Guid.NewGuid()),
                new Player(Gender.Male, "Player 2", Guid.NewGuid()),
                new Player(Gender.Female, "Player 3", Guid.NewGuid()),
                new Player(Gender.Female, "Player 4", Guid.NewGuid()),
                new Player(Gender.Female, "Player 5", Guid.NewGuid()),
                new Player(Gender.Male, "Player 6", Guid.NewGuid()),
                new Player(Gender.Male, "Player 7", Guid.NewGuid()),
                new Player(Gender.Male, "Player 8", Guid.NewGuid()),
                new Player(Gender.Male, "Player 9", Guid.NewGuid()),
            };

            var teams = new List<Team>
            {
                new Team(players[0], players[1]) { Id = 1 },
                new Team(players[2], players[3]) { Id = 2 },
                new Team(players[4], players[5]) { Id = 3 },
                new Team(players[1], players[7]) { Id = 4 },
                new Team(players[8], players[0]) { Id = 5 },
                new Team(players[1], players[6]) { Id = 6 },
            };

            var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1]),
                new Match(teams[2], teams[3]),
                new Match(teams[4], teams[5])
            };

            // Act
            int result = _rule.Validate(matchSet, players);

            // Assert
            Assert.Equal(3, result);
        }

    }
}
