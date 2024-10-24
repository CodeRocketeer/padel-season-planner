using Padel.Application.Models;
using Padel.Application.Rules;
using Match = Padel.Application.Models.Match;



namespace Padel.Tests
{
    public class UniqueTeamRuleTests
    {

        [Fact]
        public void Validate_Returns0_WhenTeamsAreUnique()
        {
            // Arrange
            var team1 = new Team(Guid.NewGuid(), Guid.NewGuid()) { Id = Guid.NewGuid() };
            var team2 = new Team(Guid.NewGuid(), Guid.NewGuid()) { Id = Guid.NewGuid() };

            // Create a match with the two teams
            var match = new Match(team1, team2);

            // Create the rule instance to test
            var rule = new UniqueTeamRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            Assert.Equal(0, result);
        }

        [Fact]
        public void Validate_Returns100_WhenTeamsAreNotUnique()
        {
            // Arrange
            var sameTeamId = Guid.NewGuid();
            var team1 = new Team(Guid.NewGuid(), Guid.NewGuid()) { Id = sameTeamId };
            var team2 = new Team(Guid.NewGuid(), Guid.NewGuid()) { Id = sameTeamId };

            // Create a match with the two teams
            var match = new Match(team1, team2);

            // Create the rule instance to test
            var rule = new UniqueTeamRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            Assert.Equal(100, result);
        }
    }
}