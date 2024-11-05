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

        private readonly Random _random = new Random();

        // Helper method to create a random season
        private Season CreateRandomSeason()
        {
            return new Season(_random.Next(10, 20), DateTime.Now, "test season", (DayOfWeek)_random.Next(0, 6));
        }

        // Helper method to create a random list of players
        private List<Player> CreateRandomPlayers()
        {
            var randomPlayers = new List<Player>();
            var numberOfPlayers = _random.Next(15, 25);

            for (var i = 0; i < numberOfPlayers; i++)
            {
                randomPlayers.Add(new Player((Gender)_random.Next(0, 2), $"Player {i}", Guid.NewGuid()));
            }

            return randomPlayers;
        }

        public SeasonScheduleGeneratorTests()
        {
            // Initialize actual instances of each dependency
            _teamGenerator = new TeamGenerator();
            _matchGenerator = new MatchGenerator();

            // Define simple rule(s) for RuleSet
            var rules = new List<IRule>
            {
                new ConsecutiveParticipantsRule(),
                new AllPlayersParticipateRule(),
                new GenderBalanceRule(),
                new BalancedParticipationRule(),
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
            var season = new Season(10, DateTime.Now, "test season", DayOfWeek.Monday);
            var players = new List<Player>
            {
                new Player(Gender.Male, "Player 1", Guid.NewGuid()),
                new Player(Gender.Female, "Player 2", Guid.NewGuid()),
                new Player(Gender.Male, "Player 3", Guid.NewGuid()),
                new Player(Gender.Female, "Player 4", Guid.NewGuid())
            };

            // Act
            var result = await _seasonScheduleGenerator.Generate(season, players);


            // Assert
            result.Should().NotBeNull();
            result.Matches.Should().NotBeNull();
            result.Matches.Should().NotBeEmpty();
            result.Matches.Should().HaveCount(10);
        }

        [Fact]
        public async Task GenerateSeasonSchedule_With_NoPlayers_Should_ThrowException()
        {
            // Arrange
            var season = new Season(4, DateTime.Now, "test season", DayOfWeek.Monday);
            var players = new List<Player>();

            // Act
            await FluentActions
              .Invoking(() => _seasonScheduleGenerator.Generate(season, players))
              .Should()
              .ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task GenerateSeasonSchedule_Should_HaveAllPlayersParticipateInAtLeastOneMatch()
        {
            // Arrange
            var season = CreateRandomSeason();
            var randomPlayers = CreateRandomPlayers();

            var result = await _seasonScheduleGenerator.Generate(season, randomPlayers);

            // Check that matches were generated
            result.Matches.Should().NotBeEmpty();
            result.Matches.Should().NotBeNull();

            // Check that all players participate in at least one match
            var allParticipants = result.Matches
                .SelectMany(match => match.Team1.GetParticipants().Concat(match.Team2.GetParticipants()))
                .Distinct()
                .ToList();

            // Logging for debugging
            var allPlayersCount = randomPlayers.Count;
            var allParticipantsCount = allParticipants.Count;
            var missingPlayers = randomPlayers.Except(allParticipants).ToList();




            allParticipants.Should().HaveCount(allPlayersCount);
        }


        [Fact]
        public async Task GenerateSeasonSchedule_Should_HaveBalancedPlayerParticipation()
        {
            // Arrange
            var season = CreateRandomSeason();
            var randomPlayers = CreateRandomPlayers();

            // Act
            var result = await _seasonScheduleGenerator.Generate(season, randomPlayers);

            // Ensure that the schedule has matches
            result.Matches.Should().NotBeEmpty("The schedule should contain matches.");

            // Count the total number of matches
            int totalMatches = result.Matches.Count;

            // Calculate the maximum possible number of participations for each player
            // Assuming that each match involves 4 players (2 per team), and each player should ideally be distributed evenly
            double averageParticipation = (totalMatches * 4.0) / randomPlayers.Count;

            // Define a higher tolerance (if appropriate)
            const int tolerance = 2; // Allow players to deviate by ±2 matches from the average

            // Recalculate the min and max participation
            int minParticipation = (int)Math.Floor(averageParticipation) - tolerance;
            int maxParticipation = (int)Math.Ceiling(averageParticipation) + tolerance;

            // Count the participation of each player across all matches
            var playerParticipationCount = new Dictionary<Player, int>();
            foreach (var player in randomPlayers)
            {
                int participationCount = result.Matches.Count(match =>
                    match.Team1.GetParticipants().Contains(player) || match.Team2.GetParticipants().Contains(player));
                playerParticipationCount[player] = participationCount;
            }

            // Identify players who are out of the acceptable participation range
            var playersOutOfRange = playerParticipationCount
                .Where(kvp => kvp.Value < minParticipation || kvp.Value > maxParticipation)
                .Select(kvp => kvp.Key)
                .ToList();

            // Calculate the number of players who are out of the range
            int countOutOfRange = playersOutOfRange.Count;

            // Define an acceptable number of players who can be out of the tolerance range
            const int maxPlayersOutOfRange = 2; // Adjust this based on your scenario's tolerance for imbalance

            // Assert that the number of players out of range does not exceed the acceptable limit
            countOutOfRange.Should().BeLessThanOrEqualTo(maxPlayersOutOfRange,
                $"The number of players with participation outside the acceptable range should not exceed {maxPlayersOutOfRange}. " +
                $"Found {countOutOfRange} players out of range.");


            // Assert that each player's participation count falls within the calculated range
            foreach (var kvp in playerParticipationCount)
            {
                kvp.Value.Should().BeInRange(
                    minParticipation,
                    maxParticipation,
                    $"Player {kvp.Key.Name}'s participation ({kvp.Value}) should be within the range of {minParticipation} to {maxParticipation} matches.");
            }
        }



        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GenerateSeasonSchedule_Should_NotHaveMoreThanFiveTotalConsecutiveMatches(int maxConsecutiveMatches)
        {
            // Arrange
            var season = CreateRandomSeason();
            var randomPlayers = CreateRandomPlayers();

            // Act
            var result = await _seasonScheduleGenerator.Generate(season, randomPlayers);

            // Ensure that the schedule has matches
            result.Matches.Should().NotBeEmpty();

            // Track the total count of consecutive matches
            int totalConsecutiveMatches = 0;
            var playerLastMatchIndex = new Dictionary<Player, int>();

            // Iterate over matches to ensure total consecutive matches do not exceed 5
            for (int matchIndex = 0; matchIndex < result.Matches.Count; matchIndex++)
            {
                var match = result.Matches[matchIndex];

                // Get participants of the current match
                var currentMatchPlayers = match.Team1.GetParticipants().Concat(match.Team2.GetParticipants()).ToList();

                // Check if the current match continues from the last match
                bool hasConsecutiveParticipants = false;

                foreach (var player in currentMatchPlayers)
                {
                    // Check if the player has played in the previous match
                    if (playerLastMatchIndex.TryGetValue(player, out int lastMatchIndex))
                    {
                        // If any player has played in the last match, we have consecutive participation
                        if (matchIndex == lastMatchIndex + 1)
                        {
                            hasConsecutiveParticipants = true;
                        }
                    }
                }

                if (hasConsecutiveParticipants)
                {
                    totalConsecutiveMatches++;
                }
                else
                {
                    totalConsecutiveMatches = 1; // Reset to current match
                }

                // Ensure that total consecutive matches do not exceed 2
                totalConsecutiveMatches.Should().BeLessThanOrEqualTo(maxConsecutiveMatches,
                    $"The total count of consecutive matches should not exceed {maxConsecutiveMatches}. Current count: {totalConsecutiveMatches}.");

                // Update the last match index for all current players
                foreach (var player in currentMatchPlayers)
                {
                    playerLastMatchIndex[player] = matchIndex;
                }
            }
        }














    }
}
