using FluentAssertions;
using Padel.Application.Database.Entities;
using Padel.Application.Mappers;
using Padel.Application.Models;
using Padel.Domain.Models;

namespace Padel.Tests.Models;

public class PlayerModelTests
{

    [Fact]
    public void CreateNewPlayer_Creates()
    {
        var userId = Guid.NewGuid();
        var player = new Player(Gender.Male, "Joske", userId);
        player.Name.Should().Be("Joske");
        player.Gender.Should().Be(Gender.Male);
        player.UserId.Should().Be(userId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void CreateNewPlayer_EmptyOrNullName_Throws(string name)
    {
        FluentActions
             .Invoking(() => new Player(Gender.Male, name, Guid.NewGuid()))
             .Should()
             .Throw< ArgumentNullException>();
    }

    [Fact]
    public void CreateNewPlayer_InvalidUserId_Throws()
    {
        FluentActions
             .Invoking(() => new Player(Gender.Male, "Joske", Guid.Empty))
             .Should()
             .Throw<ArgumentException>();
    }

    [Fact]
    public void ToEntity_ConvertsPlayerToPlayerEntity()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var player = new Player(Gender.Female, "Sofie", userId)
        {
            Id = 1,
        };

        // Act
        var playerEntity = player.ToEntity();

        // Assert
        playerEntity.Id.Should().Be(player.Id);
        playerEntity.UserId.Should().Be(player.UserId);
        playerEntity.Gender.Should().Be(player.Gender);
        playerEntity.Name.Should().Be(player.Name);
    }

    [Fact]
    public void FromEntity_ConvertsPlayerEntityToPlayer()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var playerEntity = new PlayerEntity
        {
            Id = 2,
            UserId = userId,
            Gender = Gender.Male,
            Name = "Liam"
        };

        // Act
        var player = playerEntity.FromEntity();

        // Assert
        player.Id.Should().Be(playerEntity.Id);
        player.UserId.Should().Be(playerEntity.UserId);
        player.Gender.Should().Be(playerEntity.Gender);
        player.Name.Should().Be(playerEntity.Name);
    }

    [Fact]
    public void ToEntity_WithoutId_SetsDefaultIdInEntity()
    {
        // Arrange
        var player = new Player(Gender.Male, "Jonas", Guid.NewGuid());

        // Act
        var playerEntity = player.ToEntity();

        // Assert
        playerEntity.Id.Should().Be(0); // Ensures EF Core will assign an Id on save
    }



}
