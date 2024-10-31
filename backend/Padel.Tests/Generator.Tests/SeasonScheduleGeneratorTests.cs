using FluentAssertions;
using Padel.Application.Generators;
using Padel.Application.Rules;
using Padel.Domain.Models;

namespace Padel.Tests.Generator.Tests
{



    public class SeasonScheduleGeneratorTests
    {


        private readonly TeamGenerator _teamGenerator;
        private readonly MatchGenerator _matchGenerator;
        private readonly RuleSet _ruleSet;
        private readonly SeasonScheduleGenerator _seasonScheduleGenerator;

        public SeasonScheduleGeneratorTests()
        {
            // Initialize actual instances of each dependency
            _teamGenerator = new TeamGenerator();
            _matchGenerator = new MatchGenerator();

            // Define simple rule(s) for RuleSet
            var rules = new List<IRule>
        {
            new ConsecutiveParticipantsRule()
        };
            _ruleSet = new RuleSet(rules);

            _seasonScheduleGenerator = new SeasonScheduleGenerator(
                _teamGenerator,
                _matchGenerator,
                _ruleSet);
        }

        [Fact]
        public async Task GenerateSeasonSchedule_ReturnsSeasonWithMatches()
        {
            // Arrange
            var season = new Season(4, DateTime.Now, "test season", DayOfWeek.Monday);
            var players = new List<Player>
            {
                new Player(Gender.Male, "Player 1", Guid.NewGuid()),
                new Player(Gender.Female, "Player 2", Guid.NewGuid()),
                new Player(Gender.Male, "Player 3", Guid.NewGuid()),
                new Player(Gender.Female, "Player 4", Guid.NewGuid())
            };

            // Act
            var result = await _seasonScheduleGenerator.GenerateSeasonSchedule(season, players);

         
            // Assert
            result.Should().NotBeNull();
            result.Matches.Should().NotBeNull();
            result.Matches.Should().NotBeEmpty();
            result.Matches.Should().HaveCount(1);
        }

    }
}
