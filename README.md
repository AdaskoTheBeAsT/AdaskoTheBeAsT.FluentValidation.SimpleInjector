# AdaskoTheBeAsT.FluentValidation.SimpleInjector

FluentValidation extensions to SimpleInjector

## Badges

[![CodeFactor](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.fluentvalidation.simpleinjector/badge)](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.fluentvalidation.simpleinjector)
[![Build Status](https://img.shields.io/azure-devops/build/adaskothebeast/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14)](https://img.shields.io/azure-devops/build/adaskothebeast/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/AdaskoTheBeAsT/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/AdaskoTheBeAsT/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14?style=plastic)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector&metric=alert_status)](https://sonarcloud.io/dashboard?id=AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector)
![Sonar Tests](https://img.shields.io/sonar/tests/AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Test Count](https://img.shields.io/sonar/total_tests/AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Test Execution Time](https://img.shields.io/sonar/test_execution_time/AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Coverage](https://img.shields.io/sonar/coverage/AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io&style=plastic)
![Nuget](https://img.shields.io/nuget/dt/AdaskoTheBeAsT.FluentValidation.SimpleInjector)

## Usage

This library scans assemblies and adds implementations of `IValidator<>` to the SimpleInjector container.

There are few options to use with `Container` instance:

1. Marker type from assembly which will be scanned

   ```cs
    container.AddFluentValidation(typeof(MyValidator), type2 /*, ...*/);
   ```

1. List of assemblies which will be scanned.

   Below is sample for scanning assemblies from some solution.

    ```cs
    [ExcludeFromCodeCoverage]
    public static class FluentValidationConfigurator
    {
        private const string NamespacePrefix = "YourNamespace";

        public static void Configure(Container container)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var assemblies = new List<Assembly>();
            var mainAssembly = typeof(FluentValidationConfigurator).Assembly;
            var refAssemblies = mainAssembly.GetReferencedAssemblies();
            foreach (var assemblyName in refAssemblies
                .Where(a => a.FullName.StartsWith(NamespacePrefix, StringComparison.OrdinalIgnoreCase)))
                {
                    var assembly = loadedAssemblies.Find(l => l.FullName == assemblyName.FullName)
                        ?? AppDomain.CurrentDomain.Load(assemblyName);
                    assemblies.Add(assembly);
                }
            container.AddFluentValidation(assemblies);
        }
    }
   ```

1. Special configuration action with different lifetime for validators

   ```cs
    container.AddFluentValidation(
        cfg =>
        {
            cfg.WithAssembliesToScan(assemblies);
            cfg.AsScoped();
        });
   ```  