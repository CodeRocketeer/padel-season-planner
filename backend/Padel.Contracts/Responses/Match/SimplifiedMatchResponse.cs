using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelContracts.Responses.Match
{
    public class SimplifiedMatchResponse
    {
        public int Id { get; set; }
        public DateTime MatchDate { get; set; }
        public int SeasonId { get; set; }
        public MatchTeamResponse Team1 { get; set; }
        public MatchTeamResponse Team2 { get; set; }
    }
}
