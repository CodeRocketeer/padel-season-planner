
using FluentValidation;
using Padel.Application.Models;
using Padel.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Validators
{
    public class TeamValidator : AbstractValidator<Team>
    {

        private readonly ITeamRepository _teamRepository;
        public TeamValidator(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
            // Validate that Id is not an empty Guid (if you want to enforce this)
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Team ID cannot be empty.");

            // Validate that MatchId is a valid Guid
            RuleFor(x => x.MatchId)
                .NotEmpty().WithMessage("Match ID cannot be empty.");

            // Validate that Player1Id is a valid Guid
            RuleFor(x => x.Player1Id)
                .NotEmpty().WithMessage("Player 1 ID cannot be empty.");

            // Validate that Player2Id is a valid Guid
            RuleFor(x => x.Player2Id)
                .NotEmpty().WithMessage("Player 2 ID cannot be empty.");

            // You can add more rules depending on the context of your application
            // For example, checking if Player1Id and Player2Id are not the same
            RuleFor(x => x)
                .Must(x => x.Player1Id != x.Player2Id)
                .WithMessage("Player 1 and Player 2 cannot be the same.");
        }
    }
}
