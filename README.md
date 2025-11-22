# AdaskoTheBeAsT.FluentValidation.SimpleInjector

> Seamlessly integrate [FluentValidation](https://fluentvalidation.net/) with [SimpleInjector](https://simpleinjector.org/) - automatic validator registration made simple.

[![CodeFactor](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.fluentvalidation.simpleinjector/badge)](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.fluentvalidation.simpleinjector)
[![Build Status](https://img.shields.io/azure-devops/build/adaskothebeast/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14)](https://img.shields.io/azure-devops/build/adaskothebeast/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/AdaskoTheBeAsT/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/AdaskoTheBeAsT/AdaskoTheBeAsT.FluentValidation.SimpleInjector/14?style=plastic)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector&metric=alert_status)](https://sonarcloud.io/dashboard?id=AdaskoTheBeAsT_AdaskoTheBeAsT.FluentValidation.SimpleInjector)
![Nuget](https://img.shields.io/nuget/dt/AdaskoTheBeAsT.FluentValidation.SimpleInjector)
![Nuget](https://img.shields.io/nuget/v/AdaskoTheBeAsT.FluentValidation.SimpleInjector)

---

## Why Use This?

Stop manually registering validators one by one. This library automatically scans your assemblies and registers all `IValidator<T>` implementations with SimpleInjector, saving you time and reducing boilerplate code.

### Key Features

- **Zero Configuration** - Works out of the box with sensible defaults
- **Flexible Scanning** - Register validators by assembly, marker types, or custom configuration
- **Lifecycle Control** - Choose between Singleton, Scoped, or Transient lifestyles
- **Smart Registration** - Supports both single validator and collection patterns
- **Selective Registration** - Skip specific validators with `[SkipValidatorRegistration]` attribute
- **Multi-Target Support** - Compatible with .NET 8.0, 9.0, and 10.0

---

## Installation

```bash
dotnet add package AdaskoTheBeAsT.FluentValidation.SimpleInjector
```

---

## Quick Start

### Basic Usage

The simplest way to register all validators from an assembly:

```cs
using SimpleInjector;
using AdaskoTheBeAsT.FluentValidation.SimpleInjector;

var container = new Container();

// Register all validators from the assembly containing PersonValidator
container.AddFluentValidation(typeof(PersonValidator));
```

That's it! All validators in that assembly are now registered and ready to use.

### Resolve and Use

```cs
var validator = container.GetInstance<IValidator<Person>>();
var result = validator.Validate(new Person { Name = "John" });
```

---

## Usage Patterns

### 1. Register by Marker Types

Use types as markers to identify which assemblies to scan:

```cs
// Single assembly
container.AddFluentValidation(typeof(PersonValidator));

// Multiple assemblies
container.AddFluentValidation(
    typeof(PersonValidator),
    typeof(OrderValidator),
    typeof(ProductValidator)
);
```

### 2. Register by Assemblies

Directly specify assemblies to scan:

```cs
var assembly = typeof(PersonValidator).Assembly;
container.AddFluentValidation(assembly);

// Or multiple assemblies
var assemblies = new[] 
{ 
    typeof(PersonValidator).Assembly,
    typeof(OrderValidator).Assembly 
};
container.AddFluentValidation(assemblies);
```

### 3. Scan Solution Assemblies

Automatically discover and register validators from all assemblies in your solution:

```cs
public static class ValidatorConfig
{
    public static void RegisterValidators(Container container)
    {
        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName.StartsWith("YourCompany."))
            .ToList();

        container.AddFluentValidation(assemblies);
    }
}
```

---

## Advanced Configuration

### Lifecycle Management

Choose how validators are instantiated and cached:

```cs
// Singleton (default) - one instance shared across the application
container.AddFluentValidation(cfg => 
{
    cfg.WithAssembliesToScan(assemblies);
    cfg.AsSingleton();
});

// Scoped - one instance per scope/request
container.AddFluentValidation(cfg => 
{
    cfg.WithAssembliesToScan(assemblies);
    cfg.AsScoped();
});

// Transient - new instance every time
container.AddFluentValidation(cfg => 
{
    cfg.WithAssembliesToScan(assemblies);
    cfg.AsTransient();
});
```

### Registration Patterns

#### Single Validator Pattern (Default)

Registers one validator per type. Use this when you have one validator per model:

```cs
container.AddFluentValidation(cfg =>
{
    cfg.WithAssembliesToScan(assemblies);
    cfg.RegisterAsSingleValidator(); // Default
});

// Resolves to a single IValidator<Person>
var validator = container.GetInstance<IValidator<Person>>();
```

#### Validator Collection Pattern

Register multiple validators for the same type:

```cs
container.AddFluentValidation(cfg =>
{
    cfg.WithAssembliesToScan(assemblies);
    cfg.RegisterAsValidatorCollection();
});

// Resolves to multiple validators
var validators = container.GetAllInstances<IValidator<Person>>();
```

### Selective Registration

Skip specific validators from auto-registration:

```cs
using AdaskoTheBeAsT.FluentValidation.SimpleInjector;

// This validator will be excluded from registration
[SkipValidatorRegistration]
public class PersonInternalValidator : AbstractValidator<Person>
{
    public PersonInternalValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
```

**Use Case**: When building composite validators using [included rules](https://docs.fluentvalidation.net/en/latest/including-rules.html):

```cs
public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        Include(new PersonInternalValidator());
        Include(new PersonExternalValidator());
    }
}

// Mark the included validators to prevent duplicate registration
[SkipValidatorRegistration]
public class PersonInternalValidator : AbstractValidator<Person> { ... }

[SkipValidatorRegistration]
public class PersonExternalValidator : AbstractValidator<Person> { ... }
```

---

## Complete Configuration Example

```cs
using SimpleInjector;
using AdaskoTheBeAsT.FluentValidation.SimpleInjector;

public class Startup
{
    public void ConfigureServices()
    {
        var container = new Container();

        // Advanced configuration
        container.AddFluentValidation(cfg =>
        {
            // Specify assemblies to scan
            cfg.WithAssembliesToScan(
                typeof(PersonValidator).Assembly,
                typeof(OrderValidator).Assembly
            );

            // Set validator lifecycle
            cfg.AsScoped();

            // Use single validator pattern
            cfg.RegisterAsSingleValidator();
        });

        // Verify container configuration
        container.Verify();
    }
}
```

---

## Real-World Examples

### ASP.NET Core Integration

```cs
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add SimpleInjector
        builder.Services.AddSimpleInjector(container =>
        {
            // Register validators with scoped lifetime
            container.AddFluentValidation(cfg =>
            {
                cfg.WithAssembliesToScan(typeof(Program).Assembly);
                cfg.AsScoped();
            });
        });

        var app = builder.Build();
        app.Run();
    }
}
```

### MediatR Pipeline Integration

```cs
// Validation pipeline behavior
public class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await next();
    }
}
```

---

## Framework Support

| .NET Version | Support |
|--------------|---------|
| .NET 10.0    | ✅      |
| .NET 9.0     | ✅      |
| .NET 8.0     | ✅      |

---

## Dependencies

- [FluentValidation](https://www.nuget.org/packages/FluentValidation/) >= 12.1.0
- [SimpleInjector](https://www.nuget.org/packages/SimpleInjector/) >= 5.5.0

---

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Author

**Adam "AdaskoTheBeAsT" Pluciński**

If this library saved you time, consider giving it a ⭐ on GitHub!  