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
        public void ShouldThrowExceptionWhenNullContainerPassed()
        {
            // Arrange
            const Container? container = null;
#pragma warning disable CS8604 // Possible null reference argument.

            // ReSharper disable once InvokeAsExtensionMethod
#pragma warning disable CC0026 // Call Extension Method As Extension
            Action action = () => ContainerExtension.AddFluentValidation(container!, _ => { });
#pragma warning restore CC0026 // Call Extension Method As Extension
#pragma warning restore CS8604 // Possible null reference argument.

            // Act and Assert
            action.Should().Throw<ArgumentNullException>();
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
                config => config.WithAssemblyMarkerTypes(typeof(PersonValidator)));

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
                config => config.WithAssemblyMarkerTypes(typeof(PersonValidator)));

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

        [Fact]
        public void ShouldBeRegisteredAsSingleValidator()
        {
            // Arrange
            _sut.AddFluentValidation(
                config =>
                {
                    config.WithAssemblyMarkerTypes(typeof(PersonValidator));
                    config.RegisterAsSingleValidator();
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
        public void ShouldBeRegisteredAValidatorCollection()
        {
            // Arrange
            _sut.AddFluentValidation(
                config =>
                {
                    config.WithAssemblyMarkerTypes(typeof(PersonValidator));
                    config.RegisterAsValidatorCollection();
                });

            // Act
            var result = _sut.GetAllInstances<IValidator<Person>>().ToList();
            var result2 = _sut.GetAllInstances<IValidator<Car>>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().HaveCount(1);
                result[0].Should().BeOfType<PersonValidator>();
                result2.Should().HaveCount(0);
            }
        }
    }
}
