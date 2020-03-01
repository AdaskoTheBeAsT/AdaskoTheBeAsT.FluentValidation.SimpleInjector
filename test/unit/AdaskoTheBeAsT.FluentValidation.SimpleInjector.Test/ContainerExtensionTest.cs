using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Xunit;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector.Test
{
    public sealed class ContainerExtensionTest
        : IDisposable
    {
        private readonly Container _sut;

        public ContainerExtensionTest()
        {
            _sut = new Container();
            _sut.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
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
                    config.WithAssemblyMarkerTypes(typeof(PersonValidator));
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

        [Fact]
        public void ShouldBeRegisteredAsSingletonWhenDefaultLifetime()
        {
            // Arrange
            _sut.AddFluentValidation(
                config =>
                {
                    config.WithAssemblyMarkerTypes(typeof(PersonValidator));
                });

            // Act
            var serviceDescriptors = _sut.GetCurrentRegistrations()
                .Where(r => r.ServiceType == typeof(IValidator<>));

            // Assert
            using (new AssertionScope())
            {
                serviceDescriptors.Select(sd => sd.Lifestyle).Should().AllBeEquivalentTo(Lifestyle.Singleton);
            }
        }

        [Fact]
        public void ShouldBeRegisteredAsSingletonWhenSingletonLifetime()
        {
            // Arrange
            _sut.AddFluentValidation(
                config =>
                {
                    config.WithAssemblyMarkerTypes(typeof(PersonValidator));
                    config.AsSingleton();
                });

            // Act
            var serviceDescriptors = _sut.GetCurrentRegistrations()
                .Where(r => r.ServiceType == typeof(IValidator<>));

            // Assert
            using (new AssertionScope())
            {
                serviceDescriptors.Select(sd => sd.Lifestyle).Should().AllBeEquivalentTo(Lifestyle.Singleton);
            }
        }

        [Fact]
        public void ShouldBeRegisteredAsScopedWhenScopedLifetime()
        {
            // Arrange
            _sut.AddFluentValidation(
                config =>
                {
                    config.WithAssemblyMarkerTypes(typeof(PersonValidator));
                    config.AsScoped();
                });

            // Act
            var serviceDescriptors = _sut.GetCurrentRegistrations()
                .Where(r => r.ServiceType == typeof(IValidator<>));

            // Assert
            using (new AssertionScope())
            {
                serviceDescriptors.Select(sd => sd.Lifestyle).Should().AllBeEquivalentTo(Lifestyle.Scoped);
            }
        }

        [Fact]
        public void ShouldBeRegisteredAsTransientWhenTransientLifetime()
        {
            // Arrange
            _sut.AddFluentValidation(
                config =>
                {
                    config.WithAssemblyMarkerTypes(typeof(PersonValidator));
                    config.AsTransient();
                });

            // Act
            var serviceDescriptors = _sut.GetCurrentRegistrations()
                .Where(r => r.ServiceType == typeof(IValidator<>));

            // Assert
            using (new AssertionScope())
            {
                serviceDescriptors.Select(sd => sd.Lifestyle).Should().AllBeEquivalentTo(Lifestyle.Transient);
            }
        }
    }
}
