using FluentAssertions;
using Padel.Application.Rules;
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Tests.Rules;


public class AllPlayersParticipateRuleTests
{
    private readonly AllPlayersParticipateRule _rule;
    private readonly Random random = new Random();

    public AllPlayersParticipateRuleTests()
    {
        _rule = new AllPlayersParticipateRule();
    }

    [Fact]
    public void Validate_Should_Return_100_When_No_Matches()
    {
        // Arrange
        List<Match> matches = null; // No matches
        List<Player> players = new List<Player> { new Player(Gender.Male, "Player 1", Guid.NewGuid()) };

        // Act
        decimal result = _rule.Validate(matches, players);

        // Assert
        result.Should().Be(100);
    }

    [Fact]
    public void Validate_Should_Return_100_When_One_Player_Missing()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var player3 = new Player(Gender.Male, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Female, "Player 4", Guid.NewGuid());
        var player5 = new Player(Gender.Male, "Player 5", Guid.NewGuid()); // Not participating

        var players = new List<Player> { player1, player2, player3, player4, player5 };

        var team1 = new Team(player1, player2){ Id = 1};
        var team2 = new Team(player3, player4) { Id = 2 };
        var match = new Match(team1, team2);

        var matches = new List<Match> { match };

        // Act
        decimal result = _rule.Validate(matches, players);

        // Assert
        result.Should().Be(100);
    }


    [Fact]
    public void Validate_Should_Return_100_When_All_Players_Are_Missing()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var player3 = new Player(Gender.Male, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Female, "Player 4", Guid.NewGuid());
        var player5 = new Player(Gender.Male, "Player 5", Guid.NewGuid());

        var players = new List<Player> { player1, player2, player3, player4, player5 };

        var matches = new List<Match>(); // No matches available

        // Act
        decimal result = _rule.Validate(matches, players);

        // Assert
        result.Should().Be(100); // All players missing should return 100% fault
    }


}
