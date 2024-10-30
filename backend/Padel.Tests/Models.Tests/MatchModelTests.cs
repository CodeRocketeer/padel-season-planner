using FluentAssertions;
using Padel.Application.Database.Entities;
using System;
using Padel.Application.Mappers;
using Padel.Domain.Models;


namespace Padel.Tests.Models;

public class MatchModelTests
{
    [Fact]
    public void CreateNewMatch_ValidTeams_Creates()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2) { Id = 1 };

        var player3 = new Player(Gender.Male, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Female, "Player 4", Guid.NewGuid());
        var team2 = new Team(player3, player4) { Id = 2 };

        // Act
        var match = new Match(team1, team2);

        // Assert
        match.Team1.Should().Be(team1);
        match.Team2.Should().Be(team2);
    }

    [Fact]
    public void CreateNewMatch_TeamsWithCommonPlayer_Throws()
    {
        // Arrange
        var commonPlayer = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var player3 = new Player(Gender.Male, "Player 3", Guid.NewGuid());

        var team1 = new Team(commonPlayer, player2) { Id = 1 };
        var team2 = new Team(commonPlayer, player3) { Id = 2 }; // Same player in both teams

        // Act & Assert
        FluentActions
            .Invoking(() => new Match(team1, team2))
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void CreateNewMatch_SameTeamId_Throws()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2) { Id = 1 };

        var team2 = new Team(new Player(Gender.Male, "Player 3", Guid.NewGuid()), new Player(Gender.Female, "Player 4", Guid.NewGuid())) { Id = 1 }; // Same ID

        // Act & Assert
        FluentActions
            .Invoking(() => new Match(team1, team2))
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    public void CreateNewMatch_NullTeam1_Throws(string team1)
    {
        // Arrange
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var team2 = new Team(new Player(Gender.Male, "Player 3", Guid.NewGuid()), player2) { Id = 2 };

        // Act & Assert
        FluentActions
            .Invoking(() => new Match(team1 == null ? null : new Team(new Player(Gender.Male, "Player 1", Guid.NewGuid()), new Player(Gender.Female, "Player 1", Guid.NewGuid())), team2))
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    public void CreateNewMatch_NullTeam2_Throws(string team2)
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var team1 = new Team(player1, new Player(Gender.Female, "Player 2", Guid.NewGuid())) { Id = 1 };

        // Act & Assert
        FluentActions
            .Invoking(() => new Match(team1, team2 == null ? null : new Team(new Player(Gender.Male, "Player 3", Guid.NewGuid()), new Player(Gender.Female, "Player 4", Guid.NewGuid())) { Id = 2 }))
            .Should()
            .Throw<ArgumentException>();
 
    }

    [Fact]
    public void ToEntity_ConvertsMatchToMatchEntity()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2) { Id = 1 };

        var player3 = new Player(Gender.Male, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Female, "Player 4", Guid.NewGuid());
        var team2 = new Team(player3, player4) { Id = 2 };

        var match = new Match(team1, team2) { Id = 3, SeasonId = 4, MatchDate = DateTime.UtcNow.AddDays(1) };

        // Act
        var matchEntity = match.ToEntity();

        // Assert
        matchEntity.Id.Should().Be(match.Id);
        matchEntity.SeasonId.Should().Be(match.SeasonId);
        matchEntity.MatchDate.Should().Be(match.MatchDate);
    }

    [Fact]
    public void FromEntity_ConvertsMatchEntityToMatch()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2) { Id = 1 };

        var player3 = new Player(Gender.Male, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Female, "Player 4", Guid.NewGuid());
        var team2 = new Team(player3, player4) { Id = 2 };

        var matchEntity = new MatchEntity
        {
            Id = 3,
            SeasonId = 4,
            MatchDate = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var match = matchEntity.FromEntity(team1, team2);

        // Assert
        match.Should().NotBeNull();
        match.Id.Should().Be(matchEntity.Id);
        match.SeasonId.Should().Be(matchEntity.SeasonId);
        match.MatchDate.Should().Be(matchEntity.MatchDate);
        match.Team1.Should().Be(team1);
        match.Team2.Should().Be(team2);
    }

    [Fact]
    public void IsValidMatch_MM_vs_MM_ReturnsTrue()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Male, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2)
        {
            Id = 1
        };


        var player3 = new Player(Gender.Male, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Male, "Player 4", Guid.NewGuid());
        var team2 = new Team(player3, player4)
        {
            Id = 2
        };

        // Act
        var match = new Match(team1, team2);
        bool isValid = Match.IsValidMatch(team1, team2);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidMatch_FF_vs_FF_ReturnsTrue()
    {
        // Arrange
        var player1 = new Player(Gender.Female, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2)
        {
            Id = 1
        };


        var player3 = new Player(Gender.Female, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Female, "Player 4", Guid.NewGuid());
        var team2 = new Team(player3, player4)
        {
            Id = 2
        };


        // Act
        var match = new Match(team1, team2);
        bool isValid = Match.IsValidMatch(team1, team2);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidMatch_Mixed_vs_Mixed_ReturnsTrue()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2)
        {
            Id = 1
        };

        var player3 = new Player(Gender.Female, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Male, "Player 4", Guid.NewGuid());
        var team2 = new Team(player3, player4)
        {
            Id = 2
        };

        // Act
        var match = new Match(team1, team2);
        bool isValid = Match.IsValidMatch(team1, team2);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidMatch_MM_vs_FF_ShouldThrow()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Male, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2)
        {
            Id = 1
        };

        var player3 = new Player(Gender.Female, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Female, "Player 4", Guid.NewGuid());
        var team2 = new Team(player3, player4)
        {
            Id = 2
        };

        // Assert
        FluentActions
        .Invoking(() => new Match(team1, team2))
        .Should()
        .Throw<ArgumentException>();
    }


    [Fact]
    public void IsValidMatch_Mixed_vs_MM_ShouldThrow()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player 1", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player 2", Guid.NewGuid());
        var team1 = new Team(player1, player2)
        {
            Id = 1
        };

        var player3 = new Player(Gender.Male, "Player 3", Guid.NewGuid());
        var player4 = new Player(Gender.Male, "Player 4", Guid.NewGuid());
        var team2 = new Team(player3, player4)
        {
            Id = 2
        };

        // Act & Assert
        FluentActions
        .Invoking(() => new Match(team1, team2))
        .Should()
        .Throw<ArgumentException>();
    }
}
