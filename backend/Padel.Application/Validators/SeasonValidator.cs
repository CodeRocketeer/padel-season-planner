using FluentValidation;
using Padel.Application.Repositories;
using Padel.Domain.Models;

namespace Padel.Application.Validators
{
    public class SeasonValidator : AbstractValidator<Season>
    {
        private readonly ISeasonRepository _seasonRepository;


        public SeasonValidator(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(3)
                .WithMessage("Name is not valid");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage("Start date is not valid");

            RuleFor(x => x.AmountOfMatches)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Amount of matches is not valid");

            RuleFor(x => x.DayOfWeek)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(6)
                .WithMessage("Day of week is not valid");

        }
    }
}
