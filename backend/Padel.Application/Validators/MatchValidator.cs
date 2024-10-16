using FluentValidation;
using Padel.Application.Repositories;
using Padel.Domain.Models;

namespace Padel.Application.Validators
{
    public class MatchValidator : AbstractValidator<Match>
    {

        public MatchValidator()
        {
         
            RuleFor(match => match.Team1Id)
                .NotEmpty()
                .WithMessage("Team1Id is required");
            RuleFor(match => match.Team2Id)
                .NotEmpty()
                .WithMessage("Team2Id is required")
                .Must((match, team2Id) => team2Id != match.Team1Id)
                .WithMessage("Team1Id and Team2Id must be different");
            RuleFor(match => match.SeasonId)
                .NotEmpty()
                .WithMessage("SeasonId is required");

            RuleFor(match => match.MatchDate)
                .NotEmpty()
                .WithMessage("MatchDate is required")
                .GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(match => match)
                .Must(NoSharedPlayersBetweenTeams)
                .WithMessage("A player cannot be in both teams for the same match.");
            


        }


        // Synchronous method to check if teams share players
        private bool NoSharedPlayersBetweenTeams(Match match)
        {
            if (match.Teams == null || match.Teams.Count != 2)
            {
                return false; // Invalid match structure; it must have exactly two teams.
            }

            // Extract player IDs from both teams
            var team1Players = match.Teams[0].Players.Select(p => p.Id).ToList();
            var team2Players = match.Teams[1].Players.Select(p => p.Id).ToList();

            // Check if any player is shared between the two teams
            return !team1Players.Intersect(team2Players).Any();
        }


    }
}