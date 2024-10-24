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
            RuleFor(team => team.Participant1).NotEmpty().WithMessage("Player1Id is required");

            RuleFor(team => team.Participant2).NotEmpty().WithMessage("Player2Id is required");
            RuleFor(team => team)
                .Must(t => t.Participant1.UserId != t.Participant2.UserId)
                .WithMessage("A team cannot have the same participants userIds in both positions.");
            RuleFor(team => team)
               .Must(t => t.Participant1.Id != t.Participant2.Id)
               .WithMessage("A team cannot have the same participants Ids in both positions.");

        }
    }
}