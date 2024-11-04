
using Padel.Application.Rules;
using Padel.Domain.Models;



namespace Padel.Tests.Rules;

public class ConsecutiveParticipantsRuleTests
{
    private ConsecutiveParticipantsRule _rule;
    private readonly Random _random = new Random();
    private List<Player> CreateRandomPlayers(int? amount = 5)
    {
        var randomPlayers = new List<Player>();


        for (var i = 0; i < amount; i++)
        {
            randomPlayers.Add(new Player((Gender)0, $"Player {i}", Guid.NewGuid()));
        }

        return randomPlayers;
    }


    public ConsecutiveParticipantsRuleTests()
    {
        _rule = new ConsecutiveParticipantsRule();
    }

    [Fact]
    public void Validate_Should_Returns100FaultPoints_When_MatchSetIsEmpty()
    {
        // Arrange
        var matchSet = new List<Match>();
        var players = new List<Player> { new Player(Gender.Male, "Player 1", Guid.NewGuid()) };

        // Act
        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(100, result);
    }

    [Fact]
    public void Validate_Should_ReturnsZeroFaultPoints_When_ThereIsNoConsecutiveParticipant()
    {

        // Arrange
        var players = CreateRandomPlayers(8);
        var teams = new List<Team>
        {
            new Team(players[0], players[1]){Id = 1},
            new Team(players[2], players[3]){Id = 2},
            new Team(players[4], players[5]){Id = 3},
            new Team(players[6], players[7]){Id = 4}
        };

        var matchSet = new List<Match>
        {
            new Match(teams[0], teams[1]),
            new Match(teams[2], teams[3]),

        };

        // Act
        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Validate_Should_ReturnsOneFaultPoints_When_ThereIsOneConsecutiveParticipant()
    {

        // Arrange
        var players = CreateRandomPlayers(8);
        int samePlayerIndex = 0;
        var teams = new List<Team>
        {
            new Team(players[samePlayerIndex], players[1]){Id = 1},
            new Team(players[2], players[3]){Id = 2},
            new Team(players[samePlayerIndex], players[5]){Id = 3},
            new Team(players[6], players[7]){Id = 4}
        };

        var matchSet = new List<Match>
        {
            new Match(teams[0], teams[1]),
            new Match(teams[2], teams[3]),

        };

        // Act
        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Validate_Should_ReturnsTwoFaultPoints_When_ThereAreTwoConsecutiveParticipants()
    {
        // Arrange
        var players = CreateRandomPlayers(8);
        var teams = new List<Team>
        {
            new Team(players[0], players[1]){Id = 1},
            new Team(players[2], players[3]){Id = 2},
            new Team(players[0], players[1]){Id = 3},
            new Team(players[6], players[7]){Id = 4}
        };

        var matchSet = new List<Match>
        {
            new Match(teams[0], teams[1]),
            new Match(teams[2], teams[3]),

        };

        // Act
        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Validate_Should_ReturnZeroFaultPoints_When_PlayersParticipateInNonAdjacentMatches()
    {
        // Arrange
        var players = CreateRandomPlayers(8);
        var teams = new List<Team>
        {
            new Team(players[0], players[1]){Id = 1},
            new Team(players[2], players[3]){Id = 2},
            new Team(players[4], players[5]){Id = 3},
            new Team(players[6], players[7]){Id = 4}
        };

        var matchSet = new List<Match>
        {
            new Match(teams[0], teams[1]), // non adjacent match (same players)
            new Match(teams[2], teams[3]),
            new Match(teams[0], teams[1]), // non adjacent match (same players
        };

        // Act
        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Validate_Should_CorrectlyCalculateFaultPoints_ForComplexParticipationPatterns()
    {
        // Arrange
        var players = CreateRandomPlayers(10);
        var teams = new List<Team>
    {
        new Team(players[0], players[1]){Id = 1},
        new Team(players[2], players[3]){Id = 2},
        new Team(players[0], players[4]){Id = 3}, // Player 0 participates consecutively
        new Team(players[5], players[6]){Id = 4},
        new Team(players[7], players[8]){Id = 5},
        new Team(players[4], players[9]){Id = 6}  // Player 4 participates non-consecutively
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
        Assert.Equal(2, result); // Only one violation for Player 0
    }

}