using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector.Test
{
    public sealed class NullValidatorTest
    {
        private readonly NullValidator<Person?> _sut;

        public NullValidatorTest()
        {
            _sut = new NullValidator<Person?>();
        }

        [Fact]
        public void ValidateShouldReturnTrueWhenNullPassed()
        {
            // Arrange
            var context = new ValidationContext<Person?>(null);

            // Act
            var result = _sut.Validate(context);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateShouldReturnTrueWhenDefaultPassed()
        {
            // Arrange
            var context = new ValidationContext<Person?>(new Person());

            // Act
            var result = _sut.Validate(context);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsyncShouldReturnTrueWhenNullPassed()
        {
            // Arrange
            var context = new ValidationContext<Person?>(null);

            // Act
            var result = await _sut.ValidateAsync(context);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsyncShouldReturnTrueWhenDefaultPassed()
        {
            // Arrange
            var context = new ValidationContext<Person?>(new Person());

            // Act
            var result = await _sut.ValidateAsync(context);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
