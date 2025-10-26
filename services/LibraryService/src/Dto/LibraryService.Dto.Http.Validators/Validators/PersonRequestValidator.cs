using FluentValidation;

namespace LibraryService.Dto.Http.Validators.Validators;

public class PersonRequestValidator : PersonServiceValidatorBase<PersonRequest>
{
    public PersonRequestValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty().WithMessage(PropertyIsEmptyMsg);

        When(model => model.Age is not null, () =>
        {
            RuleFor(model => model.Age)
                .GreaterThan(0).WithMessage(PropertyIsLessOrEqualThanMsg);
        });
    }
}