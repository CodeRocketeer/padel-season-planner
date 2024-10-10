
using Padel.Application.Models;
using Padel.Application.Repositories;

namespace Padel.Application.Tests.Repositories
{
    public class TeamRepositoryTests
    {

        private readonly TeamRepository _teamRepository;


        public TeamRepositoryTests()
        {
            _teamRepository = new TeamRepository();
        }

        // 1. Test CreateAsync Method
        [Fact]
        public async Task CreateAsync_ShouldAddTeamSuccessfully()
        {
            // Arrange
            var team = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };

            // Act
            var result = await _teamRepository.CreateAsync(team);

            // Assert
            Assert.True(result);
            var allTeams = await _teamRepository.GetAllAsync();
            Assert.Contains(allTeams, t => t.Id == team.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddMultipleTeamsSuccessfully()
        {
            // Arrange
            var team1 = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };

            var team2 = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };

            // Act
            var result1 = await _teamRepository.CreateAsync(team1);
            var result2 = await _teamRepository.CreateAsync(team2);

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            var allTeams = await _teamRepository.GetAllAsync();
            Assert.Contains(allTeams, t => t.Id == team1.Id);
            Assert.Contains(allTeams, t => t.Id == team2.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotAllowDuplicateIds()
        {
            // Arrange
            var teamId = Guid.NewGuid();
            var team1 = new Team
            {
                Id = teamId,
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };

            var team2 = new Team
            {
                Id = teamId, // Same Id as team1
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };

            // Act
            await _teamRepository.CreateAsync(team1);
            var result = await _teamRepository.CreateAsync(team2); // This should fail

            // Assert
            Assert.False(result); // Should return false due to duplicate Id
            var allTeams = await _teamRepository.GetAllAsync();
            Assert.Single(allTeams); // Only one team should exist
            Assert.Contains(allTeams, t => t.Id == team1.Id); // Ensure the original team exists
        }

        // 2. Test GetByIdAsync Method
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectTeam()
        {
            // Arrange
            var team = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };
            await _teamRepository.CreateAsync(team);

            // Act
            var result = await _teamRepository.GetByIdAsync(team.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(team.Id, result?.Id);
            Assert.Equal(team.MatchId, result?.MatchId);
            Assert.Equal(team.Player1Id, result?.Player1Id);
            Assert.Equal(team.Player2Id, result?.Player2Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonexistentId()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _teamRepository.GetByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result);
        }

        // 3. Test GetAllAsync Method
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTeams()
        {
            // Arrange
            var team1 = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };
            var team2 = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };
            await _teamRepository.CreateAsync(team1);
            await _teamRepository.CreateAsync(team2);

            // Act
            var result = await _teamRepository.GetAllAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Contains(result, t => t.Id == team1.Id);
            Assert.Contains(result, t => t.Id == team2.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyCollectionWhenNoTeamsExist()
        {
            // Act
            var result = await _teamRepository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        // 4. Test UpdateAsync Method
        [Fact]
        public async Task UpdateAsync_ShouldUpdateTeamSuccessfully()
        {
            // Arrange
            var team = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };
            await _teamRepository.CreateAsync(team);

            var updatedTeam = new Team
            {
                Id = team.Id, // Keep the same Id
                MatchId = Guid.NewGuid(), // Change MatchId
                Player1Id = team.Player1Id,
                Player2Id = team.Player2Id
            };

            // Act
            var result = await _teamRepository.UpdateAsync(updatedTeam);

            // Assert
            Assert.True(result);
            var updatedResult = await _teamRepository.GetByIdAsync(team.Id);
            Assert.Equal(updatedTeam.MatchId, updatedResult?.MatchId);
        }


        [Fact]
        public async Task UpdateAsync_ShouldReturnFalseWhenTeamDoesNotExist()
        {
            // Arrange
            var nonExistentTeam = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };

            // Act
            var result = await _teamRepository.UpdateAsync(nonExistentTeam);

            // Assert
            Assert.False(result);
        }

        // 5. Test DeleteByIdAsync Method
        [Fact]
        public async Task DeleteByIdAsync_ShouldRemoveTeamSuccessfully()
        {
            // Arrange
            var team = new Team
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Player1Id = Guid.NewGuid(),
                Player2Id = Guid.NewGuid()
            };
            await _teamRepository.CreateAsync(team);

            // Act
            var result = await _teamRepository.DeleteByIdAsync(team.Id);

            // Assert
            Assert.True(result);
            var allTeams = await _teamRepository.GetAllAsync();
            Assert.DoesNotContain(allTeams, t => t.Id == team.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldReturnFalseWhenTeamDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _teamRepository.DeleteByIdAsync(nonExistentId);

            // Assert
            Assert.False(result);
        }
    }
}
