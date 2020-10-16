using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using SimpleInjector;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector
{
    /// <summary>
    /// FluentValidation SimpleInjector configuration file.
    /// </summary>
    public class FluentValidationSimpleInjectorConfiguration
    {
        public FluentValidationSimpleInjectorConfiguration()
        {
            Lifestyle = Lifestyle.Singleton;
            AssembliesToScan = Enumerable.Empty<Assembly>();
            ValidatorRegistrationKind = ValidatorRegistrationKind.SingleValidator;
        }

        /// <summary>
        /// Lifestyle in which <see cref="IValidator{T}"/> implementation will be registered.
        /// Default is <see cref="Lifestyle"/>.Singleton.
        /// </summary>
        public Lifestyle Lifestyle { get; private set; }

        /// <summary>
        /// Assemblies which will be scanned for
        /// auto registering types implementing <see cref="IValidator{T}"/> interface.
        /// </summary>
        public IEnumerable<Assembly> AssembliesToScan { get; private set; }

        /// <summary>
        /// Register validators kind.
        /// AsSingle - open generic registered as single type
        /// - one implementation of validator for given entity allowed.
        /// Combining validation rules needs to be done by including rules as described on
        /// https://docs.fluentvalidation.net/en/latest/including-rules.html.
        /// Then for other composite parts special attribute can be used <see cref="SkipValidatorRegistrationAttribute"/>
        /// to skip registration of part of the composite.
        /// AsCollection - open generic registered as collection - multiple validators per same type allowed.
        /// </summary>
        public ValidatorRegistrationKind ValidatorRegistrationKind { get; private set; }

        /// <summary>
        /// Set lifestyle of
        /// <see cref="IValidator{T}"/> implementations to <see cref="Lifestyle"/>.Singleton.
        /// </summary>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with <see cref="IValidator{T}"/> implementations to <see cref="Lifestyle"/>.Singleton.</returns>
        public FluentValidationSimpleInjectorConfiguration AsSingleton()
        {
            Lifestyle = Lifestyle.Singleton;
            return this;
        }

        /// <summary>
        /// Set lifestyle of
        /// <see cref="IValidator{T}"/> implementations to <see cref="Lifestyle"/>.Scoped.
        /// </summary>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with <see cref="IValidator{T}"/> implementations to <see cref="Lifestyle"/>.Scoped.</returns>
        public FluentValidationSimpleInjectorConfiguration AsScoped()
        {
            Lifestyle = Lifestyle.Scoped;
            return this;
        }

        /// <summary>
        /// Set lifestyle of
        /// <see cref="IValidator{T}"/> implementations to <see cref="Lifestyle"/>.Transient.
        /// </summary>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with <see cref="IValidator{T}"/> implementations to <see cref="Lifestyle"/>.Transient.</returns>
        public FluentValidationSimpleInjectorConfiguration AsTransient()
        {
            Lifestyle = Lifestyle.Transient;
            return this;
        }

        /// <summary>
        /// Setup assemblies which will be scanned for
        /// auto registering types implementing MediatR interfaces.
        /// </summary>
        /// <param name="assembliesToScan">Assemblies which will be scanned for
        /// auto registering types.</param>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with assemblies to scan configured.</returns>
        public FluentValidationSimpleInjectorConfiguration WithAssembliesToScan(params Assembly[] assembliesToScan)
        {
            AssembliesToScan = assembliesToScan;
            return this;
        }

        /// <summary>
        /// Setup assemblies which will be scanned for
        /// auto registering types implementing <see cref="IValidator{T}"/> interface.
        /// </summary>
        /// <param name="assembliesToScan">Assemblies which will be scanned for
        /// auto registering types.</param>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with assemblies to scan configured.</returns>
        public FluentValidationSimpleInjectorConfiguration WithAssembliesToScan(IEnumerable<Assembly> assembliesToScan)
        {
            AssembliesToScan = assembliesToScan;
            return this;
        }

        /// <summary>
        /// Setup assemblies which will be scanned for
        /// auto registering types implementing <see cref="IValidator{T}"/> interface.
        /// by types from given assemblies (marker types).
        /// </summary>
        /// <param name="handlerAssemblyMarkerTypes">Types from assemblies which will be scanned for
        /// auto registering types.</param>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with assemblies to scan configured.</returns>
        public FluentValidationSimpleInjectorConfiguration WithAssemblyMarkerTypes(params Type[] handlerAssemblyMarkerTypes)
        {
            AssembliesToScan = handlerAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly);
            return this;
        }

        /// <summary>
        /// Setup assemblies which will be scanned for
        /// auto registering types implementing <see cref="IValidator{T}"/> interface.
        /// by types from given assemblies (marker types).
        /// </summary>
        /// <param name="handlerAssemblyMarkerTypes">Types from assemblies which will be scanned for
        /// auto registering types.</param>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with assemblies to scan configured.</returns>
        public FluentValidationSimpleInjectorConfiguration WithAssemblyMarkerTypes(IEnumerable<Type> handlerAssemblyMarkerTypes)
        {
            AssembliesToScan = handlerAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly);
            return this;
        }

        /// <summary>
        /// Set registration as single validator per given target type.
        /// One implementation of validator for given entity allowed.
        /// Combining validation rules needs to be done by including rules as described on
        /// https://docs.fluentvalidation.net/en/latest/including-rules.html.
        /// Then for other composite parts special attribute can be used <see cref="SkipValidatorRegistrationAttribute"/>
        /// to skip registration of part of the composite.
        /// </summary>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with single validator per given target type.</returns>
        public FluentValidationSimpleInjectorConfiguration RegisterAsSingleValidator()
        {
            this.ValidatorRegistrationKind = ValidatorRegistrationKind.SingleValidator;
            return this;
        }

        /// <summary>
        /// Set registration as validator collection per given target type.
        /// </summary>
        /// <returns><see cref="FluentValidationSimpleInjectorConfiguration"/>
        /// with single validator per given target type.</returns>
        public FluentValidationSimpleInjectorConfiguration RegisterAsValidatorCollection()
        {
            this.ValidatorRegistrationKind = ValidatorRegistrationKind.ValidatorCollection;
            return this;
        }
    }
}
