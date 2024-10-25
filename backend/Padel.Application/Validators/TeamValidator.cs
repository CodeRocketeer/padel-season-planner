using FluentValidation;
using Padel.Application.Models;
using Padel.Application.Repositories;


namespace Padel.Application.Validators.TeamValidator
{
    public class TeamValidator : AbstractValidator<Team>
    {



        public TeamValidator()
        {
            // Ensure a team doesn't have the same player twice
            RuleFor(team => team.Id).NotEmpty();
            RuleFor(team => team.SeasonId).NotEmpty();
            RuleFor(team => team.Player1).NotEmpty().WithMessage("Player1Id is required");

            RuleFor(team => team.Player2).NotEmpty().WithMessage("Player2Id is required");
            RuleFor(team => team)
                .Must(t => t.Player1.UserId != t.Player2.UserId)
                .WithMessage("A team cannot have the same participants userIds in both positions.");
            RuleFor(team => team)
               .Must(t => t.Player1.Id != t.Player2.Id)
               .WithMessage("A team cannot have the same participants Ids in both positions.");

        }
    }
}