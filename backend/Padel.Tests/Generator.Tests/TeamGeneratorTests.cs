using FluentAssertions;
using Padel.Application.Generators;
using Padel.Domain.Models;

namespace Padel.Tests.Generator;

public class TeamGenerartorTests
{

    private readonly TeamGenerator _teamGenerator;

    public TeamGenerartorTests()
    {
        _teamGenerator = new TeamGenerator();
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
        var teams = await _teamGenerator.GenerateAllTeamCombinations(players);

        // Assert
        teams.Should().HaveCount(6); // (4 choose 2) = 6 combinations
        teams.Should().Contain(t => t.Player1.Name == "Player 1" && t.Player2.Name == "Player 2");
        teams.Should().Contain(t => t.Player1.Name == "Player 1" && t.Player2.Name == "Player 3");
        teams.Should().Contain(t => t.Player1.Name == "Player 1" && t.Player2.Name == "Player 4");
        teams.Should().Contain(t => t.Player1.Name == "Player 2" && t.Player2.Name == "Player 3");
        teams.Should().Contain(t => t.Player1.Name == "Player 2" && t.Player2.Name == "Player 4");
        teams.Should().Contain(t => t.Player1.Name == "Player 3" && t.Player2.Name == "Player 4");
    }

    [Fact]
    public async Task GenerateAllTeamCombinations_EmptyList_Throws()
    {
        // Arrange
        var players = new List<Player>(); // Empty list

        // Act & Assert
        await FluentActions
            .Invoking(() => _teamGenerator.GenerateAllTeamCombinations(players))
            .Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("At least two players are required to form teams.");

    }

    [Fact]
    public async Task GenerateAllTeamCombinations_SinglePlayer_Throws()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player(Gender.Male, "Player 1", Guid.NewGuid()),
        };

        // Act & Assert
        await FluentActions
            .Invoking(() => _teamGenerator.GenerateAllTeamCombinations(players))
            .Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("At least two players are required to form teams.");

    }

    [Fact]
    public async Task GenerateAllTeamCombinations_NullList_Throws()
    {
        // Arrange
        List<Player> players = null; // Null list

        // Act & Assert
        await FluentActions
            .Invoking(() => _teamGenerator.GenerateAllTeamCombinations(players))
            .Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("At least two players are required to form teams.");
    }

    [Fact]
    public async Task GenerateAllTeamCombinations_WithCommonPlayer_Should_ReturnEmptyList()
    {
        //Arrange
        var commonUserId = Guid.NewGuid();
        var players = new List<Player>
        {
            new Player(Gender.Male, "Player 1", commonUserId),
            new Player(Gender.Female, "Player 2", commonUserId),     
        };

        // Act
        var teams = await _teamGenerator.GenerateAllTeamCombinations(players);

        // Act & Assert
        teams.Should().HaveCount(0);
    }




}
