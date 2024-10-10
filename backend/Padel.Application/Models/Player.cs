using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Domain.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Sex { get; set; }
    }
}
