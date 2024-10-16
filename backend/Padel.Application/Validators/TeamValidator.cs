using FluentValidation;
using Padel.Application.Repositories;
using Padel.Domain.Models;

namespace Padel.Application.Validators
{
    public class TeamValidator : AbstractValidator<Team>
    {



        public TeamValidator()
        {
            // Ensure a team doesn't have the same player twice
            RuleFor(team => team.Id).NotEmpty();
            RuleFor(team => team.SeasonId).NotEmpty();
            RuleFor(team => team.Player1Id).NotEmpty().WithMessage("Player1Id is required");

            RuleFor(team => team.Player2Id).NotEmpty().WithMessage("Player2Id is required");
            RuleFor(team => team)
                .Must(t => t.Player1Id != t.Player2Id)
                .WithMessage("A team cannot have the same player in both positions.");
            

        }
    }
}
