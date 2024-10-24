using FluentValidation;
using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;

namespace Padel.Application.Validators.SeasonValidator;

public class SeasonValidator : AbstractValidator<Season>
{
    private readonly ISeasonRepository _seasonRepository;

    public SeasonValidator(ISeasonRepository seasonRepository)
    {
        _seasonRepository = seasonRepository;

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
                .NotEmpty();

        RuleFor(x => x.AmountOfMatches)
            .NotEmpty()
            .GreaterThan(0);


        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow);

        RuleFor(x => x.DayOfWeek)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(6);
    }

    private async Task<bool> ValidateSlug(Season Season, string slug, CancellationToken token = default)
    {
        var existingSeason = await _seasonRepository.GetBySlugAsync(slug);

        if (existingSeason is not null)
        {
            return existingSeason.Id == Season.Id;
        }

        return existingSeason is null;
    }
}