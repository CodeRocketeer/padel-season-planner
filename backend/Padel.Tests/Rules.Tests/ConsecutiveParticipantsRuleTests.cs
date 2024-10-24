
using Padel.Application.Models;
using Padel.Application.Rules;
using Match = Padel.Application.Models.Match;



namespace Padel.Tests
{
    public class ConsecutiveParticipantsRuleTests
    {
        [Fact]
        public void Validate_Returns100_WhenParticipantsPlayedInConsecutiveMatch()
        {
            // Arrange
            var teams = Enumerable.Range(1, 4)
                 .Select((i, index) => new { Participant = new Participant { Id = Guid.NewGuid(), Name = $"Player {i}" }, Index = index })
                 .GroupBy(x => x.Index / 2)
                 .Select(g => new Team(g.First().Participant, g.Last().Participant))
                 .ToArray();


            var lastMatch = new Match(teams[0], teams[1])
            {
                MatchDate = DateTime.Today.AddDays(-7) // Last match was exactly one week ago
            };

            var currentMatch = new Match(teams[0], teams[1])
            {
                MatchDate = DateTime.Today // Current match is today
            };

            var rule = new ConsecutiveParticipantsRule();

            // Act
            var result = rule.Validate(currentMatch, new List<Match> { lastMatch }, new List<Participant>());

            // Assert
            Assert.Equal(100, result); // Expecting a 100% fault due to consecutive matches
        }

        [Fact]
        public void Validate_Returns0_WhenNonOfTheParticipantsPlayedInConsecutiveMatch()
        {
            // Arrange
            var teams = Enumerable.Range(1, 8)
                .Select((i, index) => new { Participant = new Participant { Id = Guid.NewGuid(), Name = $"Player {i}" }, Index = index })
                .GroupBy(x => x.Index / 2)
                .Select(g => new Team(g.First().Participant, g.Last().Participant))
                .ToArray();


            var lastMatch = new Match(teams[0], teams[1])
            {
                MatchDate = DateTime.Today.AddDays(-7) // Last match was exactly one week ago
            };

            var currentMatch = new Match(teams[2], teams[3])
            {
                MatchDate = DateTime.Today // Current match is today
            };

            var rule = new ConsecutiveParticipantsRule();

            // Act
            var result = rule.Validate(currentMatch, new List<Match> { lastMatch }, new List<Participant>());

            // Assert
            Assert.Equal(0, result); // Expecting a 100% fault due to consecutive matches
        }
    }
}