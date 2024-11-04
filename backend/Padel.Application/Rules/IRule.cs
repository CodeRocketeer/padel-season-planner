using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Padel.Domain.Models;

namespace Padel.Application.Rules;

public interface IRule
{
    int Weight { get; } // Importance of the rule
    int Validate(List<Match> matchSet , List<Player> playerList); // Method to validate matches
}
