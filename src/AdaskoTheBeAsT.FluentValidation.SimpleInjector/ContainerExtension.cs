using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using SimpleInjector;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector
{
    /// <summary>
    /// Extensions to scan for <see cref="IValidator{T}"/> implementations
    /// and registers them in SimpleInjector <see cref="Container"/>.
    /// </summary>
    public static class ContainerExtension
    {
        /// <summary>
        /// Registers types implementing <see cref="IValidator{T}"/> interface from the specified assemblies.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="assemblies">Assemblies to scan.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddFluentValidation(
            this Container container,
            params Assembly[] assemblies)
        {
            return AddFluentValidation(
                container,
                config => config.WithAssembliesToScan(assemblies));
        }

        /// <summary>
        /// Registers types implementing <see cref="IValidator{T}"/> interface from the specified assemblies.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="assemblies">Assemblies to scan.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddFluentValidation(
            this Container container,
            IEnumerable<Assembly> assemblies)
        {
            return AddFluentValidation(
                container,
                config => config.WithAssembliesToScan(assemblies));
        }

        /// <summary>
        /// Registers types implementing <see cref="IValidator{T}"/> interface from the assemblies that contain the specified types.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="handlerAssemblyMarkerTypes">Types used to mark assemblies to scan.</param>.
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddFluentValidation(
            this Container container,
            params Type[] handlerAssemblyMarkerTypes)
        {
            return AddFluentValidation(
                container,
                config => config.WithAssemblyMarkerTypes(handlerAssemblyMarkerTypes));
        }

        /// <summary>
        /// Registers types implementing <see cref="IValidator{T}"/> interface from the assemblies that contain the specified types.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="handlerAssemblyMarkerTypes">Types used to mark assemblies to scan.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddFluentValidation(
            this Container container,
            IEnumerable<Type> handlerAssemblyMarkerTypes)
        {
            return AddFluentValidation(
                container,
                config => config.WithAssemblyMarkerTypes(handlerAssemblyMarkerTypes));
        }

#pragma warning disable 8632
        /// <summary>
        /// Registers types implementing <see cref="IValidator{T}"/> interface from the specified assemblies.
        /// </summary>
        /// <param name="container"><see cref="Container"/>.</param>
        /// <param name="configuration">The action used to configure the options.</param>
        /// <returns><see cref="Container"/>.</returns>
        public static Container AddFluentValidation(
            this Container container,
            Action<FluentValidationSimpleInjectorConfiguration>? configuration)
        {
#if NETSTANDARD2_0 || NETSTANDARD2_1
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }
#endif

#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(container);
#endif

            var serviceConfig = new FluentValidationSimpleInjectorConfiguration();
            configuration?.Invoke(serviceConfig);

            return SetupContainer(container, serviceConfig);
        }
#pragma warning restore 8632

        internal static Container SetupContainer(
            this Container container,
            FluentValidationSimpleInjectorConfiguration serviceConfig)
        {
            var uniqueAssemblies = serviceConfig.AssembliesToScan.Distinct().ToArray();

            var typesToRegister = container.GetTypesToRegister(typeof(IValidator<>), uniqueAssemblies);
            var nonSkippedTypes = typesToRegister.Where(
                type =>
                    type.GetCustomAttribute<SkipValidatorRegistrationAttribute>() is null);

            if (serviceConfig.ValidatorRegistrationKind == ValidatorRegistrationKind.SingleValidator)
            {
                return RegisterAsSingleValidator(container, serviceConfig, nonSkippedTypes);
            }

            return RegisterAsValidatorCollection(container, serviceConfig, nonSkippedTypes);
        }

        internal static Container RegisterAsSingleValidator(
            this Container container,
            FluentValidationSimpleInjectorConfiguration serviceConfig,
            IEnumerable<Type> nonSkippedTypes)
        {
            container.Register(typeof(IValidator<>), nonSkippedTypes, serviceConfig.Lifestyle);
            container.RegisterConditional(
                typeof(IValidator<>),
                typeof(NullValidator<>),
                c => !c.Handled);

            return container;
        }

        internal static Container RegisterAsValidatorCollection(
            this Container container,
            FluentValidationSimpleInjectorConfiguration serviceConfig,
            IEnumerable<Type> nonSkippedTypes)
        {
            container.Collection.Register(typeof(IValidator<>), nonSkippedTypes, serviceConfig.Lifestyle);

            return container;
        }
    }
}
