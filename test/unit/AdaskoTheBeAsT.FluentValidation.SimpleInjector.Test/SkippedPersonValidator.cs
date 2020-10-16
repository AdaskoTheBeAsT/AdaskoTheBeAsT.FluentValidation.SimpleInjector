using FluentValidation;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector.Test
{
    [SkipValidatorRegistration]
    public class SkippedPersonValidator
        : AbstractValidator<Person>
    {
        public SkippedPersonValidator()
        {
            RuleFor(p => p.Id > 0);
        }
    }
}
