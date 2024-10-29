using Padel.Application.Database.Entities;
using Padel.Domain.Models;

namespace Padel.Application.Mappers;

public static class TeamMapper
{
    // Method to convert a Team model to a TeamEntity
    public static TeamEntity ToEntity(this Team team)
    {
        if (team == null)
            throw new ArgumentNullException(nameof(team));

        return new TeamEntity
        {
            MatchId = team.MatchId,
            Id = team.Id != 0 ? team.Id : 0,
            Players = new List<PlayerEntity>
            {
                new PlayerEntity { UserId = team.Player1.UserId, Name = team.Player1.Name, Gender = team.Player1.Gender },
                new PlayerEntity { UserId = team.Player2.UserId, Name = team.Player2.Name, Gender = team.Player2.Gender }
            }
        };
    }

    // Method to convert a TeamEntity to a Team model
    public static Team FromEntity(this TeamEntity teamEntity)
    {
        if (teamEntity == null)
            throw new ArgumentNullException(nameof(teamEntity));

        var players = teamEntity.Players?.ToList() ?? new List<PlayerEntity>();
        if (players.Count != 2)
        {
            throw new ArgumentException("Team must have exactly two players.", nameof(teamEntity.Players));
        }

        var player1 = new Player(players[0].Gender, players[0].Name, players[0].UserId) { Id = players[0].Id };
        var player2 = new Player(players[1].Gender, players[1].Name, players[1].UserId) { Id = players[1].Id };

        return new Team(player1, player2)
        {
            Id = teamEntity.Id,
            MatchId = teamEntity.MatchId
        };
    }
}