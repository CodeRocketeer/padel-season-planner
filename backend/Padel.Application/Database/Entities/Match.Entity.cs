using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Database.Entities
{
    public class Match
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Guid Team1Id { get; set; }
        public Guid Team2Id { get; set; }
        public DateTime MatchDate { get; set; }
    }
}
