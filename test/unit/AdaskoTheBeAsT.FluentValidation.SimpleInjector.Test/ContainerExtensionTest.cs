using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector.Test
{
    public sealed partial class ContainerExtensionTest
        : IDisposable
    {
        private readonly Container _sut;

        public ContainerExtensionTest()
        {
            _sut = new Container();
        }

        public void Dispose()
        {
            _sut.Dispose();
        }

        [Fact]
        public void ShouldResolveValidatorWhenAssemblyParamsPassed()
        {
            // Arrange
            _sut.AddFluentValidation(typeof(Person).GetTypeInfo().Assembly);

            // Act
            var result = _sut.GetInstance<IValidator<Person>>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<PersonValidator>();
            }
        }

        [Fact]
        public void ShouldResolveValidatorWhenAssemblyEnumerablePassed()
        {
            // Arrange
            _sut.AddFluentValidation(new List<Assembly> { typeof(Person).GetTypeInfo().Assembly });

            // Act
            var result = _sut.GetInstance<IValidator<Person>>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<PersonValidator>();
            }
        }

        [Fact]
        public void ShouldResolveValidatorWhenMarkerTypeParamsPassed()
        {
            // Arrange
            _sut.AddFluentValidation(typeof(Person));

            // Act
            var result = _sut.GetInstance<IValidator<Person>>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<PersonValidator>();
            }
        }

        [Fact]
        public void ShouldResolveValidatorWhenMarkerTypeEnumerablePassed()
        {
            // Arrange
            _sut.AddFluentValidation(new List<Type> { typeof(Person) });

            // Act
            var result = _sut.GetInstance<IValidator<Person>>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<PersonValidator>();
            }
        }

        [Fact]
        public void ShouldResolveValidatorsWhenConfigurationPassed()
        {
            // Arrange
            _sut.AddFluentValidation(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(PersonValidator));
                });

            // Act
            var result = _sut.GetInstance<IValidator<Person>>();
            var result2 = _sut.GetInstance<IValidator<Car>>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<PersonValidator>();
                result2.Should().NotBeNull();
                result2.Should().BeOfType<NullValidator<Car>>();
            }
        }
    }
}
