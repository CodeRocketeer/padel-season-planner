using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Database.Entities
{
    public class Team
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Guid Participant1Id { get; set; }
        public Guid Participant2Id { get; set; }
    }
}
