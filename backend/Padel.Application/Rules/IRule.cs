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
    decimal Validate(Match match, List<Match> scheduledMatches); // Method to validate matches
}
