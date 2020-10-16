using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector.Test
{
    public sealed class FluentValidationSimpleInjectorConfigurationTest
    {
        private readonly FluentValidationSimpleInjectorConfiguration _sut;

        public FluentValidationSimpleInjectorConfigurationTest()
        {
            _sut = new FluentValidationSimpleInjectorConfiguration();
        }

        [Fact]
        public void ShouldContainDefaultSettingsAfterCreation()
        {
            // Arrange
            var config = new FluentValidationSimpleInjectorConfiguration();

            // Assert
            using (new AssertionScope())
            {
                config.Lifestyle.Should().Be(Lifestyle.Singleton);
                config.AssembliesToScan.Should().BeEmpty();
            }
        }

        [Fact]
        public void AsSingletonShouldSetSingleton()
        {
            // Act
            var result = _sut.AsSingleton();

            // Assert
            result.Lifestyle.Should().Be(Lifestyle.Singleton);
        }

        [Fact]
        public void AsScopedShouldSetScoped()
        {
            // Act
            var result = _sut.AsScoped();

            // Assert
            result.Lifestyle.Should().Be(Lifestyle.Scoped);
        }

        [Fact]
        public void AsTransientShouldSetTransient()
        {
            // Act
            var result = _sut.AsTransient();

            // Assert
            result.Lifestyle.Should().Be(Lifestyle.Transient);
        }

        [Fact]
        public void WithAssembliesToScanShouldSetAssembliesCorrectlyWhenPassedParams()
        {
            // Arrange
            var assemblyToScan = typeof(FluentValidationSimpleInjectorConfigurationTest).GetTypeInfo().Assembly;

            // Act
            var result = _sut.WithAssembliesToScan(
                assemblyToScan);

            // Assert
            using (new AssertionScope())
            {
                result.AssembliesToScan.Should().HaveCount(1);
                result.AssembliesToScan.First().Should().BeSameAs(assemblyToScan);
            }
        }

        [Fact]
        public void WithAssembliesToScanShouldSetAssembliesCorrectlyWhenPassedIEnumerable()
        {
            // Arrange
            var assemblyToScan = typeof(FluentValidationSimpleInjectorConfigurationTest).GetTypeInfo().Assembly;

            // Act
            var result = _sut.WithAssembliesToScan(
                new List<Assembly> { assemblyToScan });

            // Assert
            using (new AssertionScope())
            {
                result.AssembliesToScan.Should().HaveCount(1);
                result.AssembliesToScan.First().Should().BeSameAs(assemblyToScan);
            }
        }

        [Fact]
        public void WithHandlerAssemblyMarkerTypesShouldSetAssembliesCorrectlyWhenPassedParams()
        {
            // Arrange
            var handlerAssemblyMarkerType = typeof(FluentValidationSimpleInjectorConfigurationTest);

            // Act
            var result = _sut.WithAssemblyMarkerTypes(
                handlerAssemblyMarkerType);

            // Assert
            // Assert
            using (new AssertionScope())
            {
                result.AssembliesToScan.Should().HaveCount(1);
                result.AssembliesToScan.First().Should().BeSameAs(handlerAssemblyMarkerType.GetTypeInfo().Assembly);
            }
        }

        [Fact]
        public void WithHandlerAssemblyMarkerTypesShouldSetAssembliesCorrectlyWhenPassedIEnumerable()
        {
            // Arrange
            var handlerAssemblyMarkerType = typeof(FluentValidationSimpleInjectorConfigurationTest);

            // Act
            var result = _sut.WithAssemblyMarkerTypes(
                new List<Type> { handlerAssemblyMarkerType });

            // Assert
            // Assert
            using (new AssertionScope())
            {
                result.AssembliesToScan.Should().HaveCount(1);
                result.AssembliesToScan.First().Should().BeSameAs(handlerAssemblyMarkerType.GetTypeInfo().Assembly);
            }
        }

        [Fact]
        public void RegisterAsSingleValidatorShouldSetSingleValidator()
        {
            // Act
            var result = _sut.RegisterAsSingleValidator();

            // Assert
            result.ValidatorRegistrationKind.Should().Be(ValidatorRegistrationKind.SingleValidator);
        }

        [Fact]
        public void RegisterAsValidatorCollectionShouldSetValidatorCollection()
        {
            // Act
            var result = _sut.RegisterAsValidatorCollection();

            // Assert
            result.ValidatorRegistrationKind.Should().Be(ValidatorRegistrationKind.ValidatorCollection);
        }
    }
}
