using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Models
{
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SeasonId { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }

        // You could add methods here related to player stats or activities
        public void UpdateParticipantDetails(string name, string gender)
        {
            Name = name;
            Gender = gender;
        }
    }
}
