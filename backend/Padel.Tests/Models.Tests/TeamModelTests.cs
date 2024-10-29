using FluentAssertions;
using Padel.Application.Database.Entities;
using Padel.Application.Models;


namespace Padel.Tests.Models;

public class TeamModelTests
{

    [Fact]
    public void CreateNewTeam_WithValidPlayers_CreatesSuccessfully()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player One", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player Two", Guid.NewGuid());

        // Act
        var team = new Team(player1, player2);

        // Assert
        team.Player1.Should().Be(player1);
        team.Player2.Should().Be(player2);
    }

    [Fact]
    public void CreateNewTeam_WithSameUserIds_ThrowsArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var player1 = new Player(Gender.Male, "Player One", userId);
        var player2 = new Player(Gender.Female, "Player Two", userId);

        // Act
        Action action = () => new Team(player1, player2);

        // Assert
        action.Should().Throw<ArgumentException>();
             
    }

    [Fact]
    public void CreateNewTeam_WithNullPlayer1_ThrowsArgumentNullException()
    {
        // Arrange
        var player2 = new Player(Gender.Female, "Player Two", Guid.NewGuid());

        // Act
        Action action = () => new Team(null, player2);

        // Assert
        action.Should().Throw<ArgumentNullException>();
              
    }
    [Fact]
    public void CreateNewTeam_WithNullPlayer2_ThrowsArgumentNullException()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player One", Guid.NewGuid());

        // Act
        Action action = () => new Team(player1, null);

        // Assert
        action.Should().Throw<ArgumentNullException>();
            
    }

    [Fact]
    public void ToEntity_ConvertsTeamToEntityCorrectly()
    {
        // Arrange
        var player1 = new Player(Gender.Male, "Player One", Guid.NewGuid());
        var player2 = new Player(Gender.Female, "Player Two", Guid.NewGuid());
        var team = new Team(player1, player2) { Id = 1, MatchId = 2 };

        // Act
        var entity = Team.ToEntity(team);

        // Assert
        entity.MatchId.Should().Be(2);
        entity.Players.Should().HaveCount(2);
        entity.Players.Should().Contain(p => p.UserId == player1.UserId && p.Name == player1.Name);
        entity.Players.Should().Contain(p => p.UserId == player2.UserId && p.Name == player2.Name);
    }

    [Fact]
    public void FromEntity_ConvertsEntityToTeamCorrectly()
    {
        // Arrange
        var playerEntity1 = new PlayerEntity { Id = 1, UserId = Guid.NewGuid(), Name = "Player One", Gender = Gender.Male };
        var playerEntity2 = new PlayerEntity { Id = 2, UserId = Guid.NewGuid(), Name = "Player Two", Gender = Gender.Female };
        var teamEntity = new TeamEntity
        {
            Id = 1,
            MatchId = 2,
            Players = new List<PlayerEntity> { playerEntity1, playerEntity2 }
        };

        // Act
        var team = Team.FromEntity(teamEntity);

        // Assert
        team.Id.Should().Be(1);
        team.MatchId.Should().Be(2);
        team.Player1.UserId.Should().Be(playerEntity1.UserId);
        team.Player2.UserId.Should().Be(playerEntity2.UserId);
    }

   
    [Fact]
    public void FromEntity_WithIncorrectNumberOfPlayers_ThrowsArgumentException()
    {
        // Arrange
        var teamEntity = new TeamEntity
        {
            Id = 1,
            MatchId = 2,
            Players = new List<PlayerEntity>
        {
            new PlayerEntity { Id = 1, UserId = Guid.NewGuid(), Name = "Player One", Gender = Gender.Male }
        }
        };

        // Act
        var action = () => Team.FromEntity(teamEntity);

        // Assert
        action.Should()
              .Throw<ArgumentException>();
    
    }






}
