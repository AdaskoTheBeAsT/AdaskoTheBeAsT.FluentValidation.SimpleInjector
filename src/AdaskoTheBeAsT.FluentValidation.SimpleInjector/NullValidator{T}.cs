using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector
{
#pragma warning disable CA1710 // Identifiers should have correct suffix
    internal sealed class NullValidator<T>
        : AbstractValidator<T>
    {
        private readonly ValidationResult _validResult = new ValidationResult();

        public override ValidationResult Validate(ValidationContext<T> context) => _validResult;

        public override Task<ValidationResult> ValidateAsync(
            ValidationContext<T> context,
            CancellationToken cancellation = default) => Task.FromResult(_validResult);
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}
