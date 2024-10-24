using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Database.Entities
{
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SeasonId { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
    }
    
}

