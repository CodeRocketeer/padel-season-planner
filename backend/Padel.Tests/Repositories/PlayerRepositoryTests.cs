using Padel.Application.Repositories;
using Padel.Domain.Models;
using Xunit;

namespace Padel.Application.Tests.Repositories
{
    public class PlayerRepositoryTests
    {
        //private readonly PlayerRepository _playerRepository;

        //public PlayerRepositoryTests()
        //{
        //    _playerRepository = new PlayerRepository();
        //}

        //// 1. Test CreateAsync Method
        //[Fact]
        //public async Task CreateAsync_ShouldAddSuccessfully()
        //{
        //    // Arrange
        //    var player = new Player(Guid.NewGuid(), "John Doe", "M");

        //    // Act
        //    var result = await _playerRepository.CreateAsync(player);

        //    // Assert
        //    Assert.True(result);
        //    var allPlayers = await _playerRepository.GetAllAsync();
        //    Assert.Contains(allPlayers, t => t.Id == player.Id);
        //}

        //[Fact]
        //public async Task CreateAsync_ShouldNotAllowDuplicateIds()
        //{
        //    // Arrange
        //    var player1 = new Player(Guid.NewGuid(), "John Doe", "M");
        //    var player2 = new Player(player1.Id, "John Doe", "M");

        //    // Act
        //    await _playerRepository.CreateAsync(player1);
        //    var result = await _playerRepository.CreateAsync(player2); // This should fail

        //    // Assert
        //    Assert.False(result); // Should return false due to duplicate Id
        //    var allPlayers = await _playerRepository.GetAllAsync();
        //    Assert.Single(allPlayers); // Only one player should exist
        //    Assert.Contains(allPlayers, t => t.Id == player1.Id); // Ensure the original player exists
        //}

        //[Fact]
        //public async Task GetAllAsync_ShouldReturnAllPlayers()
        //{
        //    // Arrange
        //    var player1 = new Player(Guid.NewGuid(), "John Doe", "M");
        //    var player2 = new Player(Guid.NewGuid(), "Jane Doe", "F");

        //    await _playerRepository.CreateAsync(player1);
        //    await _playerRepository.CreateAsync(player2);

        //    // Act
        //    var allPlayers = await _playerRepository.GetAllAsync();

        //    // Assert
        //    Assert.NotEmpty(allPlayers);
        //    Assert.Contains(allPlayers, t => t.Id == player1.Id);
        //    Assert.Contains(allPlayers, t => t.Id == player2.Id);
        //}

        //[Fact]
        //public async Task GetAllAsync_ShouldReturnEmptyCollectionWhenNoPlayersExist()
        //{
        //    // Act
        //    var allPlayers = await _playerRepository.GetAllAsync();

        //    // Assert
        //    Assert.Empty(allPlayers);
        //}

        //[Fact]
        //public async Task GetByIdAsync_ShouldReturnCorrectPlayer()
        //{
        //    // Arrange
        //    var player = new Player(Guid.NewGuid(), "John Doe", "M");

        //    await _playerRepository.CreateAsync(player);

        //    // Act
        //    var result = await _playerRepository.GetByIdAsync(player.Id);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(player.Id, result?.Id);
        //    Assert.Equal(player.Name, result?.Name);
        //    Assert.Equal(player.Sex, result?.Sex);
        //}

        //[Fact]
        //public async Task GetByIdAsync_ShouldReturnNullForNonexistentId()
        //{
        //    // Arrange
        //    var nonExistentId = Guid.NewGuid();

        //    // Act
        //    var result = await _playerRepository.GetByIdAsync(nonExistentId);

        //    // Assert
        //    Assert.Null(result);
        //}

        //[Fact]
        //public async Task DeleteByIdAsync_ShouldRemovePlayerSuccessfully()
        //{
        //    // Arrange
        //    var player = new Player(Guid.NewGuid(), "John Doe", "M");

        //    await _playerRepository.CreateAsync(player);

        //    // Act
        //    var result = await _playerRepository.DeleteByIdAsync(player.Id);

        //    // Assert
        //    Assert.True(result);
        //    var allPlayers = await _playerRepository.GetAllAsync();
        //    Assert.DoesNotContain(allPlayers, t => t.Id == player.Id);
        //}

        //// New Test for Updating a Player
        //[Fact]
        //public async Task UpdateAsync_ShouldUpdatePlayerSuccessfully()
        //{
        //    // Arrange
        //    var player = new Player(Guid.NewGuid(), "John Doe", "M");
        //    await _playerRepository.CreateAsync(player);
        //    var updatedPlayer = new Player(player.Id, "John Smith", "M"); // Same ID

        //    // Act
        //    var result = await _playerRepository.UpdateAsync(updatedPlayer);

        //    // Assert
        //    Assert.True(result);
        //    var retrievedPlayer = await _playerRepository.GetByIdAsync(player.Id);
        //    Assert.Equal("John Smith", retrievedPlayer?.Name);
        //}

        //// New Test for Updating a Non-existent Player
        //[Fact]
        //public async Task UpdateAsync_ShouldReturnFalseForNonExistentPlayer()
        //{
        //    // Arrange
        //    var nonExistentPlayer = new Player(Guid.NewGuid(), "Non Existent", "M");

        //    // Act
        //    var result = await _playerRepository.UpdateAsync(nonExistentPlayer);

        //    // Assert
        //    Assert.False(result); // Should return false since the player does not exist
        //}

        //// New Test for Checking Player Existence
        //[Fact]
        //public async Task ExistsByIdAsync_ShouldReturnTrueForExistingPlayer()
        //{
        //    // Arrange
        //    var player = new Player(Guid.NewGuid(), "John Doe", "M");
        //    await _playerRepository.CreateAsync(player);

        //    // Act
        //    var result = await _playerRepository.ExistsByIdAsync(player.Id);

        //    // Assert
        //    Assert.True(result); // Should return true since the player exists
        //}

        //[Fact]
        //public async Task ExistsByIdAsync_ShouldReturnFalseForNonExistentPlayer()
        //{
        //    // Arrange
        //    var nonExistentId = Guid.NewGuid();

        //    // Act
        //    var result = await _playerRepository.ExistsByIdAsync(nonExistentId);

        //    // Assert
        //    Assert.False(result); // Should return false since the player does not exist
        //}

        //// New Test for Attempting to Delete a Non-existent Player
        //[Fact]
        //public async Task DeleteByIdAsync_ShouldReturnFalseForNonExistentId()
        //{
        //    // Arrange
        //    var nonExistentId = Guid.NewGuid();

        //    // Act
        //    var result = await _playerRepository.DeleteByIdAsync(nonExistentId);

        //    // Assert
        //    Assert.False(result); // Should return false since the player does not exist
        //}
    }
}
