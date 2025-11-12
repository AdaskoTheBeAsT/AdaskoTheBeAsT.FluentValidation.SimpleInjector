using System.Threading.Tasks;
using AwesomeAssertions;
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
            var context = new ValidationContext<Person?>(instanceToValidate: null);

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
        public async Task ValidateAsyncShouldReturnTrueWhenNullPassedAsync()
        {
            // Arrange
            var context = new ValidationContext<Person?>(instanceToValidate: null);

            // Act
            var result = await _sut.ValidateAsync(context);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsyncShouldReturnTrueWhenDefaultPassedAsync()
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
