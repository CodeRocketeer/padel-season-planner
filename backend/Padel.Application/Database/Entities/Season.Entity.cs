using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Database.Entities
{
    public class Season
    {
        public Guid Id { get; set; }
        public int AmountOfMatches { get; set; }
        public DateTime StartDate { get; set; }
        public string Title { get; set; }
        public int DayOfWeek { get; set; }
    }
    
}

