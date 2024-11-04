using FluentAssertions;
using Padel.Application.Generators;
using Padel.Application.Services;
using Padel.Domain.Models;

namespace Padel.Tests.Generator;

public class MatchGeneratorTests
{

    private readonly MatchGenerator _matchGenerator;
    private readonly TeamGenerator _teamGenerator;
    private readonly Random _random = new Random();

    private Season CreateRandomSeason()
    {
        return new Season(_random.Next(10, 20), DateTime.Now, "test season", (DayOfWeek)_random.Next(0, 6));
    }

    // Helper method to create a random list of players
    private List<Player> CreateRandomPlayers()
    {
        var randomPlayers = new List<Player>();
        var numberOfPlayers = _random.Next(8, 20);

        for (var i = 0; i < numberOfPlayers; i++)
        {
            randomPlayers.Add(new Player((Gender)_random.Next(0, 1), $"Player {i}", Guid.NewGuid()));
        }

        return randomPlayers;
    }

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
        matches.Should().HaveCount(3);
    }

    [Fact]
    public async Task GenerateAllMatchCombinations_EmptyTeams_ReturnsEmptyList()
    {
        // Arrange
        var teams = new List<Team>();

        // Act
        var matches = await _matchGenerator.GenerateAllMatchCombinations(teams);

        matches.Should().BeEmpty();
    
    }

    [Fact]
    public async Task GenerateAllMatchCombinations_OneTeam_ReturnsEmptyList()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());

        var teams = new List<Team>
        {
            new Team(player1, player2)
        };

        // Act
        var matches = await _matchGenerator.GenerateAllMatchCombinations(teams);

        matches.Should().BeEmpty();
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


    [Fact]
    public async Task GenerateAllMatchSchedulesSets_ShouldReturnCorrectNumberOfSets()
    {
        // Arrange
        int amountOfMatches = 5;
        int numberOfRandomSets = 10;

        var allTeamCombinations = await _teamGenerator.GenerateAllTeamCombinations(CreateRandomPlayers());
        var allMatchCombinations = await _matchGenerator.GenerateAllMatchCombinations(allTeamCombinations);

        // Act
        var result = await _matchGenerator.GenerateAllMatchSchedulesSets(allMatchCombinations, amountOfMatches, numberOfRandomSets);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(numberOfRandomSets); // Should return the requested number of sets
        foreach (var matchSet in result)
        {
            matchSet.Should().HaveCount(amountOfMatches); // Each set should have the requested number of matches
        }
    }











}
