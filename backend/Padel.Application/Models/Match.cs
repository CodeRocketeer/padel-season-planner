using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Domain.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; } // Link to the season
        public Guid Team1Id { get; set; } // ID of the first team
        public Guid Team2Id { get; set; } // ID of the second team
        public DateTime MatchDate { get; set; }

        public List<Team> Teams { get; set; }


    }
}
