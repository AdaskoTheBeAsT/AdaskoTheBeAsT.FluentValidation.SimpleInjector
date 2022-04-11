# AdaskoTheBeAsT.FluentValidation.SimpleInjector

FluentValidation extensions to SimpleInjector

## Badges

[![CodeFactor](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.fluentvalidation.simpleinjector/badge)](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.fluentvalidation.simpleinjector)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/AdaskoTheBeAsT/AdaskoTheBeAsT.FluentValidation.SimpleInjector.svg?logo=lgtm&logoWidth=18)](https://lgtm.com/projects/g/AdaskoTheBeAsT/AdaskoTheBeAsT.FluentValidation.SimpleInjector/alerts/)
[![Build Status](https://adaskothebeast.visualstudio.com/AdaskoTheBeAsT.FluentValidation.SimpleInjector/_apis/build/status/AdaskoTheBeAsT.AdaskoTheBeAsT.FluentValidation.SimpleInjector?branchName=master)](https://adaskothebeast.visualstudio.com/AdaskoTheBeAsT.FluentValidation.SimpleInjector/_build/latest?definitionId=9&branchName=master)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/AdaskoTheBeAsT/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/AdaskoTheBeAsT/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14?style=plastic)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector&metric=alert_status)](https://sonarcloud.io/dashboard?id=AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector)
![Sonar Tests](https://img.shields.io/sonar/tests/AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Test Count](https://img.shields.io/sonar/total_tests/AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Test Execution Time](https://img.shields.io/sonar/test_execution_time/AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Coverage](https://img.shields.io/sonar/coverage/AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector?server=https%3A%2F%2Fsonarcloud.io&style=plastic)
![Nuget](https://img.shields.io/nuget/dt/AdaskoTheBeAsT.FluentValidation.SimpleInjector)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FAdaskoTheBeAsT%2FAdaskoTheBeAsT.FluentValidation.SimpleInjector.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FAdaskoTheBeAsT%2FAdaskoTheBeAsT.FluentValidation.SimpleInjector?ref=badge_shield)

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

## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FAdaskoTheBeAsT%2FAdaskoTheBeAsT.FluentValidation.SimpleInjector.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FAdaskoTheBeAsT%2FAdaskoTheBeAsT.FluentValidation.SimpleInjector?ref=badge_large)