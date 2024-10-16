using Padel.Domain.Models;
namespace Padel.Tests.Models
{
    public class PlayerModelTests
    {
        [Fact]
        public void Player_Should_Have_RequiredProperties()
        {
            // Arrange
            var player = new Player
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "John Doe",
                Sex = "M",
                SeasonId = Guid.NewGuid()
            };

            // Act & Assert
            Assert.NotEqual(Guid.Empty, player.Id);
            Assert.NotEqual(Guid.Empty, player.UserId);
            Assert.Equal("John Doe", player.Name);
            Assert.Equal("M", player.Sex);
            Assert.NotEqual(Guid.Empty, player.SeasonId);
        }

        [Fact]
        public void UpdatePlayer_Should_UpdateNameAndSex()
        {
            // Arrange
            var player = new Player
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "John Doe",
                Sex = "M",
                SeasonId = Guid.NewGuid()
            };

            // Act
            player.UpdatePlayer("Jane Doe", "F");

            // Assert
            Assert.Equal("Jane Doe", player.Name);
            Assert.Equal("F", player.Sex);
        }

        [Theory]
        [InlineData("", "M")]
        [InlineData("John", "")]
        [InlineData("John", "X")]
        public void UpdatePlayer_Should_ThrowArgumentException_OnInvalidData(string name, string sex)
        {
            // Arrange
            var player = new Player
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "John Doe",
                Sex = "M",
                SeasonId = Guid.NewGuid()
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => player.UpdatePlayer(name, sex));
        }


    }
}
