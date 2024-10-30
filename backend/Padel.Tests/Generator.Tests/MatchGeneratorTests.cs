using FluentAssertions;
using Padel.Application.Generators;
using Padel.Application.Services;
using Padel.Domain.Models;

namespace Padel.Tests.Generator;

public class MatchGeneratorTests
{

    private readonly MatchGenerator _matchGenerator;
    private readonly TeamGenerator _teamGenerator;

    public MatchGeneratorTests()
    {
        _matchGenerator = new MatchGenerator();
        _teamGenerator = new TeamGenerator();
    }

    [Fact]
    public async Task GenerateAllMatchCombinations_ValidPlayers_ReturnsAllCombinations()
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
        var teams = await _teamGenerator.GenerateAllTeamCombinations(players);
        var matches = await _matchGenerator.GenerateAllMatchCombinations(teams.ToList());

        // Assert
        teams.Should().HaveCount(6);
        matches.Should().HaveCount(2);
    }

    [Fact]
    public async Task GenerateAllMatchCombinations_EmptyTeams_ThrowsArgumentException()
    {
        // Arrange
        var teams = new List<Team>();

        // Act
        await FluentActions
          .Invoking(() => _matchGenerator.GenerateAllMatchCombinations(teams))
          .Should()
          .ThrowAsync<ArgumentException>();
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
           .Invoking(() => _matchGenerator.GenerateAllMatchCombinations(teams))
           .Should()
           .ThrowAsync<ArgumentException>();
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
        var matches = await _matchGenerator.GenerateAllMatchCombinations(teams);

        // Assert
        matches.Should().BeEmpty(); // No matches should be created due to common players
    }









}
