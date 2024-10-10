using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Domain.Models
{
    public class Season
    {
        public int Id { get; set; } // Unique identifier for each season
        public string Title { get; set; } // Title of the season
        public string DayOfWeek { get; set; } // Day of the week when matches are scheduled
        public DateTime StartDate { get; set; } // Start date of the season
        public DateTime EndDate { get; set; } // End date of the season

        public List<Team> Teams { get; set; } = new List<Team>(); // Collection of teams in the season
        public List<Match> Matches { get; set; } = new List<Match>(); // Collection of matches in the season
    }
}
