using Padel.Domain.Models;



namespace Padel.Tests.Rules;

public class GenderBalanceRuleTests
{

    private GenderBalanceRule _rule;

    public GenderBalanceRuleTests()
    {
        _rule = new GenderBalanceRule();
    }


    private List<Player> CreatePlayers(int maleCount, int femaleCount)
    {
        var players = new List<Player>();
        for (int i = 0; i < maleCount; i++)
        {
            players.Add(new Player(Gender.Male, $"Male Player {i}", Guid.NewGuid()));
        }
        for (int i = 0; i < femaleCount; i++)
        {
            players.Add(new Player(Gender.Female, $"Female Player {i}", Guid.NewGuid()));
        }
        return players;
    }

    [Fact]
    public void Validate_Should_Return100FaultPoints_When_MatchSetIsEmpty()
    {
        // Arrange
        var matchSet = new List<Match>();
        var players = CreatePlayers(2, 2);

        // Act
        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(100, result);
    }


    [Fact]
    public void Validate_Should_ReturnZeroFaultPoints_When_AllMatchesAreGenderBalanced()
    {
        // Arrange
        var players = CreatePlayers(2, 2);
        var teams = new List<Team>
            {
                new Team(players[0], players[2]){Id = 1}, // Male and Female
                new Team(players[1], players[3]){Id = 2}  // Male and Female
            };

        var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1])
            };

        // Act
        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Validate_Should_ReturnOneFaultPoint_When_OneMatchIsPartialImbalance()
    {
        // Arrange
        var teams = new List<Team>
        {
                new Team(new Player(Gender.Male, "Player 1", Guid.NewGuid()), new Player(Gender.Male, "Player 2", Guid.NewGuid())){Id = 1}, // Both Male
                new Team(new Player(Gender.Male, "Player 3", Guid.NewGuid()), new Player(Gender.Female, "Player 4", Guid.NewGuid())){Id = 2}, // Male and Female (Balanced)
            };

        var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1]), // partially imbalanced: (ex. mm vs mf)
               
            };

        // Act
        var players = new List<Player>();

        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(1, result);
    }


    [Fact]
    public void Validate_Should_ReturnTwoFaultPoint_When_OneMatchIsCompleteImbalance()
    {
        var teams = new List<Team>
            {
                new Team(new Player(Gender.Male, "Player 1", Guid.NewGuid()), new Player(Gender.Male, "Player 2", Guid.NewGuid())){Id = 1}, // Both Male
                new Team(new Player(Gender.Female, "Player 3", Guid.NewGuid()), new Player(Gender.Female, "Player 4", Guid.NewGuid())){Id = 2} // Male and Female (Balanced)
            };

        var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1]), // complete imbalanced: (ex. mm vs ff)
             
            };

        // Act
        var players = new List<Player>();

        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(2, result);

    }

    [Fact]
    public void Validate_Should_ReturnTwoFaultPoints_When_TwoMatchesHavePartialImbalance()
    {
        var teams = new List<Team>
            {
                new Team(new Player(Gender.Male, "Player 1", Guid.NewGuid()), new Player(Gender.Female, "Player 2", Guid.NewGuid())){Id = 1},
                new Team(new Player(Gender.Female, "Player 3", Guid.NewGuid()), new Player(Gender.Female, "Player 4", Guid.NewGuid())){Id = 2},
                new Team(new Player(Gender.Male, "Player 5", Guid.NewGuid()), new Player(Gender.Male, "Player 6", Guid.NewGuid())){Id = 3},
                new Team(new Player(Gender.Male, "Player 7", Guid.NewGuid()), new Player(Gender.Female, "Player 8", Guid.NewGuid())){Id = 4}
            };

        var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1]), // partially imbalanced: (ex. mf vs ff)
                new Match(teams[2], teams[3]) // partially imbalanced: (ex. mm vs mf)
            };

        // Act
        var players = new List<Player>();

        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(2, result);

    }

    [Fact]
    public void Validate_Should_ReturnThreeFaultPoints_When_OneMatchHasCompleteImbalance_And_OneMatchHasPartialImbalance()
    {
        var teams = new List<Team>
            {
                new Team(new Player(Gender.Male, "Player 1", Guid.NewGuid()), new Player(Gender.Male, "Player 2", Guid.NewGuid())){Id = 1},
                new Team(new Player(Gender.Female, "Player 3", Guid.NewGuid()), new Player(Gender.Female, "Player 4", Guid.NewGuid())){Id = 2},
                new Team(new Player(Gender.Male, "Player 5", Guid.NewGuid()), new Player(Gender.Male, "Player 6", Guid.NewGuid())){Id = 3},
                new Team(new Player(Gender.Male, "Player 7", Guid.NewGuid()), new Player(Gender.Female, "Player 8", Guid.NewGuid())){Id = 4}
            };

        var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1]), // complete imbalanced: (ex. mm vs ff)
                new Match(teams[2], teams[3]) // partially imbalanced: (ex. mm vs mf)
            };

        // Act
        var players = new List<Player>();

        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(3, result);
    }

    // TODO: Validate_Should_ReturnFourFaultPoints_When_TwoMatchesHaveCompleteImbalance
    [Fact]
    public void Validate_Should_ReturnFourFaultPoints_When_TwoMatchesHaveCompleteImbalance()
    {
        var teams = new List<Team>
            {
                new Team(new Player(Gender.Male, "Player 1", Guid.NewGuid()), new Player(Gender.Male, "Player 2", Guid.NewGuid())){Id = 1},
                new Team(new Player(Gender.Female, "Player 3", Guid.NewGuid()), new Player(Gender.Female, "Player 4", Guid.NewGuid())){Id = 2},
                new Team(new Player(Gender.Male, "Player 5", Guid.NewGuid()), new Player(Gender.Male, "Player 6", Guid.NewGuid())){Id = 3},
                new Team(new Player(Gender.Female, "Player 7", Guid.NewGuid()), new Player(Gender.Female, "Player 8", Guid.NewGuid())){Id = 4}
            };

        var matchSet = new List<Match>
            {
                new Match(teams[0], teams[1]), // complete imbalanced: (ex. mm vs ff)
                new Match(teams[2], teams[3]) // complete imbalanced: (ex. mm vs ff)
            };

        // Act
        var players = new List<Player>();

        int result = _rule.Validate(matchSet, players);

        // Assert
        Assert.Equal(4, result);
    }



}

