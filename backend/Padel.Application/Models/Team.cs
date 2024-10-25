using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Models;

public class Team
{
    public Guid Id { get; set; }
    public Guid SeasonId { get; set; }
    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }


    public Team(Player player1, Player player2)
    {
        Player1 = player1 ?? throw new ArgumentNullException("player 1 may not be null");
        Player2 = player2 ?? throw new ArgumentNullException("player 2 may not be null");

        if (player1.Id == player2.Id)
        {
            throw new Exception("addjsa");
        }


        // Method to check if both players are valid and part of the same season
        //public bool AreParticipantsValidForSeason(Guid seasonId)
        //{
        //    return Player1.SeasonId == seasonId && Player2.SeasonId == seasonId;
        //}

        //public List<Player> GetParticipants() => new List<Player> { Player1, Player2 }; // <--->
    }
}