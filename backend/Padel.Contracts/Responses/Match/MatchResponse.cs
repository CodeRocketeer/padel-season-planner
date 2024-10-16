
using Padel.Contracts.Responses.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Padel.Contracts.Responses.Match
{
    public class MatchResponse
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }

        public Guid? Team1Id { get; set; }
        public Guid? Team2Id { get; set; }
        
        public DateTime MatchDate { get; set; }

        public List<TeamResponse>? Teams { get; set; }
      
    }
}
