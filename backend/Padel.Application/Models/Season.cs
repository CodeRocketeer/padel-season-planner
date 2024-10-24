using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Models
{
    public class Season
    {
        public Guid Id { get; set; }
        public int AmountOfMatches { get; set; }
        public DateTime StartDate { get; set; }
        public string Title { get; set; }
        public int DayOfWeek { get; set; }

        public bool? UserParticipates { get; set; }

        public List<Team> Teams { get; set; }
        public List<Match> Matches { get; set; }

        public Season()
        {
            Teams = new List<Team>();
            Matches = new List<Match>();
        }

        // Method to add a team to the season
        public void AddTeam(Team team)
        {
            Teams.Add(team);
        }

        // Method to schedule a match in the season
        public void ScheduleMatch(Match match)
        {
            Matches.Add(match);
        }
    }
}
