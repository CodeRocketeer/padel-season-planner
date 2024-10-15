

using Padel.Application.Repositories;
using Padel.Domain.Models;


namespace Padel.Application.Tests.Repositories
{
    public class PlayerRepositoryTests
    {

        private readonly PlayerRepository _playerRepository;


        public PlayerRepositoryTests()
        {
            _playerRepository = new PlayerRepository();
        }


        // 1. Test CreateAsync Method
        [Fact]
        public async Task CreateAsync_ShouldAddSuccessfully()
        {
            // Arrange
            var player = new Player
            {
                Id = Guid.NewGuid(),
                Name = "Jens",
                Sex = "M",
            };

            // Act
            var result = await _playerRepository.CreateAsync(player);

            // Assert
            Assert.True(result);
            var allPlayers = await _playerRepository.GetAllAsync();
            Assert.Contains(allPlayers, t => t.Id == player.Id);
        }


    }
}
