using FluentValidation;
using Padel.Application.Models;

namespace Padel.Application.Validators.ParticipantValidator;

public class ParticipantValidator : AbstractValidator<Participant>
{


    public ParticipantValidator()
    {


        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.SeasonId)
                .NotEmpty();


        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Gender)
            .NotEmpty()
            .Must(gender => gender == "M" || gender == "F")
            .WithMessage("Gender must be either 'M' or 'F'");

    }


}
