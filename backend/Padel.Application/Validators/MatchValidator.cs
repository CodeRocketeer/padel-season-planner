using FluentValidation;
using Padel.Application.Repositories;
using Padel.Domain.Models;

namespace Padel.Application.Validators
{
    public class MatchValidator : AbstractValidator<Match>
    {
        private readonly IMatchRepository _matchRepository;


        public MatchValidator(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
            RuleFor(x => x.MatchDate)
            .NotEmpty()
            .WithMessage("MatchDate is not valid.");

            RuleFor(x => x.SeasonId)
            .NotEmpty()
            .WithMessage("SeasonId is not valid.");
            


        }
    }
}
