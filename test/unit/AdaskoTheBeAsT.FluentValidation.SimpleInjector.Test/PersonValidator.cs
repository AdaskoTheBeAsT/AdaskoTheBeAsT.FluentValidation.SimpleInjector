using FluentValidation;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector.Test
{
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public class PersonValidator
        : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.Id > 0);
            RuleFor(p => !string.IsNullOrEmpty(p.Name));
        }
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}
