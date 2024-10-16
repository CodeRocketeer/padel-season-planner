using Padel.Application.Repositories;
using Padel.Domain.Models;

public class TeamRepositoryTests
{
    private readonly TeamRepository _teamRepository;

    public TeamRepositoryTests()
    {
        _teamRepository = new TeamRepository();
    }

    // Helper method to create a sample team for testing
    private Team CreateSampleTeam()
    {
        return new Team
        {
            Id = Guid.NewGuid(),
            SeasonId = Guid.NewGuid(),
            Player1Id = Guid.NewGuid(),
            Player2Id = Guid.NewGuid()
        };
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Team()
    {
        // Arrange
        var team = CreateSampleTeam();

        // Act
        var result = await _teamRepository.CreateAsync(team);

        // Assert
        Assert.True(result);
        Assert.True(await _teamRepository.ExistsByIdAsync(team.Id));
    }

    [Fact]
    public async Task CreateAsync_Should_Not_Add_DuplicateTeam()
    {
        // Arrange
        var team = CreateSampleTeam();
        await _teamRepository.CreateAsync(team);

        // Act
        var result = await _teamRepository.CreateAsync(team); // Attempt to create the same team

        // Assert
        Assert.False(result); // Should return false due to duplicate
    }

    [Fact]
    public async Task DeleteByIdAsync_Should_Remove_Team()
    {
        // Arrange
        var team = CreateSampleTeam();
        await _teamRepository.CreateAsync(team);

        // Act
        var result = await _teamRepository.DeleteByIdAsync(team.Id);

        // Assert
        Assert.True(result);
        Assert.False(await _teamRepository.ExistsByIdAsync(team.Id)); // Ensure it's deleted
    }

    [Fact]
    public async Task DeleteByIdAsync_Should_ReturnFalse_When_TeamDoesNotExist()
    {
        // Arrange
        var nonExistentTeamId = Guid.NewGuid();

        // Act
        var result = await _teamRepository.DeleteByIdAsync(nonExistentTeamId);

        // Assert
        Assert.False(result); // Deletion should fail for a non-existent team
    }

    [Fact]
    public async Task ExistsByIdAsync_Should_ReturnTrue_IfTeamExists()
    {
        // Arrange
        var team = CreateSampleTeam();
        await _teamRepository.CreateAsync(team);

        // Act
        var result = await _teamRepository.ExistsByIdAsync(team.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_Should_ReturnFalse_IfTeamDoesNotExist()
    {
        // Act
        var result = await _teamRepository.ExistsByIdAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
    [Fact]
    public async Task GetAllAsync_Should_ReturnAllTeams()
    {
        // Arrange
        var team1 = CreateSampleTeam();
        var team2 = CreateSampleTeam();
        await _teamRepository.CreateAsync(team1);
        await _teamRepository.CreateAsync(team2);

        // Act
        var teams = await _teamRepository.GetAllAsync();

        // Assert
        Assert.NotNull(teams);
        Assert.Equal(2, teams.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Team_IfExists()
    {
        // Arrange
        var team = CreateSampleTeam();
        await _teamRepository.CreateAsync(team);

        // Act
        var retrievedTeam = await _teamRepository.GetByIdAsync(team.Id);

        // Assert
        Assert.NotNull(retrievedTeam);
        Assert.Equal(team.Id, retrievedTeam?.Id);
        Assert.Equal(team.SeasonId, retrievedTeam?.SeasonId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_IfTeamDoesNotExist()
    {
        // Act
        var team = await _teamRepository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(team);
    }
    [Fact]
public async Task UpdateAsync_Should_Update_Team_IfExists()
{
    // Arrange
    var team = CreateSampleTeam();
    await _teamRepository.CreateAsync(team);
    
    var updatedSeasonId = Guid.NewGuid();
    team.SeasonId = updatedSeasonId;

    // Act
    var result = await _teamRepository.UpdateAsync(team);

    // Assert
    Assert.True(result);

    // Verify the update
    var updatedTeam = await _teamRepository.GetByIdAsync(team.Id);
    Assert.Equal(updatedSeasonId, updatedTeam?.SeasonId);
}

[Fact]
public async Task UpdateAsync_Should_ReturnFalse_IfTeamDoesNotExist()
{
    // Arrange
    var team = CreateSampleTeam();

    // Act
    var result = await _teamRepository.UpdateAsync(team);

    // Assert
    Assert.False(result); // Update should fail since the team does not exist
}
}
